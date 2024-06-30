namespace Shared.Interfaces;

/// <summary>
/// 	Interface for configuration service.
/// </summary>
public interface IConfigurationService
{
	/// <summary>
	/// 	Gets the shared configuration.
	/// </summary>
	Config Config { get; }
}