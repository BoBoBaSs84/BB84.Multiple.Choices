using BB84.Multiple.Choices.Core.Events;

namespace BB84.Multiple.Choices.Core.Tests.Events;

[TestClass]
public sealed class QuizStartedEventTests
{
	[TestMethod]
	public void QuizStartedEventShouldAssignTitle()
	{
		QuizStartedEvent? @event;

		@event = new QuizStartedEvent("New Quiz");

		Assert.IsNotNull(@event);
		Assert.AreEqual("New Quiz", @event.Title);
		Assert.AreNotEqual(Guid.Empty, @event.Id);
	}
}
