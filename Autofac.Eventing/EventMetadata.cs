using System;
using System.Collections.Generic;
using Autofac.Builder;
using Autofac.Core;
using System.Linq.Expressions;

namespace Autofac.Eventing
{
	internal class EventMetadata<T> : IEventMetadata<T>, IEventMetadataInception
	{
		public IEventMetadata<T> OnlyOnReturn<TReturn>(Expression<Func<T, TReturn>> expression, Func<TReturn, bool> returnValue)
		{
			_methodReturnFilters.Add(new Tuple<LambdaExpression, Func<object, bool>>(expression, x => x is TReturn && returnValue((TReturn)x)));

			return this;
		}

		private HashSet<Type> _eventHandlerTypes = new HashSet<Type>();
		private HashSet<Type> _dataProviderTypes = new HashSet<Type>();
		private List<EventMetadataExceptionFilter> _exceptionFilters = new List<EventMetadataExceptionFilter>();
		private List<LambdaExpression> _methodFilters = new List<LambdaExpression>();
		private List<Tuple<LambdaExpression, Func<object, bool>>> _methodReturnFilters = new List<Tuple<LambdaExpression, Func<object, bool>>>();
		IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> _instance;

		public EventMetadata(string name, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> instance)
		{
			_instance = instance;
			Name = name;
		}

		public IEventMetadata<T> UsingEventHandler<THandler>() where THandler : IEventHandler
		{
			_eventHandlerTypes.Add(typeof(THandler));

			return this;
		}

		public IEventMetadata<T> RegisterEvent(string name)
		{
			return _instance.RegisterEvent<T>(name);
		}

		public IEnumerable<Type> EventHandlerTypes
		{
			get
			{
				return _eventHandlerTypes;
			}
		}

		public IEnumerable<Type> DataProviderTypes
		{
			get
			{
				return _dataProviderTypes;
			}
		}

		public IEnumerable<EventMetadataExceptionFilter> ExceptionFilters
		{
			get
			{
				return _exceptionFilters;
			}
		}

		public IEnumerable<LambdaExpression> MethodFilters
		{
			get
			{
				return _methodFilters;
			}
		}

		public IEnumerable<Tuple<LambdaExpression, Func<object, bool>>> MethodReturnFilters
		{
			get
			{
				return _methodReturnFilters;
			}
		}

		public IEventMetadata<T> OnlyOnException<TException>(bool includeSubtypes = true)
		{
			_exceptionFilters.Add(new EventMetadataExceptionFilter(typeof(TException), includeSubtypes));

			return this;
		}

		public string Name
		{
			get;
			private set;
		}

		public IEventMetadata<T> OnlyOn(Expression<Action<T>> expression)
		{
			_methodFilters.Add(expression);

			return this;
		}

		public IEventMetadata<T> OnlyOn(Expression<Func<T, object>> expression)
		{
			_methodFilters.Add(expression);

			return this;
		}

		public IEventMetadata<T> UsingDataProvider<TProvider>() where TProvider : IDataProvider
		{
			_dataProviderTypes.Add(typeof(TProvider));

			return this;
		}
	}
}

