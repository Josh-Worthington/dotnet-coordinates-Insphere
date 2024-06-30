using System.Windows.Input;

namespace Viewer.Interfaces.ViewModels;

/// <summary>
/// 	Interface for dialog view model.
/// </summary>
public interface IConnectionDialogViewModel
{
	/// <summary>
	/// 	Gets or sets the port.
	/// </summary>
	string Port { get; set; }

	/// <summary>
	/// 	Gets a value indicating whether the connection was successfully established.
	/// </summary>
	bool IsConnected { get; }

	/// <summary>
	/// 	Gets or sets the 'start server' command.
	/// </summary>
	ICommand StartServerCommand { get; }

	/// <summary>
	/// 	Gets or sets the close action.
	/// </summary>
	Action? Close { get; set; }
}