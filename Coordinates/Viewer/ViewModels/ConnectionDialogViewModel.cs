using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Shared.Interfaces;
using Viewer.Common;
using Viewer.Interfaces.Services;
using Viewer.Interfaces.ViewModels;

namespace Viewer.ViewModels;

public class ConnectionDialogViewModel : ViewModelBase, IConnectionDialogViewModel
{
	private readonly IConfigurationService _configurationService;

	/// <summary>
	/// 	Constructor.
	/// </summary>
	/// <exception cref="ArgumentNullException">	Thrown when one or more required arguments are null. </exception>
	/// <param name="logger">				  	The logger. </param>
	/// <param name="connectionService">	The start server service. </param>
	/// <param name="configurationService">   	The configuration service. </param>
	public ConnectionDialogViewModel(
		ILogger<ConnectionDialogViewModel> logger,
		IConnectionService connectionService,
		IConfigurationService configurationService) : base(logger)
	{
		_configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));

		StartServerCommand = new RelayCommand(StartServer);

		return;

		void StartServer()
		{
			IsConnected = connectionService.StartServer(Port);

			if (!IsConnected)
			{
				var message = MessageBox.Show("Failed to start the server. Would you like to try again?", "CoordinateEntity Viewer", MessageBoxButton.YesNo, MessageBoxImage.Error);
				if (message is MessageBoxResult.Yes) return;
			}

			Close?.Invoke();
		}
	}

	/// <inheritdoc/>
	public string Port
	{
		get => _configurationService.Config.Port.ToString();
		set
		{
			if (int.TryParse(value, out var port))
			{
				_configurationService.Config.Port = port;
				RaisePropertyChanged();
			}
			else
			{
				MessageBox.Show("Port must only contain numbers.", "CoordinateEntity Reader", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
	}

	/// <inheritdoc/>
	public bool IsConnected { get; private set; }

	/// <inheritdoc/>
	public ICommand StartServerCommand { get; }

	/// <inheritdoc/>
	public Action? Close { get; set; }
}