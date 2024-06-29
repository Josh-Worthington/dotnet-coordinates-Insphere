using System.Windows.Media.Media3D;
using Viewer.Common;

namespace Viewer.Interfaces.Services;

/// <summary>
/// 	Interface for the sphere drawing service.
/// </summary>
public interface ISphereDrawingService
{
	/// <summary>
	/// 	Draw spheres at coordinates onto the mesh.
	/// </summary>
	/// <param name="collection">	The collection. </param>
	/// <returns>
	/// 	Either the centre point of the spheres, or an Exception;
	/// </returns>
	Either<Exception, Point3D> DrawSpheres(
		Point3DCollection collection);
}