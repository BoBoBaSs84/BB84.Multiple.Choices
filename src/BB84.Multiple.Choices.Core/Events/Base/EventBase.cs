/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Abstractions.Events;

namespace BB84.Multiple.Choices.Core.Events.Base;

/// <summary>
/// Represents a base class for domain events, providing common properties
/// such as Id and Timestamp.
/// </summary>
public abstract class EventBase : IEvent
{
	/// <inheritdoc/>
	public Guid Id { get; } = Guid.NewGuid();

	/// <inheritdoc/>
	public DateTimeOffset Timestamp { get; } = DateTimeOffset.UtcNow;
}
