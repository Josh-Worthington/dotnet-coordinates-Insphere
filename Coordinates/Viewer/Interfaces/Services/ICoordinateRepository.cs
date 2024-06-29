using System.Collections.ObjectModel;
using CoordinateReader;

namespace Viewer.Interfaces.Services;

/// <summary>
/// 	Interface for coordinate repository.
/// </summary>
public interface ICoordinateRepository
{
	/// <summary>
	/// 	Gets the coordinates.
	/// </summary>
	ReadOnlyObservableCollection<Coordinate>? Coordinates { get; set; }
}