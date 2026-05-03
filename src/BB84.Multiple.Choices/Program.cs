/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using System.Reflection;

using BB84.Multiple.Choices.Common;
using BB84.Multiple.Choices.Extensions;

using Microsoft.Extensions.Hosting;

using Spectre.Console;
using Spectre.Console.Cli;

internal class Program
{
	private static readonly Assembly Assembly = typeof(Program).Assembly;

	private static async Task Main(string[] args)
	{
		AnsiConsole.WriteLine($"{Assembly.GetName().Name} - {Assembly.GetName().Version}");
		AnsiConsole.WriteLine();

		CommandApp app = CreateCommandApp(args);

		await app.RunAsync(args)
			.ConfigureAwait(false);
	}

	private static CommandApp CreateCommandApp(string[] args)
	{
		// Create host builder
		IHostBuilder builder = Host.CreateDefaultBuilder(args)
			.ConfigureServices((context, services) => services.RegisterServices(context.HostingEnvironment));
		// Create type registrar
		TypeRegistrar typeRegistrar = new(builder);
		// Create command app
		CommandApp app = new(typeRegistrar);
		// Register available commands
		app.Configure(config => config.ConfigureCommands());

		return app;
	}
}
