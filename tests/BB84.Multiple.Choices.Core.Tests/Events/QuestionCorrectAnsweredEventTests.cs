using BB84.Multiple.Choices.Core.Events;

namespace BB84.Multiple.Choices.Core.Tests.Events;

[TestClass]
public sealed class QuestionCorrectAnsweredEventTests
{
	[TestMethod]
	public void QuestionCorrectAnsweredEventShouldCreateInstanceWithBaseProperties()
	{
		QuestionCorrectAnsweredEvent? @event;

		@event = new QuestionCorrectAnsweredEvent();

		Assert.IsNotNull(@event);
		Assert.AreNotEqual(Guid.Empty, @event.Id);
		Assert.AreNotEqual(default, @event.Timestamp);
	}
}
