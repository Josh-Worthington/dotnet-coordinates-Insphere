using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using CoordinateReader;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Viewer.Common;
using Viewer.Interfaces.Services;
using Viewer.Interfaces.ViewModels;

namespace Viewer.ViewModels;

/// <summary>
/// 	A ViewModel for the main window. This class cannot be inherited.
/// </summary>
/// <seealso cref="ViewModelBase"/>
/// <seealso cref="IMainWindowViewModel"/>
public sealed class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
	private readonly ILogger<MainWindowViewModel> _logger;
	private readonly ICoordinateReaderService _coordinateReaderService;

	/// <summary>
	/// 	Constructor.
	/// </summary>
	/// <exception cref="ArgumentNullException">	Thrown when one or more required arguments are null. </exception>
	/// <param name="logger">				  	The logger. </param>
	/// <param name="coordinateReaderService">	The coordinate reader service. </param>
	/// <param name="sphereDrawingService">   	The sphere drawing service. </param>
	public MainWindowViewModel(
		ILogger<MainWindowViewModel> logger,
		ICoordinateReaderService coordinateReaderService,
		ISphereDrawingService sphereDrawingService) : base(logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_coordinateReaderService = coordinateReaderService ?? throw new ArgumentNullException(nameof(coordinateReaderService));

		SelectFileCommand = new RelayCommand(SelectFile);
		DrawSpheresCommand = new RelayCommand(DrawSpheres);
		RetrieveCoordinatesCommand = new RelayCommandAsync(RetrieveCoordinates, () => FilePath is not null);

		Geometry = new MeshGeometry3D();

		logger.LogInformation("Main Window View Model loaded");

		return;

		void SelectFile()
		{
			var openFileDialog = new OpenFileDialog { Filter = "CSV Files|*.csv" };

			var success = openFileDialog.ShowDialog();

			if (success is true)
			{
				FilePath = openFileDialog.FileName;
			}
		}

		async Task RetrieveCoordinates()
		{
			IsSuccessful = (await _coordinateReaderService.GetCoordinatesAsync(FilePath!))
				.BiMap(
					coordinates => new ReadOnlyObservableCollection<Coordinate>(new ObservableCollection<Coordinate>(coordinates)),
					ex => ex.Status)
				.Match(
					coordinates =>
					{
						Coordinates = coordinates;
						return true;
					},
					status =>
					{
						var errorMessage = status.StatusCode switch
						{
							StatusCode.Unavailable => "Failed to retrieve the coordinates because the server is unavailable. Ensure it is running and you have set the address correctly.",
							_ => $"Failed to retrieve the coordinates. The server returned this error:\r\n\r\n{status.Detail}"
						};
						MessageBox.Show(errorMessage, "Coordinate Viewer", MessageBoxButton.OK, MessageBoxImage.Error);
						return false;
					});

			_logger.LogInformation("Retrieved coordinates");
		}

		void DrawSpheres()
		{
			sphereDrawingService.DrawSpheresAtCoordinates(Geometry, Coordinates);
			RaisePropertyChanged(nameof(Geometry));
		}
	}

	/// <inheritdoc/>
	public ICommand SelectFileCommand { get; }

	/// <inheritdoc/>
	public ICommand DrawSpheresCommand { get; }

	/// <inheritdoc/>
	public ICommand RetrieveCoordinatesCommand { get; }

	/// <inheritdoc/>
	public ReadOnlyObservableCollection<Coordinate> Coordinates
	{
		get => GetValue<ReadOnlyObservableCollection<Coordinate>>();
		private set => SetValue(value);
	}

	/// <inheritdoc/>
	public string? FilePath
	{
		get => GetValue<string?>();
		private set => SetValue(value);
	}

	/// <inheritdoc/>
	public MeshGeometry3D Geometry
	{
		get => GetValue<MeshGeometry3D>();
		private init => SetValue(value);
	}


	public bool IsSuccessful
	{
		get => GetValue<bool>();
		private set => SetValue(value);
	}
}
