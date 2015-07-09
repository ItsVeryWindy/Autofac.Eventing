using System;
using System.Collections.Generic;

namespace Autofac.Eventing
{
	public interface IDataProvider
	{
		void BeforeEvent();
		Dictionary<string, string> AfterEvent();
	}
}

