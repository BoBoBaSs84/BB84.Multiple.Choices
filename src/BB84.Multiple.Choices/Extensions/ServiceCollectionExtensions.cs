/*
Copyright: 2026 Robert Peter Meyer
License: MIT

This source code is licensed under the MIT license found in the
LICENSE file in the root directory of this source tree.
*/
using System.Diagnostics.CodeAnalysis;

using BB84.Multiple.Choices.Abstractions.Services;
using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Services;
using BB84.Multiple.Choices.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BB84.Multiple.Choices.Extensions;

/// <summary>
/// The service collection extensions class.
/// </summary>
[SuppressMessage("Style", "IDE0058", Justification = "Not relevant here, dependency injection.")]
internal static class ServiceCollectionExtensions
{
	/// <summary>
	/// Registers the application services to the service collection.
	/// </summary>
	/// <param name="services">The service collection to enrich.</param>
	/// <param name="environment">The host environment instance to use.</param>
	/// <returns>
	/// The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.
	/// </returns>	
	internal static IServiceCollection RegisterServices(this IServiceCollection services, IHostEnvironment environment)
	{
		services.RegisterLoggerService(environment);

		services.AddSingleton<IEventService, EventService>();
		services.AddSingleton<IQuizDataService, QuizDataService>();
		services.AddSingleton<IQuizService, QuizService>();

		return services;
	}

	/// <summary>
	/// Registers the logger service.
	/// </summary>
	/// <param name="services">The service collection to enrich.</param>
	/// <param name="environment">The host environment instance to use.</param>
	/// <returns>
	/// The same <see cref="IServiceCollection"/> instance so that multiple calls can be chained.
	/// </returns>	
	[SuppressMessage("Interoperability", "CA1416", Justification = "Validate platform compatibility done.")]
	private static IServiceCollection RegisterLoggerService(this IServiceCollection services, IHostEnvironment environment)
	{
		services.TryAddSingleton(typeof(ILoggerService<>), typeof(LoggerService<>));

		services.AddLogging(builder =>
		{
			builder.ClearProviders();

			if (environment.IsDevelopment())
			{
				builder.SetMinimumLevel(LogLevel.Debug);
				builder.AddConsole();
			}

			if (environment.IsProduction())
			{
				builder.SetMinimumLevel(LogLevel.Warning);
				builder.AddEventLog(settings => settings.SourceName = environment.ApplicationName);
			}
		});

		return services;
	}
}
