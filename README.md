# BB84.Multiple.Choices

A multiple-choice quiz application built with .NET 8, available as both a **console application** and a **Blazor WebAssembly** web app.

[![CI](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/actions/workflows/ci.yml)
[![CD](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/actions/workflows/cd.yml/badge.svg?event=push)](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/actions/workflows/cd.yml)
[![CodeQL](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/actions/workflows/github-code-scanning/codeql/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/actions/workflows/github-code-scanning/codeql)
[![Dependabot](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/actions/workflows/dependabot/dependabot-updates/badge.svg?branch=main)](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/actions/workflows/dependabot/dependabot-updates)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![C#](https://img.shields.io/badge/C%23-13.0-239120)](https://github.com/BoBoBaSs84/BB84.Multiple.Choices)
[![.NET8.0](https://img.shields.io/badge/.NET-8.0-5C2D91)](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)
[![Issues](https://img.shields.io/github/issues/BoBoBaSs84/BB84.Multiple.Choices)](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/issues)
[![LastCommit](https://img.shields.io/github/last-commit/BoBoBaSs84/BB84.Multiple.Choices)](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/commit/main)
[![PullRequests](https://img.shields.io/github/issues-pr/BoBoBaSs84/BB84.Multiple.Choices)](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/pulls)
[![RepoSize](https://img.shields.io/github/repo-size/BoBoBaSs84/BB84.Multiple.Choices)](https://github.com/BoBoBaSs84/BB84.Multiple.Choices)

## Projects

| Project                      | Description                                                                              |
| ---------------------------- | ---------------------------------------------------------------------------------------- |
| `BB84.Multiple.Choices.Core` | Shared class library containing models, services, events, and abstractions               |
| `BB84.Multiple.Choices`      | Console application using [Spectre.Console](https://spectreconsole.net/) for terminal UI |
| `BB84.Multiple.Choices.Web`  | Blazor WebAssembly application for browser-based quizzes                                 |
| `BB84.Multiple.ChoicesTests` | Unit tests using MSTest and Moq                                                          |

## Architecture

```
BB84.Multiple.Choices.Core (shared library)
├── Models        → Quiz, Round, Question
├── Services      → QuizService, EventService
├── Events        → Quiz/Round/Question lifecycle events
├── Settings      → QuizSettings
└── Abstractions  → Service and event interfaces

BB84.Multiple.Choices (console app)
├── Commands      → QuizCommand (Spectre.Console.Cli)
├── Services      → QuizDataService (file-based), LoggerService
└── Program.cs    → Host builder entry point

BB84.Multiple.Choices.Web (Blazor WASM)
├── Pages         → Home, QuizPage
├── Components    → QuestionCard, ScoreBoard, RoundSummary
├── Services      → WebQuizDataService (HttpClient-based)
└── Program.cs    → WebAssembly entry point
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Run the Console App

```pwsh
cd src/BB84.Multiple.Choices
dotnet run -- --file sampleQuestions.json
```

### Run the Blazor WebAssembly App

```pwsh
cd src/BB84.Multiple.Choices.Web
dotnet run
```

Then open the URL shown in the terminal (typically `https://localhost:5001`).

### Run Tests

```pwsh
dotnet test
```

## Deployment

The Blazor WebAssembly app is automatically deployed to **GitHub Pages** via GitHub Actions on every push to `main`.

**Live URL:** [https://bobobass84.github.io/BB84.Multiple.Choices/](https://bobobass84.github.io/BB84.Multiple.Choices/)

The deployment workflow (`.github/workflows/cd.yml`) publishes the WASM app as static files and handles SPA routing fallback via a `404.html` redirect.

## Question Files

Quiz questions are defined as JSON arrays. Example:

```json
[
	{
		"Text": "What is the capital of France?",
		"Answers": ["Berlin", "London", "Paris", "Madrid"],
		"CorrectAnswerIndices": [2]
	}
]
```

- **Console app:** Place JSON files in the project root; they are copied to the output directory.
- **Web app:** Place JSON files in `wwwroot/sample-data/`.

## Contributing

Contributions are welcome! If you have an idea for a new feature, improvement, or bug fix, please follow these steps:

1. Have a look at the [Issues](https://github.com/BoBoBaSs84/BB84.Multiple.Choices/issues) to see if your idea has already been discussed.
2. If you want to work on an existing issue, please comment on the issue to let others know you're working on it.
3. Fork the repository and create a new branch for your contribution.
4. Make your changes and commit them with clear and descriptive messages.
5. Push your changes to your forked repository and submit a pull request to the main repository.

## Code of Conduct

We expect all contributors to adhere to the [Code of Conduct](CODE_OF_CONDUCT.md).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

**Robert Peter Meyer (BoBoBaSs84)**

- GitHub: [@BoBoBaSs84](https://github.com/BoBoBaSs84)
- Repository: [BB84.Multiple.Choices](https://github.com/BoBoBaSs84/BB84.Multiple.Choices)
