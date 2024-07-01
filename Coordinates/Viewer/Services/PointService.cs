using System.Windows.Media.Media3D;
using CoordinateReader;
using Viewer.Extensions;
using Viewer.Interfaces.Services;

namespace Viewer.Services;

/// <summary>
/// 	A service for retrieving points and doing calculations.
/// </summary>
/// <seealso cref="IPointService"/>
public class PointService : IPointService
{
	/// <inheritdoc/>
	public Point3DCollection GetPoints(
		IEnumerable<Coordinate> coordinates)
	{
		ArgumentNullException.ThrowIfNull(coordinates);

		return new Point3DCollection(coordinates.Select(x => x.ToPoint()));
	}

	/// <inheritdoc/>
	public Point3D CalculateCentroid(
		Point3DCollection points)
	{
		double minY, minZ, maxY, maxZ;
		var minX = minY = minZ = double.MaxValue;
		var maxX = maxY = maxZ = double.MinValue;

		foreach (var point in points)
		{
			if (minX > point.X) minX = point.X;
			if (minY > point.Y) minY = point.Y;
			if (minZ > point.Z) minZ = point.Z;
			if (maxX < point.X) maxX = point.X;
			if (maxY < point.Y) maxY = point.Y;
			if (maxZ < point.Z) maxZ = point.Z;
		}

		return new Point3D(minX + (maxX - minX) / 2, minY + (maxY - minY) / 2, minZ + (maxZ - minZ) / 2);
	}
}