using BB84.Multiple.Choices.Core.Models;

namespace BB84.Multiple.Choices.Core.Tests.Models;

[TestClass]
public sealed class QuizTests
{
	[TestMethod]
	public void QuizShouldAssignProperties()
	{
		Round round;
		List<Round> rounds;
		Quiz? quiz;

		round = new Round
		{
			Title = "Science Round",
			Questions =
			[
				new Question
				{
					Text = "What planet is known as the red planet?",
					Answers = ["Earth", "Mars", "Venus"],
					CorrectAnswerIndices = [1]
				}
			]
		};
		rounds = [round];

		quiz = new Quiz
		{
			Title = "General Knowledge",
			Rounds = rounds
		};

		Assert.IsNotNull(quiz);
		Assert.AreEqual("General Knowledge", quiz.Title);
		CollectionAssert.AreEqual(rounds, quiz.Rounds);
	}
}
