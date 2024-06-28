using System.Collections.ObjectModel;
using CoordinateReader;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Viewer.Common;
using Viewer.Interfaces.Services;

namespace Viewer.Services;

/// <summary>
/// 	A service for reading coordinates from the gRPC server.
/// </summary>
public class CoordinateReaderService(
	Reader.ReaderClient client,
	ILogger<CoordinateReaderService> logger) : ICoordinateReaderService
{
	/// <inheritdoc/>
	public async Task<Either<RpcException, IReadOnlyCollection<Coordinate>>> GetCoordinatesAsync(
		string filePath)
	{
		using var call = client.ReadCoordinates(new ReadPath { FilePath = filePath, Id = "" });

		var coordinates = new List<Coordinate>();
		try
		{
			while (await call.ResponseStream.MoveNext(CancellationToken.None))
			{
				coordinates.Add(call.ResponseStream.Current);
			}
		}
		catch (RpcException ex)
		{
			logger.LogError(ex, "Failed to retrieve the coordinates from the server");
			return ex;
		}

		return new ReadOnlyCollection<Coordinate>(coordinates);
	}
}

