using System.Windows.Media.Media3D;

namespace Viewer.Interfaces.Services;

/// <summary>
/// 	Interface for point service.
/// </summary>
public interface IPointMathService
{
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