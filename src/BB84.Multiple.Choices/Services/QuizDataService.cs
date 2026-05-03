/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using System.Text.Json;

using BB84.Extensions.Serialization;
using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Events;
using BB84.Multiple.Choices.Core.Models;

namespace BB84.Multiple.Choices.Services;

/// <summary>
/// Provides methods to load and save quizzes from and to JSON files.
/// </summary>
internal sealed class QuizDataService : IQuizDataService
{
	private readonly IEventService _eventService;
	private readonly JsonSerializerOptions _jsonOptions;

	public QuizDataService(IEventService eventService)
	{
		_eventService = eventService;
		_jsonOptions = new() { PropertyNameCaseInsensitive = true };
	}

	public async Task<IReadOnlyList<Question>> LoadAllQuestionsAsync(string filePath, CancellationToken cancellationToken = default)
	{
		if (!File.Exists(filePath))
			throw new FileNotFoundException("Quiz file not found.", filePath);

		string jsonFileContent = await File.ReadAllTextAsync(filePath, cancellationToken)
			.ConfigureAwait(false);

		List<Question> questions = jsonFileContent.FromJson<List<Question>>(_jsonOptions) ?? [];

		_eventService.Publish(new QuestionsLoadedEvent(questions.Count));

		return questions;
	}
}
