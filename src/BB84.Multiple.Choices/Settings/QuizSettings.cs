/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using BB84.Multiple.Choices.Core.Abstractions.Settings;

using Spectre.Console.Cli;

namespace BB84.Multiple.Choices.Settings;

/// <summary>
/// Represents the settings for the quiz command.
/// </summary>
public sealed class QuizSettings : CommandSettings, IQuizSettings
{
	/// <summary>
	/// Gets or sets the file path to the questions file.
	/// </summary>
	[Description("The file path to the questions file.")]
	[CommandArgument(0, "<questions-file-path>")]
	public required string QuestionsFilePath { get; set; }

	/// <summary>
	///Gets or sets the total number of questions per quiz session.
	/// </summary>
	[CommandOption("-q|--questions")]
	[Description("The total number of questions per quiz session.")]
	public int QuestionsPerQuiz { get; set; } = 50;

	/// <summary>
	/// Gets or sets the number of questions per quiz round.
	/// </summary>
	[CommandOption("-r|--rounds")]
	[Description("The number of questions per quiz round.")]
	[Range(5, 25)]
	public int QuestionsPerRound { get; set; } = 5;

	/// <summary>
	/// Gets or sets if the questions should be randomized.
	/// </summary>
	[CommandOption("-n|--randomize")]
	[Description("Indicates if the questions should be randomized.")]
	[Range(10, 200)]
	public bool RandomizeQuestions { get; set; }

	/// <summary>
	/// Gets or sets the threshold score to pass on the next round.
	/// </summary>
	[CommandOption("-t|--threshold")]
	[Description("The threshold score to pass on the next round.")]
	[Range(0.5f, 1.0f)]
	public float ThresholdScorePerRound { get; set; } = 0.5f;
}
