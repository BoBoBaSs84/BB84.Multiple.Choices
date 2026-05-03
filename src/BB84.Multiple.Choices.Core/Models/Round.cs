/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
namespace BB84.Multiple.Choices.Core.Models;

/// <summary>
/// Represents a round in a quiz, containing a list of questions.
/// </summary>
public sealed class Round
{
	/// <summary>
	/// Gets or sets the title of the round.
	/// </summary>
	public required string Title { get; init; }

	/// <summary>
	/// Gets or sets the list of questions in the round.
	/// </summary>
	public required List<Question> Questions { get; init; }
}
