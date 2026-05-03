/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Events.Base;

namespace BB84.Multiple.Choices.Core.Events;

/// <summary>
/// Represents the event that a round has been completed.
/// </summary>
/// <param name="title">The title of the completed round.</param>
/// <param name="score">The score achieved in the completed round.</param>
public sealed class RoundCompletedEvent(string title, float score) : EventBase
{
	/// <summary>
	/// Gets the title of the completed round.
	/// </summary>
	public string Title => title;

	/// <summary>
	/// Gets the score achieved in the completed round.
	/// </summary>
	public float Score => score;
}
