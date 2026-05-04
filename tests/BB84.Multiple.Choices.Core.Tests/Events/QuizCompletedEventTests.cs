using BB84.Multiple.Choices.Core.Events;

namespace BB84.Multiple.Choices.Core.Tests.Events;

[TestClass]
public sealed class QuizCompletedEventTests
{
	[TestMethod]
	public void QuizCompletedEventShouldAssignTitleAndScore()
	{
		QuizCompletedEvent? @event;

		@event = new QuizCompletedEvent("General Quiz", 0.9f);

		Assert.IsNotNull(@event);
		Assert.AreEqual("General Quiz", @event.Title);
		Assert.AreEqual(0.9f, @event.Score);
		Assert.AreNotEqual(Guid.Empty, @event.Id);
	}
}
