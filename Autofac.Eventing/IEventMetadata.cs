using System;
using System.Linq.Expressions;

namespace Autofac.Eventing
{
	public interface IEventMetadata<T>
	{
		IEventMetadata<T> UsingEventHandler<THandler>() where THandler : IEventHandler;
		IEventMetadata<T> UsingDataProvider<TProvider>() where TProvider : IDataProvider;
		IEventMetadata<T> RegisterEvent(string name);
		IEventMetadata<T> OnlyOnException<TException>(bool isAssignableTo = true);
		IEventMetadata<T> OnlyOn(Expression<Action<T>> expression);
		IEventMetadata<T> OnlyOn(Expression<Func<T, object>> expression);
		IEventMetadata<T> OnlyOnReturn<TReturn>(Expression<Func<T, TReturn>> expression, Func<TReturn, bool> returnValue);
	}
}

