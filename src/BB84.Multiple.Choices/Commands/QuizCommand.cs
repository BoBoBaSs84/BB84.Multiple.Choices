/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using BB84.Extensions;
using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Events;
using BB84.Multiple.Choices.Core.Models;
using BB84.Multiple.Choices.Settings;

using Spectre.Console;
using Spectre.Console.Cli;

namespace BB84.Multiple.Choices.Commands;

/// <summary>
/// Represents the command to manage quizzes.
/// </summary>
public sealed class QuizCommand : Command<QuizSettings>
{
	private readonly IQuizDataService _quizDataService;
	private readonly IQuizService _quizService;
	private readonly IEventService _eventService;
	private bool _isQuizCompleted;

	/// <summary>
	/// Initializes a new instance of the <see cref="QuizCommand"/> class.
	/// </summary>
	/// <param name="eventService">The event service for publishing and subscribing to events.</param>
	/// <param name="quizDataService">The quiz data service to use.</param>
	/// <param name="quizService">The quiz service to use.</param>
	public QuizCommand(IEventService eventService, IQuizDataService quizDataService, IQuizService quizService)
	{
		_eventService = eventService;
		_quizDataService = quizDataService;
		_quizService = quizService;

		RegisterEvents();
	}

	/// <inheritdoc/>
	protected override int Execute(CommandContext context, QuizSettings settings, CancellationToken cancellationToken)
	{
		try
		{
			List<Question> allQuestions = [.. _quizDataService
				.LoadAllQuestionsAsync(settings.QuestionsFilePath, cancellationToken)
				.GetAwaiter()
				.GetResult()];

			_quizService.StartQuiz("SampleQuiz", allQuestions, settings);

			// while loop to allow retaking the quiz
			while (_isQuizCompleted.IsFalse())
			{
				Round round = _quizService.GetCurrentRound();
				Question question = _quizService.GetCurrentQuestion();

				if (question is not null)
				{
					int questionIndex = round.Questions.IndexOf(question) + 1;

					AnsiConsole.WriteLine();
					AnsiConsole.MarkupLine($"[blue]Question {questionIndex}[/] - {question.Text}");

					foreach (string answer in question.Answers)
					{
						int i = question.Answers.IndexOf(answer);
						AnsiConsole.WriteLine();
						AnsiConsole.WriteLine($"{i + 1}.) {answer}");
					}

					AnsiConsole.WriteLine();
					AnsiConsole.Write("Your answer (enter the number(s), comma separated): ");

					string? input = Console.ReadLine();
					List<int> answerIndices = [.. (input ?? string.Empty)
						.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
						.Select(s => int.TryParse(s, out int n) ? n - 1 : -1)];

					_quizService.SubmitAnswer(answerIndices);
				}
			}

			return 0;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred while executing the quiz command: {ex.Message}");
			return -1;
		}
	}

	private void RegisterEvents()
	{
		_eventService.Subscribe<QuestionsLoadedEvent>(OnQuestionsLoaded);
		_eventService.Subscribe<QuizStartedEvent>(OnQuizStarted);
		_eventService.Subscribe<QuizCompletedEvent>(OnQuizCompleted);
		_eventService.Subscribe<RoundStartedEvent>(OnRoundStarted);
		_eventService.Subscribe<RoundCompletedEvent>(OnRoundCompleted);
		_eventService.Subscribe<QuestionCorrectAnsweredEvent>(OnQuestionCorrectAnswered);
		_eventService.Subscribe<QuestionIncorrectAnsweredEvent>(OnQuestionIncorrectAnswered);
	}

	private void OnQuestionsLoaded(QuestionsLoadedEvent @event)
	{
		AnsiConsole.MarkupLine($"[green]{@event.Count} questions loaded successfully.[/]");
	}

	private void OnRoundCompleted(RoundCompletedEvent @event)
	{
		AnsiConsole.MarkupLine($"[green]Round '{@event.Title}' completed![/]");
		AnsiConsole.MarkupLine($"[yellow]Score: {@event.Score}%[/]");
	}

	private void OnRoundStarted(RoundStartedEvent @event)
	{
		AnsiConsole.Write(new FigletText(@event.Title).Color(Color.Blue));
		AnsiConsole.MarkupLine($"[blue]Starting round '{@event.Title}' with {@event.QuestionsCount} questions ...[/]");
	}

	private void OnQuizCompleted(QuizCompletedEvent @event)
	{
		AnsiConsole.WriteLine();
		AnsiConsole.MarkupLine($"[bold green]Quiz '{@event.Title}' completed![/]");
		AnsiConsole.MarkupLine($"[bold yellow]Final Score: {@event.Score}%[/]");
		_isQuizCompleted = true;
	}

	private void OnQuizStarted(QuizStartedEvent @event)
	{
		AnsiConsole.WriteLine();
		AnsiConsole.MarkupLine($"[bold blue]Quiz '{@event.Title}' started![/]");
	}

	private void OnQuestionIncorrectAnswered(QuestionIncorrectAnsweredEvent @event)
	{
		AnsiConsole.WriteLine();
		AnsiConsole.MarkupLine("[red]Incorrect answer provided for the question.[/]");
		AnsiConsole.WriteLine();
		AnsiConsole.MarkupLine($"[yellow]Correct Answer(s): {string.Join(", ", @event.CorrectAnswers)}[/]");
	}

	private void OnQuestionCorrectAnswered(QuestionCorrectAnsweredEvent @event)
	{
		AnsiConsole.WriteLine();
		AnsiConsole.MarkupLine("[green]Correct answer! Well done.[/]");
	}
}
