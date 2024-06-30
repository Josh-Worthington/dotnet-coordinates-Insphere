using Microsoft.Extensions.Configuration;
using Shared.Interfaces;

namespace Shared;

/// <summary>
/// 	A configuration service singleton.
/// </summary>
/// <seealso cref="IConfigurationService"/>
public class ConfigurationServiceSingleton : IConfigurationService
{
	private const int DefaultPort = 7001;

	/// <inheritdoc/>
	public Config Config { get; } = CreateConfigObject();

	private static Config CreateConfigObject()
	{
		var settingsPath = Path.Combine(Config.RootDirectory, "settings.json");
		if (!File.Exists(settingsPath))
		{
			var config = new Config
			{
				Port = DefaultPort
			};
		}

		return new ConfigurationBuilder()
			.SetBasePath(Config.RootDirectory)
			.AddJsonFile("settings.json", false, true)
			.Build()
			.Get<Config>() ?? throw new Exception("Failed to create config.");
	}
}

