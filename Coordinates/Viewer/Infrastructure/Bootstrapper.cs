using System.Windows;
using CoordinateReader;
using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Extensions;
using Viewer.Interfaces.Services;
using Viewer.Interfaces.ViewModels;
using Viewer.Services;
using Viewer.ViewModels;
using Viewer.Views;
using Application = System.Windows.Application;

namespace Viewer;

/// <summary>
/// 	Bootstraps the application.
/// </summary>
/// <seealso cref="IDisposable"/>
public sealed class Bootstrapper : IAsyncDisposable
{
	private IHost _host;

	/// <summary>
	/// 	Default constructor.
	/// </summary>
	public Bootstrapper()
	{
		var builder = CreateBaseBuilder();
		_host = builder.Build();
	}

	/// <summary>
	/// 	Starts the given application.
	/// </summary>
	/// <param name="application">	The application. </param>
	public void Start(
		Application application)
	{
		ArgumentNullException.ThrowIfNull(application);

		application.ShutdownMode = ShutdownMode.OnExplicitShutdown;

		GrpcChannel? connection = null;
		var connectionService = _host.Services.GetService<IServerConnectionService>();
		if (connectionService.IsServerRunning() && connectionService.EstablishConnection())
		{
			connection = connectionService.Connection;
		}
		else
		{
			var dialogWindow = _host.Services.GetService<DialogWindow>()!;

			if (dialogWindow.ShowDialog() is true)
			{
				connection = connectionService.Connection;
			}
		}

		if (connection is null)
		{
			throw new Exception("Failed to connect.");
		}

		Run(application);
	}

	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		await _host.StopAsync();
		_host.Dispose();
	}

	private void Run(Application application)
	{
		application.MainWindow = _host.Services.GetService<MainWindow>();
		application.MainWindow!.Show();
		application.ShutdownMode = ShutdownMode.OnMainWindowClose;
	}

	private HostApplicationBuilder CreateBaseBuilder()
	{
		var builder = Host.CreateApplicationBuilder();
		ConfigureServices(builder.Services);
		builder.Services.AddSharedServices();
		return builder;
	}

	private static void ConfigureServices(IServiceCollection services)
	{
		// Repositories
		services.AddSingleton<ICoordinateRepository, CoordinateRepositorySingleton>();

		// Clients
		services.AddSingleton(x => new Ping.PingClient(x.GetRequiredService<IServerConnectionService>().Connection));
		services.AddSingleton(x => new Reader.ReaderClient(x.GetRequiredService<IServerConnectionService>().Connection));

		// Services
		services.AddScoped<IPingService, PingService>();
		services.AddScoped<ICoordinateReaderService, CoordinateReaderService>();
		services.AddScoped<ISphereDrawingService, SphereDrawingService>();
		services.AddSingleton<IServerConnectionService, ServerConnectionService>();

		// View models
		services.AddScoped<IMainWindowViewModel, MainWindowViewModel>();
		services.AddScoped<IConnectionDialogViewModel, ConnectionDialogViewModel>();
		services.AddScoped<IDisplay3DViewModel, Display3DViewModel>();

		// Windows
		services.AddSingleton<MainWindow>();
		services.AddSingleton<DialogWindow>();
	}
}