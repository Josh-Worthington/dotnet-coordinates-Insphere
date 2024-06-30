using Grpc.Core;

namespace CoordinateReader.Services;

/// <summary>
/// 	A service for sending pings.
/// </summary>
/// <seealso cref="Ping.PingBase"/>
public class PingService : Ping.PingBase
{
	/// <inheritdoc />
	public override Task<PingResponse> SendPing(
		PingRequest request,
		ServerCallContext context) => Task.FromResult(new PingResponse());
}