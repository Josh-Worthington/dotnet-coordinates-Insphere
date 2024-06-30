using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Viewer.Interfaces.ViewModels;

namespace Viewer.Views;

/// <summary>
/// Interaction logic for DialogWindow.xaml
/// </summary>
public partial class DialogWindow : Window
{
	private static readonly Regex NumbersOnlyRegex = new("[^0-9]+");

	public DialogWindow(IConnectionDialogViewModel connectionDialogViewModel)
	{
		InitializeComponent();
		DataContext = connectionDialogViewModel;

		connectionDialogViewModel.Close ??= Close;
	}

	private void PortTextChanged(
		object sender,
		TextCompositionEventArgs e)
	{
		e.Handled = NumbersOnlyRegex.IsMatch(e.Text);
	}

	protected override void OnClosing(
		CancelEventArgs e)
	{
		DialogResult = (DataContext as IConnectionDialogViewModel)?.IsConnected;
		base.OnClosing(e);
	}
}
