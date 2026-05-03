/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Models;
using BB84.Multiple.Choices.Core.Services;

using Moq;

namespace BB84.Multiple.ChoicesTests.Services;

[TestClass]
public sealed class QuizServiceTests
{
	private readonly Mock<IEventService> _eventServiceMock;
	private readonly QuizService _sut;

	public QuizServiceTests()
	{
		_eventServiceMock = new Mock<IEventService>();
		_sut = new QuizService(_eventServiceMock.Object);
	}

	[TestMethod]
	public void Implements_IQuizService()
		=> Assert.IsInstanceOfType<IQuizService>(_sut);

	[TestMethod]
	public void StartQuiz_WithValidQuiz_DoesNotThrowArgumentNullException()
	{
		Quiz quiz = CreateQuiz();

		_sut.StartQuiz(quiz);

		Assert.IsNotNull(_sut.GetCurrentRound());
		Assert.IsNotNull(_sut.GetCurrentQuestion());
	}

	[TestMethod]
	public void GetCurrentQuestion_AfterStartingQuiz_ReturnsFirstQuestion()
	{
		Quiz quiz = CreateQuiz();
		_sut.StartQuiz(quiz);

		Question? currentQuestion = _sut.GetCurrentQuestion();

		Assert.IsNotNull(currentQuestion);
		Assert.AreEqual(quiz.Rounds[0].Questions[0], currentQuestion);
	}

	[TestMethod]
	public void SubmitAnswer_WithoutStartingQuiz_ThrowsInvalidOperationException()
		=> Assert.Throws<InvalidOperationException>(() => _sut.SubmitAnswer([0]));

	[TestMethod]
	public void SubmitAnswer_AfterStartingQuiz_WithCorrectAnswers_DoesNotThrow()
	{
		Quiz quiz = CreateQuiz();
		_sut.StartQuiz(quiz);
		_sut.SubmitAnswer([0, 2]);
	}

	[TestMethod]
	public void SubmitAnswer_AfterStartingQuiz_WithIncorrectAnswers_DoesNotThrow()
	{
		Quiz quiz = CreateQuiz();
		_sut.StartQuiz(quiz);
		_sut.SubmitAnswer([1]);
	}

	private static Quiz CreateQuiz()
	{
		return new()
		{
			Title = "Sample Quiz",
			Rounds =
			[
				new Round
				{
					Title = "Round 1",
					Questions = [
						new Question
						{
							Text = "Sample Question?",
							Answers = ["A", "B", "C", "D"],
							CorrectAnswerIndices = [0, 2]
						}
					]
				}
			]
		};
	}
}
