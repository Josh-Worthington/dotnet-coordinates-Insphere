using System.Diagnostics;
using System.Windows.Input;

namespace Viewer.Common;

/// <summary>
/// Creates a new command.
/// </summary>
/// <param name="execute">The execution logic.</param>
/// <param name="canExecute">The execution status logic.</param>
public class RelayCommandAsync(
	Func<Task> execute,
	Func<bool>? canExecute = null) : ICommand
{
	private Task? _task;

	/// <inheritdoc />
	public event EventHandler? CanExecuteChanged
	{
		add => CommandManager.RequerySuggested += value;
		remove => CommandManager.RequerySuggested -= value;
	}

	/// <inheritdoc />
	[DebuggerStepThrough]
	public bool CanExecute(
		object? parameters = null)
	{
		if (_task is { IsCompleted: true })
		{
			_task = null;
		}
		return canExecute?.Invoke() ?? _task is not { IsCompleted: false };
	}

	/// <inheritdoc />
	public void Execute(
		object? parameters = null) => _task = execute();
}