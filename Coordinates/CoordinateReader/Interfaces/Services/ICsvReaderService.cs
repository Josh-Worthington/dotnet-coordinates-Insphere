using Viewer.Common;

namespace CoordinateReader.Interfaces.Services;

public interface ICsvReaderService
{
	/// <summary>
	/// 	Gets a value indicating whether the read has completed.
	/// </summary>
	bool Completed { get; }

	/// <summary>
	/// 	Initialises the CSV reader service for the CSV at the given filepath.
	/// </summary>
	/// <param name="csvFilePath">	Full pathname of the CSV file. </param>
	/// <param name="hasHeaders">  	True if the CSV has headers, false if not. </param>
	void Initialise(
		string csvFilePath,
		bool hasHeaders);

	/// <summary>
	/// 	Reads the header from the CSV.
	/// </summary>
	/// <param name="pathId">	Identifier for the path to read. </param>
	/// <returns>
	/// 	Either the current coordinate, or an error string.
	/// </returns>
	Either<string, Coordinate> ReadPath(
		string pathId);
}
