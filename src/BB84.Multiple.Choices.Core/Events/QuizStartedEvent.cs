/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Events.Base;

namespace BB84.Multiple.Choices.Core.Events;

/// <summary>
/// Represents an event that is triggered when a new quiz is started.
/// </summary>
/// <param name="title">The title of the quiz.</param>
public sealed class QuizStartedEvent(string title) : EventBase
{
	/// <summary>
	/// Gets the title of the quiz.
	/// </summary>
	public string Title { get; } = title;
}
