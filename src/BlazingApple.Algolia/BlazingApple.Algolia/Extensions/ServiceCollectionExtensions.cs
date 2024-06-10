using BlazingApple.Algolia.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BlazingApple.Algolia.Extensions;

/// <summary>Extensions for <see cref="IServiceCollection"/></summary>
public static class ServiceCollectionExtensions
{
	/// <summary>Add algolia ui components.</summary>
	/// <returns>Fluent API.</returns>
	public static IServiceCollection AddAlgoliaUI(this IServiceCollection services)
	{
		services.AddScoped<IAlgoliaSearchService, AlgoliaSearchService>();
		return services;
	}
}