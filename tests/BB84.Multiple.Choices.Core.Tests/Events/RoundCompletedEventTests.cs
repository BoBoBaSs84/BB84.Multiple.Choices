using BB84.Multiple.Choices.Core.Events;

namespace BB84.Multiple.Choices.Core.Tests.Events;

[TestClass]
public sealed class RoundCompletedEventTests
{
	[TestMethod]
	public void RoundCompletedEventShouldAssignTitleAndScore()
	{
		RoundCompletedEvent? @event;

		@event = new RoundCompletedEvent("Round 1", 0.8f);

		Assert.IsNotNull(@event);
		Assert.AreEqual("Round 1", @event.Title);
		Assert.AreEqual(0.8f, @event.Score);
		Assert.AreNotEqual(Guid.Empty, @event.Id);
	}
}
