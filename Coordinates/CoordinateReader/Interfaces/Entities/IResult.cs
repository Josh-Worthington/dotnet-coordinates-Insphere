namespace CoordinateReader.Interfaces.Entities;

public interface IResult<TResult>
{
	/// <summary>
	/// 	Gets or initialises the result.
	/// </summary>
	TResult Value { get; init; }

	/// <summary>
	/// 	Gets or initialises a value indicating whether this result is a success.
	/// </summary>
	bool IsSuccess { get; init; }

	/// <summary>
	/// 	Gets or initialises the error string.
	/// </summary>
	string ErrorString { get; init; }
}