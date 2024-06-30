namespace Viewer.Interfaces.Services;

/// <summary>
/// 	Interface for ping service.
/// </summary>
public interface IPingService
{
	/// <summary>
	/// 	Pings the server.
	/// </summary>
	Task<bool> Ping();
}