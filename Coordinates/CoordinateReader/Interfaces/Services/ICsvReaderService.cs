using CoordinateReader.Interfaces.Entities;

namespace CoordinateReader.Interfaces.Services;

public interface ICsvReaderService
{
	/// <summary>
	/// 	Reads a path from CSV at given filepath.
	/// </summary>
	/// <param name="csvFilePath">	Full pathname of the CSV file to read. </param>
	/// <param name="pathId">	  	Identifier for the path to read. </param>
	/// <param name="hasHeader">  	True if csv has header, false if not. </param>
	IResult<Coordinate> ReadPath(
		string csvFilePath,
		string pathId,
		bool hasHeader);
}