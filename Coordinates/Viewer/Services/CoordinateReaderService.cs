using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using CoordinateReader;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Viewer.Common;
using Viewer.Interfaces.Services;
using Coordinate = Viewer.Entities.Coordinate;

namespace Viewer.Services;

/// <summary>
/// 	A service for reading coordinates from the gRPC server.
/// </summary>
public class CoordinateReaderService(
	ILogger<CoordinateReaderService> logger,
	Reader.ReaderClient client) : ICoordinateReaderService
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
				var coordinate = call.ResponseStream.Current;
				coordinates.Add(new Coordinate
				{
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

		return new ReadOnlyCollection<Coordinate>(coordinates);
	}
}

