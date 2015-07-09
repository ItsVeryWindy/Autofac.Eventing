using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Autofac.Eventing
{
	public class EventContext : IEventContext
	{
		public string Name
		{
			get;
			set;
		}
		
		public object Exception
		{
			get;
			set;
		}

		public ReadOnlyDictionary<string, string> Data
		{
			get;
			private set;
		}

		public EventContext(IDictionary<string, string> dict)
		{
			Data = new ReadOnlyDictionary<string, string>(dict);
		}
	}

}

