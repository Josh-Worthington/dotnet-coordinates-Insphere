using System.Diagnostics;
using System.Windows.Input;

namespace Viewer.Common;

/// <summary>
/// A command whose sole purpose is to 
/// relay its functionality to other
/// objects by invoking delegates. The
/// default return value for the CanExecute
/// method is 'true'.
/// </summary>
public class RelayCommand : ICommand
{
	private readonly Action _execute;
	private readonly Func<bool>? _canExecute;

	/// <summary>
	/// Creates a new command.
	/// </summary>
	/// <param name="execute">The execution logic.</param>
	/// <param name="canExecute">The execution status logic.</param>
	public RelayCommand(
		Action execute,
		Func<bool>? canExecute = null)
	{
		_execute = execute;
		_canExecute = canExecute;
	}

	[DebuggerStepThrough]
	public bool CanExecute(object? parameters = null) => _canExecute?.Invoke() ?? true;

	public event EventHandler? CanExecuteChanged
	{
		add => CommandManager.RequerySuggested += value;
		remove => CommandManager.RequerySuggested -= value;
	}

	public void Execute(object? parameters = null) => _execute();
}
