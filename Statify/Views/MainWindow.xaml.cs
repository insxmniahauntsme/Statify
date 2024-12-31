using System.Windows;
using System.Windows.Input;

namespace Statify.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void DragWindow(object sender, MouseButtonEventArgs e)
	{
		if (sender is Window window)
		{
			if (window.WindowState == WindowState.Maximized)
			{
				window.WindowState = WindowState.Normal;
				window.DragMove();
			}
			else
			{
				window.DragMove();
			}
		}
	}
}