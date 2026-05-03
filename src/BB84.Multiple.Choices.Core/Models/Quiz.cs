/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
namespace BB84.Multiple.Choices.Core.Models;

/// <summary>
/// Represents a quiz consisting of multiple rounds.
/// </summary>
public sealed class Quiz
{
	/// <summary>
	/// Gets or sets the title of the quiz.
	/// </summary>
	public required string Title { get; init; }

	/// <summary>
	/// Gets or sets the list of rounds in the quiz.
	/// </summary>
	public required List<Round> Rounds { get; init; }
}
