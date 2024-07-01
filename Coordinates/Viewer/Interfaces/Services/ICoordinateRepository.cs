using System.Collections.ObjectModel;
using Viewer.Entities;

namespace Viewer.Interfaces.Services;

/// <summary>
/// 	Interface for coordinate repository.
/// </summary>
public interface ICoordinateRepository
{
	/// <summary>
	/// 	Gets or sets the coordinates.
	/// </summary>
	ReadOnlyObservableCollection<Coordinate>? Coordinates { get; set; }
}