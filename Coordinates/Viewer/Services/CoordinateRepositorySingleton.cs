using System.Collections.ObjectModel;
using CoordinateReader;
using Viewer.Interfaces.Services;

namespace Viewer.Services;

/// <summary>
/// 	A coordinate repository.
/// </summary>
/// <seealso cref="ICoordinateRepository"/>
public class CoordinateRepositorySingleton : ICoordinateRepository
{
	/// <inheritdoc/>
	public ReadOnlyObservableCollection<Coordinate>? Coordinates { get; set; }
}
