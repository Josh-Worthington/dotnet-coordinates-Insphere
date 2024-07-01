using System.Windows.Media.Media3D;
using CoordinateReader;

namespace Viewer.Interfaces.Services;

/// <summary>
/// 	Interface for point service.
/// </summary>
public interface IPointService
{
	/// <summary>
	/// 	Gets the points from the coordinates.
	/// </summary>
	/// <param name="coordinates">	The coordinates. </param>
	/// <returns>
	/// 	The collection of points.
	/// </returns>
	Point3DCollection GetPoints(
		IEnumerable<Coordinate> coordinates);

	/// <summary>
	/// 	Calculates the centroid.
	/// </summary>
	/// <param name="points">	The points. </param>
	/// <returns>
	/// 	The calculated centroid.
	/// </returns>
	Point3D CalculateCentroid(
		Point3DCollection points);
}