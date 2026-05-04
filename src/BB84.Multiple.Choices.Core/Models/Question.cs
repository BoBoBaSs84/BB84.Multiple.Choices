/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
namespace BB84.Multiple.Choices.Core.Models;

/// <summary>
/// Represents a multiple-choice question.
/// </summary>
public sealed class Question
{
	/// <summary>
	/// Gets or sets the question text.
	/// </summary>
	public required string Text { get; init; }

	/// <summary>
	/// Gets or sets the list of possible answers.
	/// </summary>
	public required List<string> Answers { get; init; }

	/// <summary>
	/// Gets or sets the indices of the correct answers in the <see cref="Answers"/> list.
	/// </summary>
	public required List<int> CorrectAnswerIndices { get; init; }
}
