/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Extensions;
using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Abstractions.Settings;
using BB84.Multiple.Choices.Core.Events;
using BB84.Multiple.Choices.Core.Models;

namespace BB84.Multiple.Choices.Core.Services;

/// <summary>
/// Implements quiz logic and user interaction state management.
/// </summary>
/// <param name="eventService">The event service for publishing events.</param>
public sealed class QuizService(IEventService eventService) : IQuizService
{
	private Quiz? _quiz;
	private int _currentRoundIndex;
	private int _currentQuestionIndex;
	private int _score;
	private bool _quizStarted;
	private List<int> _correctAnswersPerRound = [];
	private float _thresholdScorePerRound = 0;

	/// <inheritdoc/>
	public bool LastRoundThresholdNotMet { get; private set; }

	/// <inheritdoc/>
	public Question GetCurrentQuestion()
	{
		return !_quizStarted || _quiz is null
			? throw new InvalidOperationException("No active quiz.")
			: _quiz.Rounds[_currentRoundIndex].Questions[_currentQuestionIndex];
	}

	/// <inheritdoc/>
	public Round GetCurrentRound()
	{
		return !_quizStarted || _quiz is null
			? throw new InvalidOperationException("No active quiz.")
			: _quiz.Rounds[_currentRoundIndex];
	}

	/// <inheritdoc/>
	public void StartQuiz(Quiz quiz)
	{
		_quiz = quiz;
		_currentRoundIndex = 0;
		_currentQuestionIndex = 0;
		_score = 0;
		_quizStarted = true;
		_correctAnswersPerRound = [.. new int[_quiz?.Rounds.Count ?? 0]];

		eventService.Publish(new QuizStartedEvent(quiz.Title));
		eventService.Publish(new RoundStartedEvent(quiz.Rounds[_currentRoundIndex].Title, quiz.Rounds[_currentRoundIndex].Questions.Count));
	}

	/// <inheritdoc/>
	public void StartQuiz(string title, IList<Question> allQuestions, IQuizSettings settings)
	{
		if (allQuestions == null || allQuestions.Count == 0)
			throw new ArgumentException("No questions provided.", nameof(allQuestions));

		if (settings.RandomizeQuestions)
			allQuestions = allQuestions.Randomize();

		_thresholdScorePerRound = settings.ThresholdScorePerRound;

		allQuestions = [.. allQuestions.Take(settings.QuestionsPerQuiz)];

		List<Round> rounds = [.. allQuestions
			.Select((question, i) => new { question, i })
			.GroupBy(x => x.i / settings.QuestionsPerRound)
			.Select((g, idx) => new Round
			{
				Title = $"Round {idx + 1}",
				Questions = [.. g.Select(x => x.question)]
			})];

		var quiz = new Quiz
		{
			Title = title,
			Rounds = rounds
		};

		StartQuiz(quiz);
	}

	/// <inheritdoc/>
	public bool SubmitAnswer(IReadOnlyList<int> answerIndices)
	{
		if (!_quizStarted || _quiz is null || GetIsQuizComplete())
			throw new InvalidOperationException("No active quiz or quiz is complete.");

		Round round = _quiz.Rounds[_currentRoundIndex];
		Question question = round.Questions[_currentQuestionIndex];

		bool isCorrect = answerIndices.Order().SequenceEqual(question.CorrectAnswerIndices.Order());
		if (isCorrect)
		{
			_score++;
			_correctAnswersPerRound[_currentRoundIndex]++;
			eventService.Publish(new QuestionCorrectAnsweredEvent());
		}
		else
		{
			List<string> correctAnswers = [.. question.CorrectAnswerIndices.Select(i => question.Answers[i])];
			eventService.Publish(new QuestionIncorrectAnsweredEvent(correctAnswers));
		}

		// Move to next question or round
		if (_currentQuestionIndex < round.Questions.Count - 1)
		{
			_currentQuestionIndex++;
		}
		else
		{
			// End of round: check threshold
			float roundScore = GetRoundScorePercentage();

			if (roundScore >= _thresholdScorePerRound)
			{
				eventService.Publish(new RoundCompletedEvent(_quiz.Rounds[_currentRoundIndex].Title, roundScore));
				LastRoundThresholdNotMet = false;
				// Allow advancing to next round
				if (_currentRoundIndex < _quiz.Rounds.Count - 1)
				{
					_currentRoundIndex++;
					_currentQuestionIndex = 0;
					eventService.Publish(new RoundStartedEvent(_quiz.Rounds[_currentRoundIndex].Title, _quiz.Rounds[_currentRoundIndex].Questions.Count));
				}
				else
				{
					_quizStarted = false; // Quiz complete
					eventService.Publish(new QuizCompletedEvent(_quiz.Title, GetTotalScorePercentage()));
				}
			}
			else
			{
				LastRoundThresholdNotMet = true;
				// Stay on this round; UI can now check LastRoundThresholdNotMet
			}
		}

		return true;
	}

	/// <inheritdoc/>
	public bool GetIsQuizComplete()
		=> _quiz is null || !_quizStarted;

	/// <inheritdoc/>
	public void RestartQuiz()
	{
		if (_quiz is not null)
			StartQuiz(_quiz);
	}

	private float GetTotalScorePercentage()
	{
		if (_quiz is null || _quiz.Rounds.Count == 0)
			return 0;

		int totalQuestions = _quiz.Rounds.Sum(r => r.Questions.Count);
		return totalQuestions == 0 ? 0 : (float)_score / totalQuestions * 100;
	}

	private float GetRoundScorePercentage()
	{
		if (_quiz is null || _quiz.Rounds.Count == 0)
			return 0;

		Round round = _quiz.Rounds[_currentRoundIndex];
		int correct = _correctAnswersPerRound[_currentRoundIndex];
		int total = round.Questions.Count;
		return total == 0 ? 0 : (float)correct / total * 100;
	}
}
