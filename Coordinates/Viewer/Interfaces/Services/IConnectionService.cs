using Grpc.Net.Client;

namespace Viewer.Interfaces.Services;

/// <summary>
/// 	Interface for connection service.
/// </summary>
public interface IConnectionService
{
	/// <summary>
	/// 	Gets the connection.
	/// </summary>
	public GrpcChannel? Connection { get; }

	/// <summary>
	/// 	Gets if the server is running asynchronously.
	/// </summary>
	/// <returns>
	/// 	True if the server is running, false if not.
	/// </returns>
	bool IsServerRunning();

	/// <summary>
	/// 	Connects to the server.
	/// </summary>
	/// <returns>
	///  	True if the connection was established, false if not.
	/// </returns>
	bool EstablishConnection();

	/// <summary>
	/// 	Starts the server at the given port.
	/// </summary>
	/// <param name="port">	The port. </param>
	/// <returns>
	/// 	True if the server started, false if not.
	/// </returns>
	bool StartServer(
		string port);
}