using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Abstractions.Settings;
using BB84.Multiple.Choices.Core.Events;
using BB84.Multiple.Choices.Core.Models;
using BB84.Multiple.Choices.Core.Services;

using Moq;

namespace BB84.Multiple.Choices.Core.Tests.Services;

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
	public void ImplementsIQuizService()
		=> Assert.IsInstanceOfType<IQuizService>(_sut);

	[TestMethod]
	public void GetCurrentRoundWithoutActiveQuizShouldThrowInvalidOperationException()
		=> Assert.Throws<InvalidOperationException>(() => _sut.GetCurrentRound());

	[TestMethod]
	public void GetCurrentQuestionWithoutActiveQuizShouldThrowInvalidOperationException()
		=> Assert.Throws<InvalidOperationException>(() => _sut.GetCurrentQuestion());

	[TestMethod]
	public void StartQuizWithQuizShouldPublishStartedEvents()
	{
		Quiz quiz;

		quiz = CreateQuiz(questionsPerRound: 1, roundsCount: 1);

		_sut.StartQuiz(quiz);

		_eventServiceMock.Verify(x => x.Publish(It.IsAny<QuizStartedEvent>()), Times.Once);
		_eventServiceMock.Verify(x => x.Publish(It.IsAny<RoundStartedEvent>()), Times.Once);
	}

	[TestMethod]
	public void StartQuizWithQuestionsAndEmptyCollectionShouldThrowArgumentException()
	{
		Mock<IQuizSettings> settingsMock;

		settingsMock = new Mock<IQuizSettings>();
		settingsMock.SetupGet(x => x.RandomizeQuestions).Returns(false);
		settingsMock.SetupGet(x => x.ThresholdScorePerRound).Returns(50f);
		settingsMock.SetupGet(x => x.QuestionsPerQuiz).Returns(10);
		settingsMock.SetupGet(x => x.QuestionsPerRound).Returns(5);

		Assert.Throws<ArgumentException>(() => _sut.StartQuiz("Quiz", [], settingsMock.Object));
	}

	[TestMethod]
	public void SubmitAnswerWithCorrectAnswerShouldPublishQuestionCorrectAnsweredEvent()
	{
		Quiz quiz;

		quiz = CreateQuiz(questionsPerRound: 1, roundsCount: 1);
		_sut.StartQuiz(quiz);

		_sut.SubmitAnswer([0]);

		_eventServiceMock.Verify(x => x.Publish(It.IsAny<QuestionCorrectAnsweredEvent>()), Times.Once);
	}

	[TestMethod]
	public void SubmitAnswerWithIncorrectAnswerShouldPublishQuestionIncorrectAnsweredEvent()
	{
		Quiz quiz;

		quiz = CreateQuiz(questionsPerRound: 1, roundsCount: 1);
		_sut.StartQuiz(quiz);

		_sut.SubmitAnswer([1]);

		List<string> sequence = ["A"];
		_eventServiceMock.Verify(
			x => x.Publish(It.Is<QuestionIncorrectAnsweredEvent>(e => e.CorrectAnswers.SequenceEqual(sequence))),
			Times.Once);
	}

	[TestMethod]
	public void SubmitAnswerWhenRoundThresholdNotMetShouldSetLastRoundThresholdNotMet()
	{
		Mock<IQuizSettings> settingsMock;
		List<Question> questions;

		questions =
		[
			new Question
			{
				Text = "Q1",
				Answers = ["A", "B"],
				CorrectAnswerIndices = [0]
			}
		];
		settingsMock = CreateSettingsMock(randomizeQuestions: false, thresholdScorePerRound: 100f, questionsPerQuiz: 1, questionsPerRound: 1);

		_sut.StartQuiz("Quiz", questions, settingsMock.Object);
		_sut.SubmitAnswer([1]);

		Assert.IsTrue(_sut.LastRoundThresholdNotMet);
	}

	[TestMethod]
	public void SubmitAnswerWhenRoundThresholdMetShouldStartNextRound()
	{
		Mock<IQuizSettings> settingsMock;
		List<Question> questions;

		questions =
		[
			new Question { Text = "Q1", Answers = ["A", "B"], CorrectAnswerIndices = [0] },
			new Question { Text = "Q2", Answers = ["A", "B"], CorrectAnswerIndices = [0] }
		];
		settingsMock = CreateSettingsMock(randomizeQuestions: false, thresholdScorePerRound: 100f, questionsPerQuiz: 2, questionsPerRound: 1);

		_sut.StartQuiz("Quiz", questions, settingsMock.Object);
		_sut.SubmitAnswer([0]);

		Assert.IsFalse(_sut.LastRoundThresholdNotMet);
		Assert.AreEqual("Q2", _sut.GetCurrentQuestion().Text);
		_eventServiceMock.Verify(x => x.Publish(It.IsAny<RoundCompletedEvent>()), Times.Once);
		_eventServiceMock.Verify(x => x.Publish(It.IsAny<RoundStartedEvent>()), Times.Exactly(2));
	}

	[TestMethod]
	public void SubmitAnswerWhenLastRoundCompletedShouldCompleteQuizAndPublishQuizCompletedEvent()
	{
		Quiz quiz;

		quiz = CreateQuiz(questionsPerRound: 1, roundsCount: 1);
		_sut.StartQuiz(quiz);

		_sut.SubmitAnswer([0]);

		Assert.IsTrue(_sut.GetIsQuizComplete());
		_eventServiceMock.Verify(x => x.Publish(It.IsAny<QuizCompletedEvent>()), Times.Once);
	}

	[TestMethod]
	public void RestartQuizShouldResetCurrentQuestionToFirst()
	{
		Quiz quiz;

		quiz = CreateQuiz(questionsPerRound: 2, roundsCount: 1);
		_sut.StartQuiz(quiz);
		_sut.SubmitAnswer([0]);

		Assert.AreEqual("Question 2", _sut.GetCurrentQuestion().Text);

		_sut.RestartQuiz();

		Assert.AreEqual("Question 1", _sut.GetCurrentQuestion().Text);
	}

	private static Mock<IQuizSettings> CreateSettingsMock(bool randomizeQuestions, float thresholdScorePerRound, int questionsPerQuiz, int questionsPerRound)
	{
		Mock<IQuizSettings> settingsMock;

		settingsMock = new Mock<IQuizSettings>();
		settingsMock.SetupGet(x => x.RandomizeQuestions).Returns(randomizeQuestions);
		settingsMock.SetupGet(x => x.ThresholdScorePerRound).Returns(thresholdScorePerRound);
		settingsMock.SetupGet(x => x.QuestionsPerQuiz).Returns(questionsPerQuiz);
		settingsMock.SetupGet(x => x.QuestionsPerRound).Returns(questionsPerRound);

		return settingsMock;
	}

	private static Quiz CreateQuiz(int questionsPerRound, int roundsCount)
	{
		List<Round> rounds;
		int questionIndex;

		rounds = [];
		questionIndex = 1;

		for (int roundIndex = 1; roundIndex <= roundsCount; roundIndex++)
		{
			List<Question> questions;

			questions = [];
			for (int i = 0; i < questionsPerRound; i++)
			{
				questions.Add(new Question
				{
					Text = $"Question {questionIndex++}",
					Answers = ["A", "B", "C"],
					CorrectAnswerIndices = [0]
				});
			}

			rounds.Add(new Round
			{
				Title = $"Round {roundIndex}",
				Questions = questions
			});
		}

		return new Quiz
		{
			Title = "Sample Quiz",
			Rounds = rounds
		};
	}
}
