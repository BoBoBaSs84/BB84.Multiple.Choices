/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Abstractions.Settings;
using BB84.Multiple.Choices.Core.Models;

namespace BB84.Multiple.Choices.Core.Abstractions.Services;

/// <summary>
/// Represents a service contract for managing quizzes.
/// </summary>
public interface IQuizService
{
	/// <summary>
	/// Inindicates whether the last round's threshold was not met.
	/// </summary>
	bool LastRoundThresholdNotMet { get; }

	/// <summary>
	/// Retrieves the current round in the quiz.
	/// </summary>
	/// <returns>
	/// The current <see cref="Round"/> object.
	/// </returns>
	Round GetCurrentRound();

	/// <summary>
	/// Retrieves the current question in the quiz.
	/// </summary>
	/// <returns>
	/// The current <see cref="Question"/> object.
	/// </returns>
	Question GetCurrentQuestion();

	/// <summary>
	/// Starts a new quiz session with the specified quiz.
	/// </summary>
	/// <param name="quiz"></param>
	void StartQuiz(Quiz quiz);

	/// <summary>
	/// Starts a new quiz session with the specified questions and number of questions per round.
	/// </summary>
	/// <param name="title">The title of the quiz.</param>
	/// <param name="allQuestions">The list of all questions for the quiz.</param>
	/// <param name="settings">The quiz command settings to use.</param>
	void StartQuiz(string title, IList<Question> allQuestions, IQuizSettings settings);

	/// <summary>
	/// Submits answers for the current question.
	/// </summary>
	/// <param name="answerIndices">The indices of the selected answers.</param>
	/// <returns>
	/// True if the answers were submitted successfully, false otherwise.
	/// </returns>
	bool SubmitAnswer(IReadOnlyList<int> answerIndices);

	/// <summary>
	/// Indicates whether the quiz is complete.
	/// </summary>
	/// <returns>
	/// True if the quiz is complete, false otherwise.
	/// </returns>
	bool GetIsQuizComplete();

	/// <summary>
	/// Restarts the quiz from the beginning.
	/// </summary>
	void RestartQuiz();
}
