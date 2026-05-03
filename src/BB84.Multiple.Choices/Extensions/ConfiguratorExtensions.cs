/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using System.Diagnostics.CodeAnalysis;

using BB84.Multiple.Choices.Commands;

using Spectre.Console.Cli;

namespace BB84.Multiple.Choices.Extensions;

/// <summary>
/// Represents extension methods for the <see cref="IConfigurator"/> interface.
/// </summary>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here.")]
internal static class ConfiguratorExtensions
{
	/// <summary>
	/// Configures the commands.
	/// </summary>
	/// <param name="configurator">The configurator instance to use.</param>
	/// <returns>
	/// The same <see cref="IConfigurator"/> instance, so that multiple calls can be chained.
	/// </returns>
	internal static IConfigurator ConfigureCommands(this IConfigurator configurator)
	{
		configurator.AddCommand<QuizCommand>("quiz")
			.WithDescription("Start a multiple-choice quiz.")
			.WithExample(["quiz", "questions.json"]);

		return configurator;
	}
}
