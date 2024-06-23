using CoordinateReader.Interfaces.Services;
using Grpc.Core;

namespace CoordinateReader.GrpuServices;

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
	/// <param name="request">The read path request.</param>
	/// <param name="context">The server call context.</param>
	/// <returns></returns>
	public override Task<Coordinate> ReadCoordinates(ReadPath request, ServerCallContext context)
	{
		logger.LogInformation("Reading path {Id}", request.Id);
		var result = csvReaderService.ReadPath(request.FilePath, request.Id, true);
		if (!result.IsSuccess)
		{
			throw new RpcException(new Status(StatusCode.FailedPrecondition, result.ErrorString));
		}

		return Task.FromResult(result.Value);
	}
}
