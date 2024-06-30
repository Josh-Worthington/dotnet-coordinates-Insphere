namespace Viewer.Common;

/// <summary>
/// 	An either, for result handling in a functional way.
/// </summary>
/// <typeparam name="TLeft"> 	Left type. </typeparam>
/// <typeparam name="TRight">	Right type. </typeparam>
/// <remarks>
///		Following convention, the Left state of an Either can be thought of as an Error state,
///		whereas the Right state is the "correct" state.
///		
///		Essentially allows any server calling function to handle exceptions by returning this,
///		instead of leaving the exception handling to the the view/view model layer.
///	</remarks>
public abstract record Either<TLeft, TRight>
{
	/// <summary>
	/// 	Matches an Either to a value, depending on.
	/// </summary>
	/// <typeparam name="TResult">	Result type. </typeparam>
	/// <param name="right">	The function that selects a value if in the Right state. </param>
	/// <param name="left"> 	The function that selects a value if in the Left state. </param>
	/// <returns>
	/// 	A Either with Right mapped to a new type.
	/// </returns>
	public abstract TResult Match<TResult>(
		Func<TRight, TResult> right,
		Func<TLeft, TResult> left);

	/// <summary>
	/// 	Map an Either's Right value to a new type, depending on what state it is in.
	/// </summary>
	/// <typeparam name="TRightOut">	The type to map Right to. </typeparam>
	/// <param name="map">	The function that maps the Right value. </param>
	/// <returns>
	/// 	A Either with Right mapped to a new type.
	/// </returns>
	public abstract Either<TLeft, TRightOut> Map<TRightOut>(
		Func<TRight, TRightOut> map);

	/// <summary>
	/// 	Map an Either's Right value to a new type, depending on what state it is in.
	/// </summary>
	/// <typeparam name="TLeftOut">		The type to map Left to. </typeparam>
	/// <param name="mapLeft">	The function that maps the Left value. </param>
	/// <returns>
	/// 	A Either with Right mapped to a new type.
	/// </returns>
	public abstract Either<TLeftOut, TRight> MapLeft<TLeftOut>(
		Func<TLeft, TLeftOut> mapLeft);

	/// <summary>
	/// 	Maps an Either's Left or Right value, depending on what state it is in.
	/// </summary>
	/// <typeparam name="TLeftOut"> 	The type to map Left to. </typeparam>
	/// <typeparam name="TRightOut">	The type to map Right to. </typeparam>
	/// <param name="map">	  	The function that maps the Right value. </param>
	/// <param name="mapLeft">	The function that maps the Left value. </param>
	/// <returns>
	/// 	A Either with Left and Right mapped to new types.
	/// </returns>
	public abstract Either<TLeftOut, TRightOut> BiMap<TLeftOut, TRightOut>(
		Func<TRight, TRightOut> map,
		Func<TLeft, TLeftOut> mapLeft);

	/// <summary>
	/// 	Performs corresponding action depending on the state of the Either.
	/// </summary>
	/// <param name="right">	The right action. </param>
	/// <param name="left"> 	The left action. </param>
	public abstract void BiIter(
		Action<TRight> right,
		Action<TLeft> left);

	/// <summary>
	/// 	Implicitly creates an either from the given left value.
	/// </summary>
	/// <param name="left">	The left value. </param>
	public static implicit operator Either<TLeft, TRight>(
		TLeft left) => new Left(left);

	/// <summary>
	/// 	Implicitly creates an either from the given right value.
	/// </summary>
	/// <param name="right"> The right value. </param>
	public static implicit operator Either<TLeft, TRight>(
		TRight right) => new Right(right);

	/// <summary>
	/// 	The Left. This record cannot be inherited.
	/// </summary>
	internal sealed record Left(
		TLeft Value) : Either<TLeft, TRight>
	{
		/// <inheritdoc/>
		public override TResult Match<TResult>(
			Func<TRight, TResult> right,
			Func<TLeft, TResult> left) => left(Value);

		/// <inheritdoc/>
		public override Either<TLeft, TRightOut> Map<TRightOut>(
			Func<TRight, TRightOut> map) => new Either<TLeft, TRightOut>.Left(Value);

		/// <inheritdoc/>
		public override Either<TLeftOut, TRight> MapLeft<TLeftOut>(
			Func<TLeft, TLeftOut> mapLeft) => mapLeft(Value);

		/// <inheritdoc/>
		public override Either<TLeftOut, TRightOut> BiMap<TLeftOut, TRightOut>(
			Func<TRight, TRightOut> map,
			Func<TLeft, TLeftOut> mapLeft) => mapLeft(Value);

		public override void BiIter(
			Action<TRight> right,
			Action<TLeft> left) => left(Value);
	}

	/// <summary>
	/// 	The Right. This record cannot be inherited.
	/// </summary>
	internal sealed record Right(
		TRight Value) : Either<TLeft, TRight>
	{
		/// <inheritdoc/>
		public override TResult Match<TResult>(
			Func<TRight, TResult> right,
			Func<TLeft, TResult> left) => right(Value);

		/// <inheritdoc/>
		public override Either<TLeft, TRightOut> Map<TRightOut>(
			Func<TRight, TRightOut> map) => map(Value);

		/// <inheritdoc/>
		public override Either<TLeftOut, TRight> MapLeft<TLeftOut>(
			Func<TLeft, TLeftOut> mapLeft) => new Either<TLeftOut, TRight>.Right(Value);

		/// <inheritdoc/>
		public override Either<TLeftOut, TRightOut> BiMap<TLeftOut, TRightOut>(
			Func<TRight, TRightOut> map,
			Func<TLeft, TLeftOut> mapLeft) => map(Value);

		public override void BiIter(
			Action<TRight> right,
			Action<TLeft> left) => right(Value);
	}
}

