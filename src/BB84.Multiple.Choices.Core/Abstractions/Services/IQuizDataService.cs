/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Models;

namespace BB84.Multiple.Choices.Core.Abstractions.Services;

/// <summary>
/// Represents a service contract for managing quiz data.
/// </summary>
public interface IQuizDataService
{
	/// <summary>
	/// Loads all questions from the specified file path asynchronously.
	/// </summary>
	/// <param name="filePath">The path to the JSON file.</param>
	/// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of <see cref="Question"/> objects.</returns>
	public Task<IReadOnlyList<Question>> LoadAllQuestionsAsync(string filePath, CancellationToken cancellationToken = default);
}
