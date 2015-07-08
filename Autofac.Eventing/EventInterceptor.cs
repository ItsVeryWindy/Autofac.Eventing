using System;
using Castle.DynamicProxy;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Eventing
{
	internal class EventInterceptor : IInterceptor
	{
		IEventMetadataInception _eventMetadata;
		IEnumerable<IEventHandler> _eventHandlers;
		IEnumerable<IDataProvider> _dataProviders;

		public EventInterceptor(IEnumerable<IEventHandler> eventHandlers, IEnumerable<IDataProvider> dataProviders, IEventMetadataInception eventMetadata)
		{
			_eventHandlers = eventHandlers;
			_dataProviders = dataProviders;
			_eventMetadata = eventMetadata;
		}

		public void Intercept(IInvocation invocation)
		{
			var canExecute = !_eventMetadata.MethodFilters.Any() || _eventMetadata.MethodFilters.Any(e => MatchExpression.Match(e, invocation.Method, invocation.Arguments));

			var returnMethodFilters = _eventMetadata.MethodReturnFilters.Where(e => MatchExpression.Match(e.Item1, invocation.Method, invocation.Arguments)).Select(e => e.Item2).ToList();

			if(canExecute)
			{
				foreach(var i in _dataProviders)
				{
					i.BeforeEvent();
				}
			}

			try
			{
				invocation.Proceed();
			}
			catch(Exception ex)
			{
				if(canExecute && _eventMetadata.ExceptionFilters.Any() && _eventMetadata.ExceptionFilters.Any(e => ex.GetType() == e.Type || (e.IncludeSubTypes && e.Type.IsInstanceOfType(ex))))
				{
					ExecuteEvents(ex);
				}

				throw;
			}

			if(canExecute && !_eventMetadata.ExceptionFilters.Any() && (!returnMethodFilters.Any() || returnMethodFilters.Any(e => e(invocation.ReturnValue))))
			{
				ExecuteEvents();
			}
		}
			
		private void ExecuteEvents(object ex = null)
		{
			var dict = new Dictionary<string, string>();

			foreach(var i in _dataProviders) {
				var results = i.AfterEvent();

				if(results != null) {
					foreach(var j in results) {
						dict[j.Key] = j.Value;
					}
				}
			}
				
			var eventContext = new EventContext(dict)
			{
				Name = _eventMetadata.Name,
				Exception = ex
			};
					
			foreach(var i in _eventHandlers) {
				i.OnEvent(eventContext);
			}

		}
	}

}

