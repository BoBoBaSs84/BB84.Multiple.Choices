/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Models;
using BB84.Multiple.Choices.Services;

using Moq;

namespace BB84.Multiple.ChoicesTests.Services;

[TestClass]
public sealed class QuizDataServiceTests
{
	private readonly Mock<IEventService> _eventServiceMock;
	private readonly IQuizDataService _quizDataService;

	public QuizDataServiceTests()
	{
		_eventServiceMock = new Mock<IEventService>();
		_quizDataService = new QuizDataService(_eventServiceMock.Object);
	}

	[TestMethod]
	public void ImplementsIQuizDataService()
		=> Assert.IsInstanceOfType<IQuizDataService>(_quizDataService);

	[TestMethod]
	public Task LoadAllQuestionsWithNonExistentFileThrowsFileNotFoundException()
	{
		CancellationToken token = CancellationToken.None;
		return Assert.ThrowsAsync<FileNotFoundException>(async () => await _quizDataService.LoadAllQuestionsAsync("non_existent_file.json", token));
	}

	[TestMethod]
	public void LoadAllQuestionsWithValidFileReturnsQuestions()
	{
		string tempFilePath = Path.GetTempFileName();
		string jsonContent = @"
		[
			{
				""Text"": ""What is the capital of France?"",
				""Answers"": [""Berlin"", ""Madrid"", ""Paris"", ""Rome""],
				""CorrectAnswerIndices"": [2]
			},
			{
				""Text"": ""What is 2 + 2?"",
				""Answers"": [""3"", ""4"", ""5"", ""6""],
				""CorrectAnswerIndices"": [1]
			}
		]";
		File.WriteAllText(tempFilePath, jsonContent);
		try
		{
			CancellationToken token = CancellationToken.None;
			List<Question> questions = [.. _quizDataService
				.LoadAllQuestionsAsync(tempFilePath, token)
				.GetAwaiter()
				.GetResult()];

			Assert.HasCount(2, questions);
			Assert.AreEqual("What is the capital of France?", questions[0].Text);
			Assert.AreEqual("What is 2 + 2?", questions[1].Text);
		}
		finally
		{
			// Clean up
			if (File.Exists(tempFilePath))
				File.Delete(tempFilePath);
		}
	}
}
