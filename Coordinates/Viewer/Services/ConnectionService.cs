using System.Diagnostics;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Viewer.Interfaces.Services;

namespace Viewer.Services;

/// <summary>
/// 	A service for starting the server.
/// </summary>
/// <seealso cref="IConnectionService"/>
public class ConnectionService(
	ILogger<ConnectionService> logger,
	IConfigurationService configurationService) : IConnectionService
{
#if DEBUG
	private const string ExePath = @"C:\Users\Josh.Worthington\Personal\Work\dotnet-coordinates-Insphere\Coordinates\CoordinateReader\bin\Debug\net8.0\CoordinateReader.exe";
#else
	private const string ExePath = @".\CoordinateReader.exe";
#endif

	/// <inheritdoc/>
	public GrpcChannel? Connection { get; private set; }

	/// <inheritdoc/>
	public bool IsServerRunning() => Process.GetProcessesByName("CoordinateReader").Length > 0;

	/// <inheritdoc/>
	public bool EstablishConnection()
	{
		try
		{
			var uri = new UriBuilder
			{
				Scheme = "https",
				Host = "localhost",
				Port = configurationService.Config.Port
			}.Uri;

			var channel = GrpcChannel.ForAddress(uri);
			var client = new Ping.PingClient(channel);
			client.SendPing(new PingRequest());

			Connection = channel;
			return true;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to connect to server at {Port}", configurationService.Config.Port);
			return false;
		}

	}

	/// <inheritdoc/>
	public bool StartServer(
		string port)
	{
		try
		{
			var startInfo = new ProcessStartInfo
			{
				FileName = ExePath,
				Arguments = port,
				WindowStyle = ProcessWindowStyle.Minimized
			};
			_ = Process.Start(startInfo) ?? throw new Exception("Failed to start server.");

			return EstablishConnection();
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to start server at {Port}", port);
			return false;
		}
	}
}