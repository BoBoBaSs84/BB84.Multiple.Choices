/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Events.Base;

namespace BB84.Multiple.Choices.Core.Events;

/// <summary>
/// Represents the event that a new round has started.
/// </summary>
/// <param name="title">The title of the new round.</param>
/// <param name="questionsCount">The amount of questions the round has.</param>
public sealed class RoundStartedEvent(string title, int questionsCount) : EventBase
{
	/// <summary>
	/// Gets the title of the new round.
	/// </summary>
	public string Title => title;

	/// <summary>
	/// Gets the amount of questions the round has.
	/// </summary>
	public int QuestionsCount => questionsCount;
}
