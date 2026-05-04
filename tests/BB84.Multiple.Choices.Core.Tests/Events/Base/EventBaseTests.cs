using BB84.Multiple.Choices.Core.Events.Base;

namespace BB84.Multiple.Choices.Core.Tests.Events.Base;

[TestClass]
public sealed class EventBaseTests
{
	private sealed class TestEvent : EventBase
	{ }

	[TestMethod]
	public void EventBaseShouldAssignIdAndTimestamp()
	{
		DateTimeOffset beforeCreation;
		DateTimeOffset afterCreation;
		TestEvent? @event;

		beforeCreation = DateTimeOffset.UtcNow;
		@event = new TestEvent();
		afterCreation = DateTimeOffset.UtcNow;

		Assert.IsNotNull(@event);
		Assert.AreNotEqual(Guid.Empty, @event.Id);
		Assert.IsGreaterThanOrEqualTo(beforeCreation, @event.Timestamp);
		Assert.IsLessThanOrEqualTo(afterCreation, @event.Timestamp);
	}
}
