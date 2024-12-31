using System.Windows;
using System.Windows.Input;

namespace Statify.ViewModels
{
	public class MainWindowViewModel
	{
		public ICommand MinimizeWindowCommand { get; }
		public ICommand RestoreOrMaximizeWindowCommand { get; }
		public ICommand CloseWindowCommand { get; }
		public MainWindowViewModel()
		{
			MinimizeWindowCommand = new DelegateCommand<Window>(MinimizeWindow);
			RestoreOrMaximizeWindowCommand = new DelegateCommand<Window>(RestoreOrMaximizeWindow);
			CloseWindowCommand = new DelegateCommand<Window>(CloseWindow);
			
		}

		private void MinimizeWindow(Window window)
		{
			window.WindowState = WindowState.Minimized;
			
		}

		private void RestoreOrMaximizeWindow(Window window)
		{
			window.WindowState = window.WindowState == WindowState.Maximized
					? WindowState.Normal
					: WindowState.Maximized;
			
		}
		private void CloseWindow(Window window)
		{
			window.Close();
			
		}

		
	}
}