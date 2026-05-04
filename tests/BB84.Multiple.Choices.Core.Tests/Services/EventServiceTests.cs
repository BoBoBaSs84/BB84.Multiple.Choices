using BB84.Multiple.Choices.Core.Abstractions.Events;
using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Services;

namespace BB84.Multiple.Choices.Core.Tests.Services;

[TestClass]
public sealed class EventServiceTests
{
	private sealed class TestEvent : IEvent
	{
		public Guid Id { get; } = Guid.NewGuid();
		public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
		public string Payload { get; init; } = string.Empty;
	}

	private sealed class OtherTestEvent : IEvent
	{
		public Guid Id { get; } = Guid.NewGuid();
		public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
	}

	[TestMethod]
	public void ImplementsIEventService()
	{
		EventService sut;

		sut = new EventService();

		Assert.IsInstanceOfType<IEventService>(sut);
	}

	[TestMethod]
	public void SubscribeAndPublishShouldInvokeHandler()
	{
		EventService sut;
		TestEvent publishedEvent;
		TestEvent? receivedEvent;

		sut = new EventService();
		receivedEvent = null;
		publishedEvent = new TestEvent { Payload = "payload" };

		sut.Subscribe<TestEvent>(@event => receivedEvent = @event);
		sut.Publish(publishedEvent);

		Assert.IsNotNull(receivedEvent);
		Assert.AreSame(publishedEvent, receivedEvent);
		Assert.AreEqual("payload", receivedEvent.Payload);
	}

	[TestMethod]
	public void PublishWithNoSubscribersShouldNotThrow()
	{
		EventService sut;

		sut = new EventService();

		sut.Publish(new TestEvent());
	}

	[TestMethod]
	public void PublishShouldInvokeAllSubscribersForSameEventType()
	{
		EventService sut;
		int invokeCount;

		sut = new EventService();
		invokeCount = 0;

		sut.Subscribe<TestEvent>(_ => invokeCount++);
		sut.Subscribe<TestEvent>(_ => invokeCount++);

		sut.Publish(new TestEvent());

		Assert.AreEqual(2, invokeCount);
	}

	[TestMethod]
	public void PublishShouldNotInvokeSubscribersOfOtherEventTypes()
	{
		EventService sut;
		bool wasCalled;

		sut = new EventService();
		wasCalled = false;

		sut.Subscribe<OtherTestEvent>(_ => wasCalled = true);
		sut.Publish(new TestEvent());

		Assert.IsFalse(wasCalled);
	}
}
