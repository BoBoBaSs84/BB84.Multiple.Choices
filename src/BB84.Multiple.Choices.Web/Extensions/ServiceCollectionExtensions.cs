using BB84.Multiple.Choices.Core.Abstractions.Services;
using BB84.Multiple.Choices.Core.Services;
using BB84.Multiple.Choices.Web.Services;

namespace BB84.Multiple.Choices.Web.Extensions;

/// <summary>
/// Represents extension methods for the <see cref="IServiceCollection"/> interface
/// to register services for the application.
/// </summary>
internal static class ServiceCollectionExtensions
{
	/// <summary>
	/// Registers the services for the application.
	/// </summary>
	/// <param name="services">The service collection to register services with.</param>
	/// <returns>The updated service collection.</returns>
	public static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddSingleton<IEventService, EventService>()
			.AddScoped<IQuizDataService, WebQuizDataService>()
			.AddScoped<IQuizService, QuizService>();

		return services;
	}
}
