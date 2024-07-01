using CoordinateReader.Interfaces.Services;
using Grpc.Core;

namespace CoordinateReader.Services;

/// <summary>
/// 	A service for accessing readers information.
/// </summary>
/// <seealso cref="Reader.ReaderBase"/>
public class ReaderService(
	ILogger<ReaderService> logger,
	ICsvReaderService csvReaderService) : Reader.ReaderBase
{
	/// <summary>
	///		Reads coordinates for the path with given id, from CSV at given filepath.
	/// </summary>
	/// <param name="request">		The read path request.</param>
	/// <param name="coordinates">	The server stream writer to write coordinates.</param>
	/// <param name="context">		The server call context.</param>
	public override async Task ReadCoordinates(
		ReadPath request,
		IServerStreamWriter<Coordinate> coordinates,
		ServerCallContext context)
	{
		logger.LogInformation("Reading path {Id}", request.Id);
		csvReaderService.Initialise(request.FilePath, true);

		while (!csvReaderService.Completed)
		{
			Coordinate? coord = null;

			var shouldContinue = csvReaderService.ReadPath(request.Id)
				.Match(result =>
					{
						coord = result;
						return true;
					},
					error =>
					{
						// if we haven't completed the read, or the id is not correct, we continue the read
						if (error == CsvReaderService.WrongPathString) return true;
						if (csvReaderService.Completed) return true;
						logger.LogError("Failed to read path from CSV, message: {Message}", error);
						return false;
					});
			if (!shouldContinue) break;

			// else if we have a coordinate, we write it to the stream
			if (coord is not null)
			{
				await coordinates.WriteAsync(coord);
			}
		}
	}
}
