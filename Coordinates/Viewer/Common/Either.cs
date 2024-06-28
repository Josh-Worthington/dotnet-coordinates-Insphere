namespace Viewer.Common;

public class Either<TLeft, TRight>
{
	private Either(TLeft? left)
	{
		Left = left;
	}

	private Either(TRight? right)
	{
		Right = right;
	}

	public bool IsLeft => Left is not null;

	public TLeft? Left { get; }

	public TRight? Right { get; }

	public static implicit operator Either<TLeft, TRight>(
		TLeft left) => FromLeft(left);

	public static implicit operator Either<TLeft, TRight>(
		TRight right) => FromRight(right);

	public static Either<TLeft, TRight> FromLeft(
		TLeft left) => new(left);

	public static Either<TLeft, TRight> FromRight(
		TRight right) => new(right);
}

public static class EitherExtensions
{
	public static TResult Match<TLeft, TRight, TResult>(
		this Either<TLeft, TRight> either,
		Func<TRight, TResult> right,
		Func<TLeft, TResult> left)
	{
		ArgumentNullException.ThrowIfNull(either);

		return either.IsLeft ? left(either.Left!) : right(either.Right!);
	}

	public static Either<TInLeft, TRight> Map<TInLeft, TInRight, TRight>(
		this Either<TInLeft, TInRight> either,
		Func<TInRight, TRight> map)
	{
		ArgumentNullException.ThrowIfNull(either);

		return either.IsLeft ?
				   Either<TInLeft, TRight>.FromLeft(either.Left!) :
				   Either<TInLeft, TRight>.FromRight(map(either.Right!));
	}

	public static Either<TLeft, TRight> BiMap<TInLeft, TInRight, TLeft, TRight>(
		this Either<TInLeft, TInRight> either,
		Func<TInRight, TRight> map,
		Func<TInLeft, TLeft> mapLeft)
	{
		ArgumentNullException.ThrowIfNull(either);

		return either.IsLeft ?
				   Either<TLeft, TRight>.FromLeft(mapLeft(either.Left!)) :
				   Either<TLeft, TRight>.FromRight(map(either.Right!));
	}
}