using CoordinateReader.Interfaces.Services;
using Shared.Entities;

namespace CoordinateReader.Services;

/// <summary>
/// 	A service for reading the run CSV information.
/// </summary>
/// <seealso cref="ICsvReaderService"/>
public sealed class CsvReaderService(
	ILogger<CsvReaderService> logger) : ICsvReaderService, IDisposable
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

	private bool _initialised;
	private StreamReader? _reader;

	/// <summary>
	/// 	The wrong path string.
	/// </summary>
	public static readonly string WrongPathString = "Coordinate has the wrong path id";

	/// <inheritdoc/>
	public bool Completed { get; private set; }

	/// <inheritdoc/>
	public void Initialise(
		string csvFilePath,
		bool hasHeaders)
	{
		if (_initialised) return;

		_reader = new StreamReader(File.OpenRead(csvFilePath));

		if (!hasHeaders) return;

		if (_reader.ReadLine() is not { } headers)
		{
			logger.LogWarning("Expected headers but CSV was empty.");
			return;
		}
		ReadHeader(headers);

		_initialised = true;
	}

	/// <inheritdoc/>
	public Either<string, Coordinate> ReadPath(
		string pathId)
	{
		if (!_initialised || _reader is null)
		{
			throw new Exception("Service has not been initialised.");
		}

		if (_reader.ReadLine() is not { } line)
		{
			Completed = true;
			return "Reached the end of the stream.";
		}

		var result = ReadCoordinate(line);
		if (result.Id != pathId)
		{
			return WrongPathString;
		}

		return result;
	}

	/// <inheritdoc/>
	public void Dispose() => _reader?.Dispose();

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