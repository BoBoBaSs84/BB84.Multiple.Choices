/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using System.Net.Http.Json;

using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Events;
using BB84.Multiple.Choices.Core.Models;

namespace BB84.Multiple.Choices.Web.Services;

/// <summary>
/// Provides methods to load quiz questions via HTTP from wwwroot.
/// </summary>
/// <param name="httpClient">The HTTP client to use for fetching question files.</param>
/// <param name="eventService">The event service for publishing events.</param>
public sealed class WebQuizDataService(HttpClient httpClient, IEventService eventService) : IQuizDataService
{
	/// <inheritdoc/>
	public async Task<IReadOnlyList<Question>> LoadAllQuestionsAsync(string filePath, CancellationToken cancellationToken = default)
	{
		List<Question> questions = await httpClient.GetFromJsonAsync<List<Question>>(filePath, cancellationToken) ?? [];
		
		eventService.Publish(new QuestionsLoadedEvent(questions.Count));
		
		return questions;
	}
}
