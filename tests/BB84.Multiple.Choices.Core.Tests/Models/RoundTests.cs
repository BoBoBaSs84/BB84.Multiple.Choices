using BB84.Multiple.Choices.Core.Models;

namespace BB84.Multiple.Choices.Core.Tests.Models;

[TestClass]
public sealed class RoundTests
{
	[TestMethod]
	public void RoundShouldAssignProperties()
	{
		Question question;
		List<Question> questions;
		Round? round;

		question = new Question
		{
			Text = "What is 2 + 2?",
			Answers = ["3", "4", "5"],
			CorrectAnswerIndices = [1]
		};
		questions = [question];

		round = new Round
		{
			Title = "Math Round",
			Questions = questions
		};

		Assert.IsNotNull(round);
		Assert.AreEqual("Math Round", round.Title);
		CollectionAssert.AreEqual(questions, round.Questions);
	}
}
