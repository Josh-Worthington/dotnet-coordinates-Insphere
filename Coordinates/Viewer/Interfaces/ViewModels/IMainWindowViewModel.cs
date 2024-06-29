using System.Collections.ObjectModel;
using System.Windows.Input;
using CoordinateReader;

namespace Viewer.Interfaces.ViewModels;

public interface IMainWindowViewModel
{
	/// <summary>
	/// 	Gets the display 3D view model.
	/// </summary>
	IDisplay3DViewModel Display3DViewModel { get; }

	/// <summary>
	/// 	Gets the coordinates.
	/// </summary>
	ReadOnlyObservableCollection<Coordinate>? Coordinates { get; }

	/// <summary>
	/// 	Gets the full pathname of the file.
	/// </summary>
	string? FilePath { get; }

	/// <summary>
	/// 	Gets the 'select file' command.
	/// </summary>
	ICommand SelectFileCommand { get; }

	/// <summary>
	/// 	Gets the 'retrieve coordinates' command.
	/// </summary>
	ICommand RetrieveCoordinatesCommand { get; }
}