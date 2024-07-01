using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Viewer.Common;
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

	/// <summary>
	/// 	Constructor.
	/// </summary>
	/// <exception cref="ArgumentNullException">	Thrown when one or more required arguments are null. </exception>
	/// <param name="logger">				  	The logger. </param>
	/// <param name="coordinatesViewModel">   	The coordinates view model. </param>
	/// <param name="display3DViewModel">	  	The display 3D view model. </param>
	public MainWindowViewModel(
		ILogger<MainWindowViewModel> logger,
		ICoordinatesViewModel coordinatesViewModel,
		IDisplay3DViewModel display3DViewModel) : base(logger)
	{
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));

		CoordinatesViewModel = coordinatesViewModel ?? throw new ArgumentNullException(nameof(coordinatesViewModel));
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
			await CoordinatesViewModel.ReadCoordinates(FilePath!);

			_logger.LogInformation("Retrieved coordinates");
		}
	}

	/// <inheritdoc/>
	public IDisplay3DViewModel Display3DViewModel { get; }

	/// <inheritdoc/>
	public ICoordinatesViewModel CoordinatesViewModel { get; }

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
