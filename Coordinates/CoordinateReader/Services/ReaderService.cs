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
			var result = csvReaderService.ReadPath(request.Id);
			if (result.IsSuccess)
			{
				await coordinates.WriteAsync(result.Value);
			}
			else
			{
				if (csvReaderService.Completed)
				{
					logger.LogInformation("Finished reading CSV {Name}", Path.GetFileName(request.FilePath));
				}
				else
				{
					logger.LogError("Failed to read path from CSV, message: {Message}", result.ErrorString);
					break;
				}
			}
		}
	}
}
