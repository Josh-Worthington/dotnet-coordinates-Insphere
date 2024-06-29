using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Viewer.Interfaces.ViewModels;

/// <summary>
/// 	Interface for 3D model view model.
/// </summary>
public interface IDisplay3DViewModel
{
	/// <summary>
	/// 	Gets the viewport.
	/// </summary>
	Point3DCollection Points { get; }

	/// <summary>
	/// 	Gets or sets the camera position.
	/// </summary>
	Point3D CameraPosition { get; set; }

	/// <summary>
	/// 	Gets or sets the camera look direction.
	/// </summary>
	Vector3D CameraLookDirection { get; set; }

	/// <summary>
	/// 	Gets the 'render' command.
	/// </summary>
	ICommand RenderCommand { get; }
}