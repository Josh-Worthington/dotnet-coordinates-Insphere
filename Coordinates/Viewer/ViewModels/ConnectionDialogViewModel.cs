﻿using System.Windows;
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
	/// <param name="serverConnectionService">	The start server service. </param>
	/// <param name="configurationService">   	The configuration service. </param>
	public ConnectionDialogViewModel(
		ILogger<ConnectionDialogViewModel> logger,
		IServerConnectionService serverConnectionService,
		IConfigurationService configurationService) : base(logger)
	{
		_configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));

		StartServerCommand = new RelayCommand(StartServer);

		return;

		void StartServer()
		{
			IsConnected = serverConnectionService.StartServer(Port);

			if (!IsConnected)
			{
				var message = MessageBox.Show("Failed to start the server. Would you like to try again?", "Coordinate Viewer", MessageBoxButton.YesNo, MessageBoxImage.Error);
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
				MessageBox.Show("Port must only contain numbers.", "Coordinate Reader", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
	}

	/// <inheritdoc/>
	public bool IsConnected { get; private set; }

	/// <inheritdoc/>
	public ICommand StartServerCommand { get; }

	public Action? Close { get; set; }
}