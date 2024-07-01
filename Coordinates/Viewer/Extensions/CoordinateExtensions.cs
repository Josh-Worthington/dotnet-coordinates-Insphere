using System.Windows.Media.Media3D;
using CoordinateReader;

namespace Viewer.Extensions;

/// <summary>
/// 	The coordinate extensions.
/// </summary>
public static class CoordinateExtensions
{
	/// <summary>
	/// 	A Coordinate extension method that converts a coordinate to a point.
	/// </summary>
	/// <param name="coordinate">	The coordinate to act on. </param>
	/// <returns>
	/// 	Coordinate as a Point3D.
	/// </returns>
	public static Point3D ToPoint(
		this Coordinate coordinate) => new(coordinate.X, coordinate.Y, coordinate.Z);
}