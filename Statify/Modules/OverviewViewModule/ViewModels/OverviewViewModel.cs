using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Statify.Interfaces;
using Statify.Models;

namespace Statify.Modules.OverviewViewModule.ViewModels;

public class OverviewViewModel : INavigationAware, INotifyPropertyChanged
{
	private readonly ISpotifyDataService _spotifyDataService;
	private User _user;

	public User User
	{
		get { return _user; }
		set 
		{ 
			_user = value;
			OnPropertyChanged();
		}
	}

	public OverviewViewModel(ISpotifyDataService spotifyDataService)
	{
		_spotifyDataService = spotifyDataService;
	}

	public void OnNavigatedTo(NavigationContext navigationContext)
	{
		LoadUserData();
	}
	private async void LoadUserData()
	{
		try
		{
			User = await _spotifyDataService.GetCurrentUserAsync();
			Console.WriteLine($"Current user: {User.DisplayName}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to get current user: {ex.Message}");
		}
	}
	
	public bool IsNavigationTarget(NavigationContext navigationContext)
	{
		return false; 
	}

	public void OnNavigatedFrom(NavigationContext navigationContext) { }
	public event PropertyChangedEventHandler? PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

}