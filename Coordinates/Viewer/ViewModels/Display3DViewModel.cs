using System.Windows.Input;
using System.Windows.Media.Media3D;
using Microsoft.Extensions.Logging;
using Viewer.Common;
using Viewer.Interfaces.Services;
using Viewer.Interfaces.ViewModels;

namespace Viewer.ViewModels;

/// <summary>
/// 	A ViewModel for the 3D display.
/// </summary>
/// <seealso cref="ViewModelBase"/>
/// <seealso cref="IDisplay3DViewModel"/>
public class Display3DViewModel : ViewModelBase, IDisplay3DViewModel
{
	/// <summary>
	/// 	Constructor.
	/// </summary>
	/// <param name="sphereDrawingService">	The sphere drawing service. </param>
	/// <param name="logger">			   	The logger. </param>
	public Display3DViewModel(
		ISphereDrawingService sphereDrawingService,
		ILogger<Display3DViewModel> logger) : base(logger)
	{
		ArgumentNullException.ThrowIfNull(sphereDrawingService);

		Points = new Point3DCollection();

		CameraPosition = new Point3D(0, 0, 0);
		CameraLookDirection = new Vector3D(100, 100, 100);

		RenderCommand = new RelayCommand(DrawSpheres);

		return;

		void DrawSpheres()
		{
			// Draw spheres around the coordinates; if successful, update the camera to look at the centre of all the points
			sphereDrawingService.DrawSpheres(Points)
				.BiIter(
					centre =>
					{
						//CameraPosition = centre;
						CameraLookDirection = new Vector3D(centre.X, centre.Y, centre.Z);
					},
					ex => logger.LogError(ex, "Failed to draw spheres."));

			logger.LogInformation("Position: {CameraPosition}", CameraPosition);
			logger.LogInformation("Position: {CameraLookDirection}", CameraLookDirection);

			RaisePropertiesChanged(nameof(Points), nameof(CameraPosition), nameof(CameraLookDirection));
		}
	}

	/// <inheritdoc/>
	public Point3DCollection Points
	{
		get => GetValue<Point3DCollection>();
		private init => SetValue(value);
	}

	/// <inheritdoc/>
	public Point3D CameraPosition
	{
		get => GetValue<Point3D>();
		set => SetValue(value);
	}

	/// <inheritdoc/>
	public Vector3D CameraLookDirection
	{
		get => GetValue<Vector3D>();
		set => SetValue(value);
	}

	/// <inheritdoc/>
	public ICommand RenderCommand { get; }
}