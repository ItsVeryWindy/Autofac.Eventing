using System;

namespace Autofac.Eventing
{
	internal class EventMetadataExceptionFilter
	{
		public Type Type
		{
			get;
			set;
		}
			
		public bool IncludeSubTypes
		{
			get;
			set;
		}

		public EventMetadataExceptionFilter(Type type, bool includeSubTypes)
		{
			IncludeSubTypes = includeSubTypes;
			Type = type;
		}
	}
}

