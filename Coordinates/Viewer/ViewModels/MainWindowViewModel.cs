using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
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
	private readonly ICoordinateRepository _coordinateRepository;

	/// <summary>
	/// 	Constructor.
	/// </summary>
	/// <exception cref="ArgumentNullException">	Thrown when one or more required arguments are null. </exception>
	/// <param name="logger">				  	The logger. </param>
	/// <param name="coordinateReaderService">	The coordinate reader service. </param>
	/// <param name="coordinateRepository">   	The coordinate repository. </param>
	/// <param name="display3DViewModel">	  	The display 3D view model. </param>
	public MainWindowViewModel(
		ILogger<MainWindowViewModel> logger,
		ICoordinateReaderService coordinateReaderService,
		ICoordinateRepository coordinateRepository,
		IDisplay3DViewModel display3DViewModel) : base(logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		_coordinateReaderService = coordinateReaderService ?? throw new ArgumentNullException(nameof(coordinateReaderService));
		_coordinateRepository = coordinateRepository ?? throw new ArgumentNullException(nameof(coordinateRepository));
		Display3DViewModel = display3DViewModel ?? throw new ArgumentNullException(nameof(display3DViewModel));

		SelectFileCommand = new RelayCommand(SelectFile);
		RetrieveCoordinatesCommand = new RelayCommandAsync(RetrieveCoordinates, () => FilePath is not null);

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
			(await _coordinateReaderService.GetCoordinatesAsync(FilePath!))
				.BiMap(
					coordinates => new ReadOnlyObservableCollection<Coordinate>(new ObservableCollection<Coordinate>(coordinates)),
					ex => ex.Status)
				.BiIter(
					coordinates => Coordinates = coordinates,
					status =>
					{
						var errorMessage = status.StatusCode switch
						{
							StatusCode.Unavailable => "Failed to retrieve the coordinates because the server is unavailable. Ensure it is running and you have set the address correctly.",
							_ => $"Failed to retrieve the coordinates. The server returned this error:\r\n\r\n{status.Detail}"
						};
						MessageBox.Show(errorMessage, "Coordinate Viewer", MessageBoxButton.OK, MessageBoxImage.Error);
					});

			_logger.LogInformation("Retrieved coordinates");
		}
	}

	/// <inheritdoc/>
	public IDisplay3DViewModel Display3DViewModel { get; }

	/// <inheritdoc/>
	public ReadOnlyObservableCollection<Coordinate>? Coordinates
	{
		get => _coordinateRepository.Coordinates;
		private set
		{
			_coordinateRepository.Coordinates = value;
			RaisePropertyChanged();
		}
	}

	/// <inheritdoc/>
	public string? FilePath
	{
		get => GetValue<string?>();
		private set => SetValue(value);
	}

	/// <inheritdoc/>
	public ICommand SelectFileCommand { get; }

	/// <inheritdoc/>
	public ICommand RetrieveCoordinatesCommand { get; }
}
