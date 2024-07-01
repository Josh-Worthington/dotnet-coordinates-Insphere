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
	/// <param name="logger">			   	The logger. </param>
	/// <param name="coordinateRepository">	The coordinate repository. </param>
	/// <param name="pointMathService">		   	The point service. </param>
	public Display3DViewModel(
		ILogger<Display3DViewModel> logger,
		ICoordinateRepository coordinateRepository,
		IPointMathService pointMathService) : base(logger)
	{
		ArgumentNullException.ThrowIfNull(pointMathService);

		Points = new Point3DCollection();
		CameraLookDirection = default;

		RenderCommand = new RelayCommand(DrawPoints, () => coordinateRepository.Coordinates?.Count > 0);

		return;

		void DrawPoints()
		{
			// Draw the coordinates as points, then update the camera to look at the centre of all the points to bring it into view
			Points = (Point3DCollection)coordinateRepository.Coordinates!.Select(x => x.Position);
			CameraLookDirection = (Vector3D)pointMathService.CalculateCentroid(Points);

			RaisePropertiesChanged(nameof(Points), nameof(CameraLookDirection));

			logger.LogInformation("Position: {CameraLookDirection}", CameraLookDirection);
		}
	}

	/// <inheritdoc/>
	public Point3DCollection Points
	{
		get => GetValue<Point3DCollection>();
		private set => SetValue(value);
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