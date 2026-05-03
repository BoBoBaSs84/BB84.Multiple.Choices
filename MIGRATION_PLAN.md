# Migration Plan: Console App → Blazor WebAssembly (GitHub Pages)

This document outlines the step-by-step plan to migrate the **BB84.Multiple.Choices** console application to a **Blazor WebAssembly** application hosted on **GitHub Pages**.

---

## Table of Contents

1. [Overview](#1-overview)
2. [Phase 1 – Project Setup](#2-phase-1--project-setup)
3. [Phase 2 – Reusable Core (Shared Library)](#3-phase-2--reusable-core-shared-library)
4. [Phase 3 – Blazor WebAssembly Project](#4-phase-3--blazor-webassembly-project)
5. [Phase 4 – UI Pages & Components](#5-phase-4--ui-pages--components)
6. [Phase 5 – GitHub Pages Deployment](#6-phase-5--github-pages-deployment)
7. [Phase 6 – Cleanup & Validation](#7-phase-6--cleanup--validation)

---

## 1. Overview

### Current Architecture

| Layer            | Technology                                          |
| ---------------- | --------------------------------------------------- |
| UI / Interaction | `Spectre.Console`, `Spectre.Console.Cli` (terminal) |
| Command Handling | `QuizCommand : Command<QuizSettings>`               |
| Business Logic   | `QuizService`, `EventService`, `QuizDataService`    |
| Models           | `Quiz`, `Round`, `Question`                         |
| Data Source      | JSON files loaded from disk (`File.ReadAllText`)    |
| Hosting          | `Microsoft.Extensions.Hosting` (Generic Host)       |

### Target Architecture

| Layer            | Technology                                         |
| ---------------- | -------------------------------------------------- |
| UI / Interaction | Blazor components (Razor / HTML / CSS)             |
| Routing          | Blazor page routing (`@page`)                      |
| Business Logic   | `QuizService`, `EventService` (reused as-is)       |
| Models           | `Quiz`, `Round`, `Question` (reused as-is)         |
| Data Source      | JSON files loaded via `HttpClient` from `wwwroot/` |
| Hosting          | Blazor WebAssembly (static, client-side)           |
| Deployment       | GitHub Pages via GitHub Actions                    |

---

## 2. Phase 1 – Project Setup

### Step 1.1 – Create a shared class library project

Create a new class library to hold all reusable, UI-agnostic code:

```
src/BB84.Multiple.Choices.Core/BB84.Multiple.Choices.Core.csproj
```

- Target: `net8.0`
- SDK: `Microsoft.NET.Sdk`
- Package references: `BB84.Extensions` (if needed by services)

### Step 1.2 – Create a Blazor WebAssembly project

Create the new Blazor WASM project:

```
src/BB84.Multiple.Choices.Web/BB84.Multiple.Choices.Web.csproj
```

- SDK: `Microsoft.NET.Sdk.BlazorWebAssembly`
- Target: `net8.0`
- Package references:
  - `Microsoft.AspNetCore.Components.WebAssembly` (8.0.x)
  - `Microsoft.AspNetCore.Components.WebAssembly.DevServer` (8.0.x, dev only)
- Project reference: `BB84.Multiple.Choices.Core`

### Step 1.3 – Keep the existing console project (optional)

The original console project can remain alongside the new projects for backward compatibility. It would reference the shared core library instead of containing the logic directly.

---

## 3. Phase 2 – Reusable Core (Shared Library)

Move the following **UI-agnostic** files from `BB84.Multiple.Choices` into `BB84.Multiple.Choices.Core`:

### Step 2.1 – Models (move as-is)

- [x] `Models/Quiz.cs`
- [x] `Models/Round.cs`
- [x] `Models/Question.cs`

### Step 2.2 – Service Abstractions (move as-is)

- [x] `Abstractions/Services/IEventService.cs`
- [x] `Abstractions/Services/IQuizService.cs`
- [x] `Abstractions/Services/IQuizDataService.cs`
- [x] `Abstractions/Services/ILoggerService.cs`

### Step 2.3 – Service Implementations (move with modifications)

- [x] `Services/EventService.cs` – move as-is
- [x] `Services/QuizService.cs` – move as-is (no Spectre dependency)

### Step 2.4 – Events (move as-is)

- [x] `Events/QuestionsLoadedEvent.cs`
- [x] `Events/QuizStartedEvent.cs`
- [x] `Events/QuizCompletedEvent.cs`
- [x] `Events/RoundStartedEvent.cs`
- [x] `Events/RoundCompletedEvent.cs`
- [x] `Events/QuestionCorrectAnsweredEvent.cs`
- [x] `Events/QuestionIncorrectAnsweredEvent.cs`

### Step 2.5 – Files that need modification or new implementations

| File                          | Action                                                                                                                                                                                                                                                                       |
| ----------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Services/QuizDataService.cs` | **Rewrite** – Replace `File.ReadAllText` with an abstraction (e.g., accept a `string` JSON content or `Stream`), since file system access is not available in WASM. Alternatively, create a new `IQuizDataService` implementation in the Web project that uses `HttpClient`. |
| `Settings/QuizSettings.cs`    | **Rewrite** – Remove `Spectre.Console.Cli` base class (`CommandSettings`). Convert to a plain POCO class with the same properties.                                                                                                                                           |
| `Services/LoggerService.cs`   | **Move** – Should work as-is if it only wraps `ILogger<T>`.                                                                                                                                                                                                                  |

---

## 4. Phase 3 – Blazor WebAssembly Project

### Step 3.1 – Scaffold the Blazor WASM project structure

```
src/BB84.Multiple.Choices.Web/
├── wwwroot/
│   ├── index.html
│   ├── css/
│   │   └── app.css
│   ├── sampleQuestions.json      ← moved from console project root
│   └── ada.json                  ← moved from console project root
├── Layout/
│   └── MainLayout.razor
├── Pages/
│   ├── Home.razor                ← landing page / quiz configuration
│   ├── QuizPage.razor            ← question display & answer submission
│   └── Results.razor             ← quiz results / score summary
├── Components/
│   ├── QuestionCard.razor        ← single question display component
│   ├── AnswerOption.razor         ← individual answer option (checkbox/radio)
│   ├── ScoreBoard.razor          ← round/quiz score display
│   └── RoundSummary.razor        ← round completion summary
├── Services/
│   └── WebQuizDataService.cs     ← IQuizDataService using HttpClient
├── _Imports.razor
├── App.razor
├── Program.cs
└── BB84.Multiple.Choices.Web.csproj
```

### Step 3.2 – `Program.cs` (Blazor entry point)

```csharp
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register core services
builder.Services.AddSingleton<IEventService, EventService>();
builder.Services.AddScoped<IQuizDataService, WebQuizDataService>();
builder.Services.AddScoped<IQuizService, QuizService>();

await builder.Build().RunAsync();
```

### Step 3.3 – `WebQuizDataService.cs` (new implementation)

Replace file system access with `HttpClient` to fetch JSON from `wwwroot/`:

```csharp
public class WebQuizDataService : IQuizDataService
{
    private readonly HttpClient _httpClient;
    private readonly IEventService _eventService;

    public WebQuizDataService(HttpClient httpClient, IEventService eventService)
    {
        _httpClient = httpClient;
        _eventService = eventService;
    }

    public async Task<IList<Question>> LoadAllQuestionsAsync(string filePath)
    {
        var questions = await _httpClient.GetFromJsonAsync<List<Question>>(filePath) ?? [];
        _eventService.Publish(new QuestionsLoadedEvent(questions.Count));
        return questions;
    }
}
```

> **Note:** The `IQuizDataService` interface needs to be updated to support `async` methods (returning `Task<IList<Question>>`), since `HttpClient` calls are asynchronous.

### Step 3.4 – `wwwroot/index.html`

```html
<!DOCTYPE html>
<html lang="en">
	<head>
		<meta charset="utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1.0" />
		<title>BB84 Multiple Choices</title>
		<base href="/" />
		<link href="css/app.css" rel="stylesheet" />
		<link href="BB84.Multiple.Choices.Web.styles.css" rel="stylesheet" />
	</head>
	<body>
		<div id="app">Loading...</div>
		<script src="_framework/blazor.webassembly.js"></script>
	</body>
</html>
```

> **Important:** When deploying to GitHub Pages under a repository subpath (e.g., `https://bobobass84.github.io/csharp-testing-stuff/`), update `<base href="/" />` to `<base href="/csharp-testing-stuff/" />`.

---

## 5. Phase 4 – UI Pages & Components

### Step 4.1 – `Home.razor` (Landing / Configuration Page)

Responsibilities:

- Let the user select a question file (e.g., `sampleQuestions.json`, `ada.json`)
- Configure quiz settings (questions per quiz, per round, randomize, threshold)
- Start the quiz → navigate to `QuizPage`

### Step 4.2 – `QuizPage.razor` (Main Quiz Flow)

Responsibilities:

- Display the current round title and question number
- Render `QuestionCard` with `AnswerOption` components
- Accept user answer selection (checkboxes for multi-answer, radio for single)
- Submit answer via `IQuizService.SubmitAnswer()`
- Listen to events (`RoundCompleted`, `QuizCompleted`) to show summaries or navigate to results
- Replace the `while` loop in `QuizCommand` with Blazor's reactive rendering cycle

### Step 4.3 – `QuestionCard.razor`

```razor
<div class="question-card">
    <h3>Question @QuestionIndex</h3>
    <p>@Question.Text</p>
    @for (int i = 0; i < Question.Answers.Count; i++)
    {
        <AnswerOption Index="i" Text="@Question.Answers[i]" ... />
    }
    <button @onclick="SubmitAnswer">Submit</button>
</div>
```

### Step 4.4 – `Results.razor` (Score Summary)

Responsibilities:

- Display final score, per-round scores
- Option to retake the quiz

### Step 4.5 – Mapping Console UI → Blazor UI

| Console (Spectre.Console)                 | Blazor Equivalent                            |
| ----------------------------------------- | -------------------------------------------- |
| `AnsiConsole.MarkupLine(...)`             | Razor markup with CSS styling                |
| `AnsiConsole.WriteLine(...)`              | `<p>` / `<span>` elements                    |
| `Console.ReadLine()`                      | Input binding (`@bind`, `@onclick`)          |
| `while` loop (blocking)                   | Blazor component state + `StateHasChanged()` |
| CLI arguments (`--questions`, `--rounds`) | Form inputs / dropdowns on `Home.razor`      |
| `CommandSettings` (Spectre)               | Plain POCO `QuizSettings` bound to form      |

---

## 6. Phase 5 – GitHub Pages Deployment

### Step 5.1 – Add GitHub Actions workflow

Create `.github/workflows/deploy-gh-pages.yml`:

```yaml
name: Deploy Blazor WASM to GitHub Pages

on:
  push:
    branches: [main]
  workflow_dispatch:

permissions:
  contents: read
  pages: write
  id-token: write

concurrency:
  group: "pages"
  cancel-in-progress: false

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - name: Checkout
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: "8.0.x"

      - name: Publish Blazor WASM
        run: >
          dotnet publish
          BB84.Multiple.Choices/src/BB84.Multiple.Choices.Web/BB84.Multiple.Choices.Web.csproj
          -c Release
          -o release
          --nologo

      - name: Rewrite base href for GitHub Pages
        run: >
          sed -i 's|<base href="/" />|<base href="/csharp-testing-stuff/" />|g'
          release/wwwroot/index.html

      - name: Add .nojekyll file
        run: touch release/wwwroot/.nojekyll

      - name: Copy index.html to 404.html (SPA fallback)
        run: cp release/wwwroot/index.html release/wwwroot/404.html

      - name: Upload artifact
        uses: actions/upload-pages-artifact@v3
        with:
          path: release/wwwroot

      - name: Deploy to GitHub Pages
        uses: actions/deploy-pages@v4
```

### Step 5.2 – Enable GitHub Pages

1. Go to the repository **Settings** → **Pages**
2. Under **Source**, select **GitHub Actions**
3. The workflow will handle deployment automatically on push to `main`

### Step 5.3 – Handle SPA routing on GitHub Pages

GitHub Pages does not support SPA client-side routing natively. The workflow handles this by:

- Copying `index.html` to `404.html` so all routes fall back to the Blazor app
- Adding a `.nojekyll` file to prevent Jekyll processing

---

## 7. Phase 6 – Cleanup & Validation

### Step 6.1 – Update the solution file

Add the new projects to the solution:

```bash
dotnet sln add src/BB84.Multiple.Choices.Core/BB84.Multiple.Choices.Core.csproj
dotnet sln add src/BB84.Multiple.Choices.Web/BB84.Multiple.Choices.Web.csproj
```

### Step 6.2 – Update existing console project (optional)

If keeping the console app, update it to reference `BB84.Multiple.Choices.Core` instead of containing the logic directly. This avoids code duplication.

### Step 6.3 – Update tests

- Move/update unit tests to reference `BB84.Multiple.Choices.Core`
- Add integration tests for the Blazor components (optional, using `bunit`)

### Step 6.4 – Verify locally

```bash
cd src/BB84.Multiple.Choices.Web
dotnet run
```

Open `https://localhost:5001` and verify:

- [x] Questions load from `wwwroot/sampleQuestions.json`
- [x] Quiz flow works (start → answer → round summary → results)
- [x] Settings (questions per round, randomize, threshold) work
- [x] Events fire correctly (score tracking, round completion)

### Step 6.5 – Verify GitHub Pages deployment

Push to `main` and verify the app is accessible at:

```
https://bobobass84.github.io/csharp-testing-stuff/
```

---

## Summary of New/Modified Files

| File                                                           | Action                                                 |
| -------------------------------------------------------------- | ------------------------------------------------------ |
| `src/BB84.Multiple.Choices.Core/*.cs`                          | **New** – Shared library with models, services, events |
| `src/BB84.Multiple.Choices.Web/Program.cs`                     | **New** – Blazor WASM entry point                      |
| `src/BB84.Multiple.Choices.Web/Pages/*.razor`                  | **New** – Blazor pages                                 |
| `src/BB84.Multiple.Choices.Web/Components/*.razor`             | **New** – Reusable UI components                       |
| `src/BB84.Multiple.Choices.Web/Services/WebQuizDataService.cs` | **New** – HttpClient-based data service                |
| `src/BB84.Multiple.Choices.Web/wwwroot/`                       | **New** – Static assets + JSON question files          |
| `.github/workflows/deploy-gh-pages.yml`                        | **New** – CI/CD pipeline                               |
| `Settings/QuizSettings.cs`                                     | **Modified** – Remove Spectre.Console dependency       |
| `Abstractions/Services/IQuizDataService.cs`                    | **Modified** – Add async method signatures             |
