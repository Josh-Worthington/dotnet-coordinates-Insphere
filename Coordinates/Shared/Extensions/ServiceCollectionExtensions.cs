using Microsoft.Extensions.DependencyInjection;
using Shared.Interfaces;

namespace Shared.Extensions;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// 	An IServiceCollection extension method that adds the shared services to the dependency injection service collection.
	/// </summary>
	/// <param name="services">	The services to act on. </param>
	public static void AddSharedServices(
		this IServiceCollection services) => services.AddSingleton<IConfigurationService, ConfigurationServiceSingleton>();
}