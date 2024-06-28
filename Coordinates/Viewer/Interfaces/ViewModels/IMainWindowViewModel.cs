using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using CoordinateReader;

namespace Viewer.Interfaces.ViewModels;

public interface IMainWindowViewModel
{
	/// <summary>
	/// 	Gets the 'select file' command.
	/// </summary>
	ICommand SelectFileCommand { get; }

	/// <summary>
	/// 	Gets the 'draw spheres' command.
	/// </summary>
	ICommand DrawSpheresCommand { get; }

	/// <summary>
	/// 	Gets the 'retrieve coordinates' command.
	/// </summary>
	ICommand RetrieveCoordinatesCommand { get; }

	/// <summary>
	/// 	Gets the coordinates.
	/// </summary>
	ReadOnlyObservableCollection<Coordinate> Coordinates { get; }

	/// <summary>
	/// 	Gets the full pathname of the file.
	/// </summary>
	string? FilePath { get; }

	/// <summary>
	/// 	Gets the mesh geometry.
	/// </summary>
	MeshGeometry3D Geometry { get; }
}