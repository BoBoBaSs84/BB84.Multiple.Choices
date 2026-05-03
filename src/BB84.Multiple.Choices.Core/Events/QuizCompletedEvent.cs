/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Events.Base;

namespace BB84.Multiple.Choices.Core.Events;

/// <summary>
/// Represents the event when a quiz has been completed.
/// </summary>
/// <param name="title">The title of the completed quiz.</param>
/// <param name="score">The score achieved in the completed quiz.</param>
public sealed class QuizCompletedEvent(string title, float score) : EventBase
{
	/// <summary>
	/// Gets the title of the completed quiz.
	/// </summary>
	public string Title => title;

	/// <summary>
	/// Gets the score achieved in the completed quiz.
	/// </summary>
	public float Score => score;
}
