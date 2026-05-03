/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
namespace BB84.Multiple.Choices.Core.Abstractions.Settings;

/// <summary>
/// Represents the settings abstraction for a quiz session.
/// </summary>
public interface IQuizSettings
{
	/// <summary>
	/// Gets or sets the file path to the questions file.
	/// </summary>
	string QuestionsFilePath { get; set; }

	/// <summary>
	/// Gets or sets the total number of questions per quiz session.
	/// </summary>
	int QuestionsPerQuiz { get; set; }

	/// <summary>
	/// Gets or sets the number of questions per quiz round.
	/// </summary>
	int QuestionsPerRound { get; set; }

	/// <summary>
	/// Gets or sets if the questions should be randomized.
	/// </summary>
	bool RandomizeQuestions { get; set; }

	/// <summary>
	/// Gets or sets the threshold score to pass on the next round.
	/// </summary>
	float ThresholdScorePerRound { get; set; }
}
