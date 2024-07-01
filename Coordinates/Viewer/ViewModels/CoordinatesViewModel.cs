using System.Collections.ObjectModel;
using System.Windows;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Viewer.Entities;
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
	public ReadOnlyObservableCollection<CoordinateEntity>? Coordinates
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
		string filePath,
		string pathId) => (await coordinateReaderService.GetCoordinatesAsync(filePath, pathId))
		.MapLeft(ex => ex.Status)
		.BiIter(
			coordinates => Coordinates = new(new ObservableCollection<CoordinateEntity>(coordinates)),
			status =>
			{
				var errorMessage = status.StatusCode switch
				{
					StatusCode.Unavailable => "Failed to retrieve the coordinates because the server is unavailable. Ensure it is running and you have set the address correctly.",
					_ => $"Failed to retrieve the coordinates. The server returned this error:\r\n\r\n{status.Detail}"
				};
				MessageBox.Show(errorMessage, "CoordinateEntity Viewer", MessageBoxButton.OK, MessageBoxImage.Error);
			});
}
