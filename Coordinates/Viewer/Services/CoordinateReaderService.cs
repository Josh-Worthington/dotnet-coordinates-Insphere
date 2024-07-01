using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Shared.Entities;
using Viewer.Entities;
using Viewer.Interfaces.Services;

namespace Viewer.Services;

/// <summary>
/// 	A service for reading coordinates from the gRPC server.
/// </summary>
public class CoordinateReaderService(
	ILogger<CoordinateReaderService> logger,
	Reader.ReaderClient client) : ICoordinateReaderService
{
	/// <inheritdoc/>
	public async Task<Either<RpcException, IReadOnlyCollection<CoordinateEntity>>> GetCoordinatesAsync(
		string filePath,
		string pathId)
	{
		using var call = client.ReadCoordinates(new ReadPath { FilePath = filePath, Id = pathId });

		var coordinates = new List<CoordinateEntity>();
		try
		{
			while (await call.ResponseStream.MoveNext(CancellationToken.None))
			{
				var coordinate = call.ResponseStream.Current;
				coordinates.Add(new CoordinateEntity
				{
					Index = coordinate.Index,
					Position = new Point3D(coordinate.X, coordinate.Y, coordinate.Z),
					Rotation = new Vector3D(coordinate.Rx, coordinate.Ry, coordinate.Rz)
				});
			}
		}
		catch (RpcException ex)
		{
			logger.LogError(ex, "Failed to retrieve the coordinates from the server");
			return ex;
		}

		return new ReadOnlyCollection<CoordinateEntity>(coordinates);
	}
}

