using System.Windows;
using Viewer.Interfaces.ViewModels;

namespace Viewer.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow(IMainWindowViewModel mainWindowViewModel)
	{
		InitializeComponent();
		DataContext = mainWindowViewModel;
	}
}
