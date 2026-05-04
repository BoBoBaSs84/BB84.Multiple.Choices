using BB84.Multiple.Choices.Core.Events;

namespace BB84.Multiple.Choices.Core.Tests.Events;

[TestClass]
public sealed class QuestionIncorrectAnsweredEventTests
{
	[TestMethod]
	public void QuestionIncorrectAnsweredEventShouldAssignCorrectAnswers()
	{
		IReadOnlyList<string> correctAnswers;
		QuestionIncorrectAnsweredEvent? @event;

		correctAnswers = ["Answer A", "Answer C"];
		@event = new QuestionIncorrectAnsweredEvent(correctAnswers);

		Assert.IsNotNull(@event);
		CollectionAssert.AreEqual(correctAnswers.ToList(), @event.CorrectAnswers.ToList());
		Assert.AreNotEqual(Guid.Empty, @event.Id);
	}
}
