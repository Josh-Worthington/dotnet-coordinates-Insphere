using System.Windows;

namespace Viewer;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	private readonly Bootstrapper _bootstrapper = new();

	protected override void OnStartup(StartupEventArgs e) =>
		_bootstrapper.Start(this);

	protected override async void OnExit(ExitEventArgs e)
	{
		await _bootstrapper.DisposeAsync();
		base.OnExit(e);
	}
}
