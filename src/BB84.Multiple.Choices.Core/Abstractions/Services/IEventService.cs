/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Abstractions.Events;

namespace BB84.Multiple.Choices.Core.Abstractions.Services;

/// <summary>
/// Represents a simple event service contract for publishing and subscribing to events.
/// </summary>
public interface IEventService
{
	/// <summary>
	/// Register a handler to be invoked when an event of <typeparamref name="T"/> type is published.
	/// </summary>
	/// <typeparam name="T">The type of event to subscribe to.</typeparam>
	/// <param name="handler">The handler to invoke when an event of type <typeparamref name="T"/> is published.</param>
	void Subscribe<T>(Action<T> handler) where T : notnull, IEvent;

	/// <summary>
	/// Publish an event of <typeparamref name="T"/> type with the specified message.
	/// </summary>
	/// <typeparam name="T">The type of event to publish.</typeparam>
	/// <param name="message">The event message to publish.</param>
	void Publish<T>(T message) where T : notnull, IEvent;
}
