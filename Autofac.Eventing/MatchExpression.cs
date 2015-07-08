using System;
using System.Reflection;
using System.Linq.Expressions;

namespace Autofac.Eventing
{
	internal static class MatchExpression
	{
		public static bool Match(LambdaExpression expression, MethodInfo method, object[] arguments)
		{
			var methodCall = expression.Body as MethodCallExpression;

			if(methodCall != null)
			{
				return MatchMethod(methodCall, method, arguments);
			}

			var propertyCall = expression.Body as MemberExpression;

			if(propertyCall != null)
			{
				return MatchMember(propertyCall, method, arguments);
			}

			//var propertySetter = expression.Body as UnaryExpression;

			//propertySetter.

			throw new NotSupportedException();
		}

		private static bool MatchMethod(MethodCallExpression expression, MethodInfo method, object[] arguments)
		{
			if(expression.Arguments.Count != arguments.Length || expression.Method != method)
			{
				return false;
			}

			for(var i = 0; i < arguments.Length; i++)
			{
				var constantExpression = expression.Arguments[i] as ConstantExpression;

				if(constantExpression != null && !object.Equals(constantExpression.Value, arguments[i]))
				{
					return false;
				}
			}

			return true;
		}

		private static bool MatchMember(MemberExpression expression, MethodInfo method, object[] arguments)
		{
			var property = expression.Member as PropertyInfo;
			 
			if(property == null)
			{
				throw new NotSupportedException();
			}

			return property.GetGetMethod() == method;
		}
	}
}

