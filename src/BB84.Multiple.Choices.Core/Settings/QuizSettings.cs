/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Abstractions.Settings;

namespace BB84.Multiple.Choices.Core.Settings;

/// <summary>
/// Represents the settings for a quiz session.
/// </summary>
public sealed class QuizSettings : IQuizSettings
{
	/// <summary>
	/// Gets or sets the file path to the questions file.
	/// </summary>
	public string QuestionsFilePath { get; set; } = "sampleQuestions.json";

	/// <summary>
	/// Gets or sets the total number of questions per quiz session.
	/// </summary>
	public int QuestionsPerQuiz { get; set; } = 50;

	/// <summary>
	/// Gets or sets the number of questions per quiz round.
	/// </summary>
	public int QuestionsPerRound { get; set; } = 5;

	/// <summary>
	/// Gets or sets if the questions should be randomized.
	/// </summary>
	public bool RandomizeQuestions { get; set; }

	/// <summary>
	/// Gets or sets the threshold score to pass on the next round.
	/// </summary>
	public float ThresholdScorePerRound { get; set; } = 0.5f;
}
