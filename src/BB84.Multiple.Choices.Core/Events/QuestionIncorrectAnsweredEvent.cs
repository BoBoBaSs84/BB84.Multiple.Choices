/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Events.Base;

namespace BB84.Multiple.Choices.Core.Events;

/// <summary>
/// Represents an event indicating that an invalid answer has been provided.
/// </summary>
/// <param name="correctAnswers">The correct answers that should have been provided.</param>
public sealed class QuestionIncorrectAnsweredEvent(IReadOnlyList<string> correctAnswers) : EventBase
{
	/// <summary>
	/// Gets the answers that should have been provided.
	/// </summary>
	public IReadOnlyList<string> CorrectAnswers { get; } = correctAnswers;
}
