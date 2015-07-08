using System;
using System.Collections.ObjectModel;

namespace Autofac.Eventing
{
	public interface IEventContext
	{
		string Name { get; }

		object Exception { get;	}

		ReadOnlyDictionary<string, string> Data
		{
			get;
		}
	}
}

