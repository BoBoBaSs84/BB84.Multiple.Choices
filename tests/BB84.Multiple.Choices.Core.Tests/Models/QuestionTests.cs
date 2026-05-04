using BB84.Multiple.Choices.Core.Models;

namespace BB84.Multiple.Choices.Core.Tests.Models;

[TestClass]
public sealed class QuestionTests
{
	[TestMethod]
	public void QuestionShouldAssignProperties()
	{
		List<string> answers;
		List<int> correctAnswerIndices;
		Question? question;

		answers = ["Answer A", "Answer B", "Answer C"];
		correctAnswerIndices = [1, 2];

		question = new Question
		{
			Text = "What are valid answers?",
			Answers = answers,
			CorrectAnswerIndices = correctAnswerIndices
		};

		Assert.IsNotNull(question);
		Assert.AreEqual("What are valid answers?", question.Text);
		CollectionAssert.AreEqual(answers, question.Answers);
		CollectionAssert.AreEqual(correctAnswerIndices, question.CorrectAnswerIndices);
	}
}
