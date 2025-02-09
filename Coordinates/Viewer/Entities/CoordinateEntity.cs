﻿using System.Windows.Media.Media3D;

namespace Viewer.Entities;

/// <summary>
/// 	A coordinate, with a data structure to accommodate the 3D Viewport.
/// </summary>
public record CoordinateEntity
{
	/// <summary>
	/// 	Gets the zero-based index of this object.
	/// </summary>
	public uint Index { get; init; }

	/// <summary>
	/// 	Gets or initialises the position.
	/// </summary>
	public Point3D Position { get; init; }

	/// <summary>
	/// 	Gets or initialises the rotation.
	/// </summary>
	public Vector3D Rotation { get; init; }
}