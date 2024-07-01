using Grpc.Core;
using Viewer.Common;
using Viewer.Entities;

namespace Viewer.Interfaces.Services;

public interface ICoordinateReaderService
{
	/// <summary>
	/// 	Gets coordinates asynchronous.
	/// </summary>
	/// <param name="filePath">	Full pathname of the file. </param>
	/// <param name="pathId">  	Identifier for the path. </param>
	/// <returns>
	/// 	Either the Status code of the failed call if failed, or the coordinates if successful.
	/// </returns>
	Task<Either<RpcException, IReadOnlyCollection<Coordinate>>> GetCoordinatesAsync(
		string filePath,
		string pathId);
}