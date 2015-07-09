using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Eventing.Tests
{

	class TestEventHandler : IEventHandler
	{
		public static List<IEventContext> Contexts
		{
			get;
			set;
		}

		static TestEventHandler()
		{
			Contexts = new List<IEventContext>();
		}

		public void OnEvent(IEventContext context)
		{
			Contexts.Add(context);
		}
	}
}
