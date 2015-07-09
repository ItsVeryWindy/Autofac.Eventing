using System;

namespace Autofac.Eventing
{
	public interface IEventHandler
	{
		void OnEvent(IEventContext context);
	}
}

