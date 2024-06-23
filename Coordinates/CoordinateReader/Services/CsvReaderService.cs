using CoordinateReader.Entities;
using CoordinateReader.Interfaces.Entities;
using CoordinateReader.Interfaces.Services;

namespace CoordinateReader.Services;

/// <summary>
/// 	A service for reading the run CSV information.
/// </summary>
/// <seealso cref="ICsvReaderService"/>
public class CsvReaderService(
	ILogger<CsvReaderService> logger) : ICsvReaderService
{
	// Set a default map in the case of no headers
	private readonly Dictionary<string, int> _headerMap = new(8)
	{
		{"ID", 0},
		{"Index", 1},
		{"X", 2},
		{"Y", 3},
		{"Z", 4},
		{"Rx", 5},
		{"Ry", 6},
		{"Rz", 7}
	};
	private const char Separator = ',';


	/// <inheritdoc/>
	public IResult<Coordinate> ReadPath(
		string csvFilePath,
		string pathId,
		bool hasHeader = true)
	{
		try
		{
			using var csv = File.OpenRead(csvFilePath);
			using var reader = new StreamReader(csv);
			string? line;

			if (hasHeader)
			{
				line = reader.ReadLine();
				if (line is null)
				{
					throw new Exception("Failed to read line");
				}
				ReadHeader(line);
			}

			line = reader.ReadLine();
			if (line is null)
			{
				throw new Exception("Failed to read line");
			}
			var result = ReadCoordinate(line);

			return Result.FromSuccess(result);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Failed to read path.");
			return Result.FromFailure<Coordinate>(ex.Message);
		}

	}

	private void ReadHeader(
		string headerLine)
	{
		var headers = headerLine.Split(Separator);

		for (var i = 0; i < headers.Length; i++)
		{
			_headerMap[headers[i]] = i;
		}
	}

	private Coordinate ReadCoordinate(
		string line)
	{
		var data = line.Split(Separator);
		return new Coordinate
		{
			Id = data[_headerMap["ID"]],
			Index = uint.Parse(data[_headerMap["Index"]]),
			X = double.Parse(data[_headerMap["X"]]),
			Y = double.Parse(data[_headerMap["Y"]]),
			Z = double.Parse(data[_headerMap["Z"]]),
			Rx = double.Parse(data[_headerMap["Rx"]]),
			Ry = double.Parse(data[_headerMap["Ry"]]),
			Rz = double.Parse(data[_headerMap["Rz"]])
		};
	}
}