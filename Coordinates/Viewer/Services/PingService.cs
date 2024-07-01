using Grpc.Core;
using Viewer.Interfaces.Services;

namespace Viewer.Services;

/// <summary>
/// 	A service for pinging the server.
/// </summary>
/// <seealso cref="IPingService"/>
public class PingService(
	Ping.PingClient pingClient) : IPingService
{
	/// <inheritdoc/>
	public async Task<bool> Ping()
	{
		try
		{
			await pingClient.SendPingAsync(new PingRequest());
			return true;
		}
		catch (RpcException)
		{
			return false;
		}
	}
}
