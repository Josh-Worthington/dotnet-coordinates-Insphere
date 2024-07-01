using System.Collections.ObjectModel;
using CoordinateReader;

namespace Viewer.Interfaces.ViewModels;

/// <summary>
/// 	Interface for coordinates view model.
/// </summary>
public interface ICoordinatesViewModel
{
	/// <summary>
	/// 	Gets the coordinates.
	/// </summary>
	ReadOnlyObservableCollection<Coordinate>? Coordinates { get; }

	/// <summary>
	/// 	Reads the coordinates.
	/// </summary>
	/// <param name="filePath">	Full pathname of the file. </param>
	Task ReadCoordinates(
		string filePath);
}