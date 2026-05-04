/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Abstractions.Events;
using BB84.Multiple.Choices.Core.Abstractions.Services;

namespace BB84.Multiple.Choices.Core.Services;

/// <summary>
/// Represents a simple event service for publishing and subscribing to events.
/// </summary>
public sealed class EventService : IEventService
{
	private readonly Dictionary<Type, List<Action<object>>> _subscribers = [];

	/// <inheritdoc/>
	public void Subscribe<T>(Action<T> handler) where T : notnull, IEvent
	{
		if (!_subscribers.TryGetValue(typeof(T), out var handlers))
		{
			handlers = [];
			_subscribers[typeof(T)] = handlers;
		}
		handlers.Add(obj => handler((T)obj));
	}

	/// <inheritdoc/>
	public void Publish<T>(T message) where T : notnull, IEvent
	{
		if (_subscribers.TryGetValue(typeof(T), out List<Action<object>>? handlers))
			handlers.ForEach(handler => handler(message));
	}
}
