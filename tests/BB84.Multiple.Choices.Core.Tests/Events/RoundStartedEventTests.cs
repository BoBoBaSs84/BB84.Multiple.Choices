using BB84.Multiple.Choices.Core.Events;

namespace BB84.Multiple.Choices.Core.Tests.Events;

[TestClass]
public sealed class RoundStartedEventTests
{
	[TestMethod]
	public void RoundStartedEventShouldAssignTitleAndQuestionsCount()
	{
		RoundStartedEvent? @event;

		@event = new RoundStartedEvent("Round 2", 10);

		Assert.IsNotNull(@event);
		Assert.AreEqual("Round 2", @event.Title);
		Assert.AreEqual(10, @event.QuestionsCount);
		Assert.AreNotEqual(Guid.Empty, @event.Id);
	}
}
