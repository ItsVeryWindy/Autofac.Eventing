using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Eventing.Tests
{

	class TestDataProvider : IDataProvider
	{
		public void BeforeEvent()
		{
		}

		public Dictionary<string, string> AfterEvent()
		{
			return new Dictionary<string, string> {
				{ "Key", "Value" }
			};
		}
	}
	
}
