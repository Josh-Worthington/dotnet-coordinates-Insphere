using CoordinateReader.Interfaces.Entities;

namespace CoordinateReader.Entities;

public readonly struct Result<T> : IResult<T>
{
	/// <inheritdoc/>
	public T Value { get; init; }

	/// <inheritdoc/>
	public bool IsSuccess { get; init; }

	/// <inheritdoc/>
	public string ErrorString { get; init; }
}

public static class Result
{
	/// <summary>
	/// 	Creates a new successful result from the given value.
	/// </summary>
	/// <typeparam name="T">	Generic type parameter. </typeparam>
	/// <param name="value">	The value. </param>
	public static IResult<T> FromSuccess<T>(
		T value) => new Result<T> { Value = value, IsSuccess = true };

	/// <summary>
	/// 	Creates a new failed result from the given error string.
	/// </summary>
	/// <typeparam name="T">	Generic type parameter. </typeparam>
	/// <param name="errorString">	The error string. </param>
	public static IResult<T> FromFailure<T>(
		string errorString) => new Result<T> { ErrorString = errorString, IsSuccess = false };
}