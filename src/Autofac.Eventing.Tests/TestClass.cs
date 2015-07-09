using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Eventing.Tests
{

	class TestClass : ITestClass
	{
		public static Exception Exception
		{
			get;
			private set;
		}

		public static ArgumentException ArgumentException
		{
			get;
			private set;
		}

		static TestClass()
		{
			Exception = new Exception();
			ArgumentException = new ArgumentException();
		}

		public string PropertyA
		{
			get
			{
				return string.Empty;
			}
			set
			{
				
			}
		}

		public void MethodA()
		{
			
		}

		public string MethodB()
		{
			return string.Empty;
		}

		public void MethodC(string a)
		{

		}

		public void ExceptionMethod()
		{
			throw Exception;
		}

		public void ArgumentExceptionMethod()
		{
			throw ArgumentException;
		}
	}

}
