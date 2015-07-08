using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Autofac.Eventing
{
	internal interface IEventMetadataInception
	{
		string Name
		{
			get;
		}

		IEnumerable<EventMetadataExceptionFilter> ExceptionFilters
		{
			get;
		}

		IEnumerable<LambdaExpression> MethodFilters
		{
			get;
		}

		IEnumerable<Tuple<LambdaExpression, Func<object, bool>>> MethodReturnFilters
		{
			get;
		}
	}
}

