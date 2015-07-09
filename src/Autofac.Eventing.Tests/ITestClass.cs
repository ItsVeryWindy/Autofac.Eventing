using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Eventing.Tests
{

	public interface ITestClass
	{
		void MethodA();
		string MethodB();
		void MethodC(string a);

		string PropertyA
		{
			get;
			set;
		}

		void ExceptionMethod();
		void ArgumentExceptionMethod();
	}

}
