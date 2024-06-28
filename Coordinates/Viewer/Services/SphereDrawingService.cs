using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using CoordinateReader;
using Viewer.Interfaces.Services;

namespace Viewer.Services;

/// <summary>
/// 	A service for drawing spheres onto <see cref="MeshGeometry3D"/>.
/// </summary>
/// <seealso cref="ISphereDrawingService"/>
public class SphereDrawingService : ISphereDrawingService
{
	// A dictionary to hold points for fast lookup.
	private readonly Dictionary<Point3D, int> _pointDictionary = new();

	private double _xMax;
	private double _xMin;
	private double _zMin;
	private double _zMax;
	private double _textureXScale;
	private double _textureZScale;

	/// <inheritdoc/>
	public void DrawSpheresAtCoordinates(
		MeshGeometry3D geometry,
		IReadOnlyCollection<Coordinate> coordinates)
	{
		SetScale(coordinates);

		foreach (var coord in coordinates)
		{
			AddSphere(geometry, new Point3D(coord.X, coord.Y, coord.Y), 5, 8, 8);
		}
	}

	private void SetScale(
		IReadOnlyCollection<Coordinate> coordinates)
	{
		_xMax = _zMax = -double.MaxValue;
		_xMin = _zMin = double.MaxValue;

		foreach (var coordinate in coordinates)
		{
			if (coordinate.X < _xMin) _xMin = coordinate.X;
			if (coordinate.X > _xMax) _xMax = coordinate.X;

			if (coordinate.Z < _zMin) _zMin = coordinate.Z;
			if (coordinate.Z > _zMax) _zMax = coordinate.Z;
		}

		_textureXScale = _xMax - _xMin;
		_textureZScale = _zMax - _zMin;
	}

	/// <summary>
	/// 	Add a sphere.
	/// </summary>
	/// <param name="mesh">	   	The mesh. </param>
	/// <param name="center">  	The center. </param>
	/// <param name="radius">  	The radius. </param>
	/// <param name="numPhi">  	Number of phis. </param>
	/// <param name="numTheta">	Number of thetas. </param>
	/// <remarks>
	///		Credit goes to Rod Stephens.
	///		Reference: https://csharphelper.com/howtos/howto_3D_sphere.html
	/// </remarks>
	private void AddSphere(
		MeshGeometry3D mesh,
		Point3D center,
		double radius,
		int numPhi,
		int numTheta)
	{
		double phi0, theta0;
		var dphi = Math.PI / numPhi;
		var dtheta = 2 * Math.PI / numTheta;

		phi0 = 0;
		var y0 = radius * Math.Cos(phi0);
		var r0 = radius * Math.Sin(phi0);
		for (var i = 0; i < numPhi; i++)
		{
			var phi1 = phi0 + dphi;
			var y1 = radius * Math.Cos(phi1);
			var r1 = radius * Math.Sin(phi1);

			// Point ptAB has phi value A and theta value B.
			// For example, pt01 has phi = phi0 and theta = theta1.
			// Find the points with theta = theta0.
			theta0 = 0;
			var pt00 = new Point3D(
				center.X + r0 * Math.Cos(theta0),
				center.Y + y0,
				center.Z + r0 * Math.Sin(theta0));
			var pt10 = new Point3D(
				center.X + r1 * Math.Cos(theta0),
				center.Y + y1,
				center.Z + r1 * Math.Sin(theta0));

			for (var j = 0; j < numTheta; j++)
			{
				// Find the points with theta = theta1.
				var theta1 = theta0 + dtheta;
				var pt01 = new Point3D(
					center.X + r0 * Math.Cos(theta1),
					center.Y + y0,
					center.Z + r0 * Math.Sin(theta1));
				var pt11 = new Point3D(
					center.X + r1 * Math.Cos(theta1),
					center.Y + y1,
					center.Z + r1 * Math.Sin(theta1));

				// Create the triangles.
				AddTriangle(mesh, pt00, pt11, pt10);
				AddTriangle(mesh, pt00, pt01, pt11);

				// Move to the next value of theta.
				theta0 = theta1;
				pt00 = pt01;
				pt10 = pt11;
			}

			// Move to the next value of phi.
			phi0 = phi1;
			y0 = y1;
			r0 = r1;
		}
	}

	/// <summary>
	/// 	Add a triangle to the indicated mesh.
	/// </summary>
	/// <param name="mesh">  	The mesh. </param>
	/// <param name="point1">	The first point. </param>
	/// <param name="point2">	The second point. </param>
	/// <param name="point3">	The third point. </param>
	/// <remarks>
	///		Credit goes to Rod Stephens.
	///		Reference: https://csharphelper.com/howtos/howto_3D_draw_surface.html
	/// </remarks>
	private void AddTriangle(
		MeshGeometry3D mesh,
		Point3D point1,
		Point3D point2,
		Point3D point3)
	{
		// Get the points' indices.
		var index1 = AddPoint(mesh.Positions, mesh.TextureCoordinates, point1);
		var index2 = AddPoint(mesh.Positions, mesh.TextureCoordinates, point2);
		var index3 = AddPoint(mesh.Positions, mesh.TextureCoordinates, point3);

		// Create the triangle.
		mesh.TriangleIndices.Add(index1);
		mesh.TriangleIndices.Add(index2);
		mesh.TriangleIndices.Add(index3);
	}

	/// <summary>
	/// 	If the point already exists, return its index. Otherwise create the point and return its new index.
	/// </summary>
	/// <param name="points">		 	The points. </param>
	/// <param name="textureCoords">	The texture coordinates. </param>
	/// <param name="point">		 	The point. </param>
	/// <remarks>
	///		Credit goes to Rod Stephens.
	/// 	Reference: https://csharphelper.com/howtos/howto_draw_gridded_surface.html
	/// </remarks>
	private int AddPoint(
		Point3DCollection points,
		PointCollection textureCoords,
		Point3D point)
	{
		// If the point is in the point dictionary,
		// return its saved index.
		if (_pointDictionary.TryGetValue(point, out var index))
		{
			return index;
		}

		// We didn't find the point. Create it.
		points.Add(point);
		_pointDictionary.Add(point, points.Count - 1);

		//// Set the point's texture coordinates.
		textureCoords.Add(new Point(
			(point.X - _xMin) * _textureXScale,
			(point.Z - _zMin) * _textureZScale));

		// Return the new point's index.
		return points.Count - 1;
	}
}