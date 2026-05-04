using BB84.Multiple.Choices.Core.Events;

namespace BB84.Multiple.Choices.Core.Tests.Events;

[TestClass]
public sealed class QuestionsLoadedEventTests
{
	[TestMethod]
	public void QuestionsLoadedEventShouldAssignCount()
	{
		QuestionsLoadedEvent? @event;

		@event = new QuestionsLoadedEvent(12);

		Assert.IsNotNull(@event);
		Assert.AreEqual(12, @event.Count);
		Assert.AreNotEqual(Guid.Empty, @event.Id);
	}
}
