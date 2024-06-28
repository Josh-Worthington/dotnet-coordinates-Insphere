using System.Windows.Media.Media3D;
using CoordinateReader;

namespace Viewer.Interfaces.Services;

/// <summary>
/// 	Interface for the sphere drawing service.
/// </summary>
public interface ISphereDrawingService
{
	/// <summary>
	/// 	Draw spheres at coordinates onto the mesh.
	/// </summary>
	/// <param name="geometry">   	The geometry. </param>
	/// <param name="coordinates">	The coordinates. </param>
	void DrawSpheresAtCoordinates(
		MeshGeometry3D geometry,
		IReadOnlyCollection<Coordinate> coordinates);
}