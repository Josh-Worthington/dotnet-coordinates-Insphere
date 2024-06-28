using System.Windows;
using CoordinateReader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Viewer.Interfaces.Services;
using Viewer.Interfaces.ViewModels;
using Viewer.Services;
using Viewer.ViewModels;
using Viewer.Views;

namespace Viewer;

/// <summary>
/// 	Bootstraps the application.
/// </summary>
/// <seealso cref="IDisposable"/>
public sealed class Bootstrapper : IAsyncDisposable
{
	private readonly IHost _host;

	/// <summary>
	/// 	Default constructor.
	/// </summary>
	public Bootstrapper()
	{
		var builder = Host.CreateApplicationBuilder();
		ConfigureServices(builder);
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

		application.MainWindow = _host.Services.GetService<MainWindow>();
		application.MainWindow.Show();
	}

	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		await _host.StopAsync();
		_host.Dispose();
	}

	private static void ConfigureServices(IHostApplicationBuilder builder)
	{
		// Clients
		builder.Services.AddGrpcClient<Reader.ReaderClient>(o => o.Address = new Uri("https://localhost:7039"));

		// Services
		builder.Services.AddScoped<ICoordinateReaderService, CoordinateReaderService>();
		builder.Services.AddScoped<ISphereDrawingService, SphereDrawingService>();

		// View models
		builder.Services.AddScoped<IMainWindowViewModel, MainWindowViewModel>();

		// Main window
		builder.Services.AddSingleton<MainWindow>();
	}
}