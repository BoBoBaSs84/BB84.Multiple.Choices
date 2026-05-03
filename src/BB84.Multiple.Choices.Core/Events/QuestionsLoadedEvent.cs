/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Events.Base;

namespace BB84.Multiple.Choices.Core.Events;

/// <summary>
/// Represents an event that is triggered when questions are loaded.
/// </summary>
/// <param name="count">The number of questions loaded.</param>
public sealed class QuestionsLoadedEvent(int count) : EventBase
{
	/// <summary>
	/// Gets the number of questions loaded.
	/// </summary>
	public int Count => count;
}
