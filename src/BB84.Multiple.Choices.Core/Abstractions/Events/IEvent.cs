/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
namespace BB84.Multiple.Choices.Core.Abstractions.Events;

/// <summary>
/// Represents a domain event that can be published and handled within the application.
/// </summary>
public interface IEvent
{
	/// <summary>
	/// Gets the unique identifier of the event.
	/// </summary>
	Guid Id { get; }

	/// <summary>
	/// Gets the timestamp when the event was created.
	/// </summary>
	DateTimeOffset Timestamp { get; }
}
