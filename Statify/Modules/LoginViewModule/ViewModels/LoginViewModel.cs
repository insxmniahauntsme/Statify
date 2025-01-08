using System.Windows.Input;
using Statify.Interfaces;
using Statify.Models;

namespace Statify.Modules.LoginViewModule.ViewModels;

public class LoginViewModel
{
	public ICommand LoginCommand { get; }
	private readonly ISpotifyAuthService _spotifyAuthService;
	private readonly IRegionManager _regionManager;
	private readonly ITokenService _tokenService;
	
	public LoginViewModel(ISpotifyAuthService spotifyAuthService, IRegionManager regionManager, ITokenService tokenService)
	{
		LoginCommand = new DelegateCommand(async void () => await LoginWithSpotify());
		_spotifyAuthService = spotifyAuthService;
		_regionManager = regionManager;
		_tokenService = tokenService;
		
	}
	
	public async Task LoginWithSpotify()
	{
		AuthorizationData authData = await _spotifyAuthService.SendAuthRequest();
		if (authData.AccessToken != null)
		{
			_tokenService.SetAccessToken(authData);
			_regionManager.RequestNavigate("MainRegion", "OverviewView");
		}
		else
		{
			Console.WriteLine("Access token is not available.");
		}
	}
}