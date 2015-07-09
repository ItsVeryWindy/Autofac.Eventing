using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Autofac.Eventing.Tests
{
	[TestFixture]
	public class Test
	{
		ContainerBuilder _builder;
		IEventMetadata<ITestClass> _eventMetadata;
		const string EventName = "Hello";
		const string EventName2 = "Hello2";

		[SetUp]
		public void SetUp()
		{
			TestEventHandler.Contexts = new List<IEventContext>();

			_builder = new ContainerBuilder();

			_builder.RegisterType<TestEventHandler>();
			_builder.RegisterType<TestDataProvider>();

			_eventMetadata = _builder
				.RegisterType<TestClass>()
				.As<ITestClass>()
				.RegisterEvent<ITestClass>(EventName)
				.UsingEventHandler<TestEventHandler>();
		}

		[Test]
		public void EventRegistration()
		{
			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			Assert.That(result.GetType().Name, Is.EqualTo("ITestClassProxy"));
		}

		[Test]
		public void LoggingEvents()
		{
			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodA();

			Assert.That(TestEventHandler.Contexts, Has.Count.EqualTo(1));
			Assert.That(TestEventHandler.Contexts.First().Name, Is.EqualTo(EventName));
		}

		[Test]
		public void LoggingEventsWithSeparateContext()
		{
			_eventMetadata
				.RegisterEvent(EventName2)
				.UsingEventHandler<TestEventHandler>();

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodA();

			Assert.That(TestEventHandler.Contexts, Has.Count.EqualTo(2));
			Assert.That(TestEventHandler.Contexts.First().Name, Is.EqualTo(EventName2));
			Assert.That(TestEventHandler.Contexts.Last().Name, Is.EqualTo(EventName));
		}

		[Test]
		public void EventOnExceptionOnlyWithNoExceptionThrown()
		{
			_eventMetadata
				.OnlyOnException<Exception>();

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodA();

			Assert.That(TestEventHandler.Contexts, Is.Empty);
		}

		[Test]
		public void EventOnExceptionOnlyWithExceptionThrown()
		{
			_eventMetadata
				.OnlyOnException<Exception>();

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			Assert.That(result.ArgumentExceptionMethod, Throws.Exception);

			Assert.That(TestEventHandler.Contexts, Has.Count.EqualTo(1));
			Assert.That(TestEventHandler.Contexts.First().Name, Is.EqualTo(EventName));
			Assert.That(TestEventHandler.Contexts.First().Exception, Is.EqualTo(TestClass.ArgumentException));
		}

		[Test]
		public void EventOnExceptionOnlyWithExceptionThrownNoSubtypes()
		{
			_eventMetadata
				.OnlyOnException<Exception>(false);

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			Assert.That(result.ArgumentExceptionMethod, Throws.ArgumentException);

			Assert.That(TestEventHandler.Contexts, Is.Empty);
		}

		[Test]
		public void EventOnCorrectMethodWithMethodFilterWithIncorrectReturnValue()
		{
			_eventMetadata
				.OnlyOnReturn(e => e.MethodB(), x => x == "Hi");

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodB();

			Assert.That(TestEventHandler.Contexts, Is.Empty);
		}

		[Test]
		public void EventOnCorrectMethodWithMethodFilterWithCorrectReturnValue()
		{
			_eventMetadata
				.OnlyOnReturn(e => e.MethodB(), x => x == string.Empty);

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodB();

			Assert.That(TestEventHandler.Contexts, Has.Count.EqualTo(1));
			Assert.That(TestEventHandler.Contexts.First().Name, Is.EqualTo(EventName));		}

		[Test]
		public void EventOnAnotherMethodWithMethodFilterWithNoParameters()
		{
			_eventMetadata
				.OnlyOn(e => e.MethodB());

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodA();

			Assert.That(TestEventHandler.Contexts, Is.Empty);
		}

		[Test]
		public void EventOnCorrectMethodWithMethodFilterWithNoParameters()
		{
			_eventMetadata
				.OnlyOn(e => e.MethodA());

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodA();

			Assert.That(TestEventHandler.Contexts, Has.Count.EqualTo(1));
			Assert.That(TestEventHandler.Contexts.First().Name, Is.EqualTo(EventName));
		}

		[Test]
		public void EventOnCorrectMethodWithMethodFilterWithWrongParameters()
		{
			_eventMetadata
				.OnlyOn(e => e.MethodC("Hello"));

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodC("Goodbye");

			Assert.That(TestEventHandler.Contexts, Is.Empty);
		}

		[Test]
		public void EventOnCorrectMethodWithMethodFilterWithCorrectParameters()
		{
			_eventMetadata
				.OnlyOn(e => e.MethodC("Hello"));

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodC("Hello");

			Assert.That(TestEventHandler.Contexts, Has.Count.EqualTo(1));
			Assert.That(TestEventHandler.Contexts.First().Name, Is.EqualTo(EventName));
		}

		[Test]
		public void EventOnAnotherMethodWithGetPropertyFilter()
		{
			_eventMetadata
				.OnlyOn(e => e.PropertyA);

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodA();

			Assert.That(TestEventHandler.Contexts, Is.Empty);
		}

		[Test]
		public void EventOnCorrectMethodWithGetPropertyFilter()
		{
			_eventMetadata
				.OnlyOn(e => e.PropertyA);

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			var value = result.PropertyA;

			Assert.That(TestEventHandler.Contexts, Has.Count.EqualTo(1));
			Assert.That(TestEventHandler.Contexts.First().Name, Is.EqualTo(EventName));
		}

		/*[Test]
		public void EventOnAnotherMethodWithSetPropertyFilter()
		{
			_eventMetadata.UsingEventHandler<TestEventHandler>()
				.OnlyOn(e => e.PropertyA = "Hello");

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodA();

			Assert.That(TestEventHandler.Contexts, Is.Empty);
		}

		[Test]
		public void EventOnCorrectMethodWithSetPropertyFilter()
		{
			_eventMetadata.UsingEventHandler<TestEventHandler>()
				.OnlyOn(e => e.PropertyA = "Hello");

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			var value = result.PropertyA;

			Assert.That(TestEventHandler.Contexts, Has.Count.EqualTo(1));
			Assert.That(TestEventHandler.Contexts.First().Name, Is.EqualTo(EventName));
		}*/

		[Test]
		public void LoggingEventsWithDataProvider()
		{
			_eventMetadata
				.UsingDataProvider<TestDataProvider>();

			var container = _builder.Build();

			var result = container.Resolve<ITestClass>();

			result.MethodA();

			Assert.That(TestEventHandler.Contexts, Has.Count.EqualTo(1));
			Assert.That(TestEventHandler.Contexts.First().Name, Is.EqualTo(EventName));
			Assert.That(TestEventHandler.Contexts.First().Data["Key"], Is.EqualTo("Value"));
		}
	}
}

