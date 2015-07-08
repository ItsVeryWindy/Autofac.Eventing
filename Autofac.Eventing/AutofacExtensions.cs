using System;
using Autofac.Builder;
using Autofac.Extras.DynamicProxy2;
using Castle.DynamicProxy;
using System.Linq;
using Autofac.Core;

namespace Autofac.Eventing
{
	public static class AutofacExtensions
	{			
		private const string Prefix = "EventInterceptor-";

		public static IEventMetadata<T> RegisterEvent<T>(this IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> obj, string name)
		{
			var eventMetadata = new EventMetadata<T>(name, obj);

			var id = Prefix + Guid.NewGuid();

			obj.OnRegistered(e => {
				var builder = new ContainerBuilder();

				builder.Register(f => new EventInterceptor(
					eventMetadata.EventHandlerTypes
					.Select(g => f.Resolve(g) as IEventHandler)
					.ToList(),eventMetadata.DataProviderTypes
					.Select(g => f.Resolve(g) as IDataProvider)
					.ToList(), eventMetadata))
					.Named<IInterceptor>(id);
				
				builder.Update(e.ComponentRegistry);
			});

			var componentRegistration = obj.CreateRegistration();

			if(componentRegistration.Services.OfType<TypedService>().Any(e => e.ServiceType.IsInterface))
			{
				if(obj.RegistrationData.ActivatingHandlers.Count == 0)
				{
					obj.EnableInterfaceInterceptors();
				}

				obj.InterceptedBy(id);
			}

			return eventMetadata;
		}
	}
}

