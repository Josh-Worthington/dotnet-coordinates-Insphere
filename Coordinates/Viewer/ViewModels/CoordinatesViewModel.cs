using System.Collections.ObjectModel;
using System.Windows;
using CoordinateReader;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Viewer.Interfaces.Services;
using Viewer.Interfaces.ViewModels;

namespace Viewer.ViewModels;

/// <summary>
/// 	A ViewModel for the coordinates.
/// </summary>
/// <seealso cref="ViewModelBase"/>
/// <seealso cref="ICoordinatesViewModel"/>
public class CoordinatesViewModel(
	ILogger<CoordinatesViewModel> logger,
	ICoordinateRepository coordinateRepository,
	ICoordinateReaderService coordinateReaderService) : ViewModelBase(logger), ICoordinatesViewModel
{
	/// <inheritdoc />
	public ReadOnlyObservableCollection<Coordinate>? Coordinates
	{
		get => coordinateRepository.Coordinates;
		set
		{
			coordinateRepository.Coordinates = value;
			RaisePropertyChanged();
		}
	}

	/// <inheritdoc />
	public async Task ReadCoordinates(
		string filePath) => (await coordinateReaderService.GetCoordinatesAsync(filePath))
		.MapLeft(ex => ex.Status)
		.BiIter(
			coordinates => Coordinates = new ReadOnlyObservableCollection<Coordinate>(new ObservableCollection<Coordinate>(coordinates)),
			status =>
			{
				var errorMessage = status.StatusCode switch
				{
					StatusCode.Unavailable => "Failed to retrieve the coordinates because the server is unavailable. Ensure it is running and you have set the address correctly.",
					_ => $"Failed to retrieve the coordinates. The server returned this error:\r\n\r\n{status.Detail}"
				};
				MessageBox.Show(errorMessage, "Coordinate Viewer", MessageBoxButton.OK, MessageBoxImage.Error);
			});
}
