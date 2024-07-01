using System.Collections.ObjectModel;
using Viewer.Entities;

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
	/// <param name="pathId">  	Identifier for the path. </param>
	Task ReadCoordinates(
		string filePath,
		string pathId);
}