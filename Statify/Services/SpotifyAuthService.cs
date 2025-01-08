using System.Diagnostics;
using Statify.Interfaces;
using Statify.Models;
using Statify.Servers;

namespace Statify.Services;

public class SpotifyAuthService : ISpotifyAuthService
{
	private CancellationTokenSource CancellationTokenSource { get; }
	private AccessTokenServer AccessTokenServer { get; }
	private ITokenService _tokenService { get; }

	public SpotifyAuthService(AccessTokenServer accessTokenServer, ITokenService tokenService)
	{
		AccessTokenServer = accessTokenServer;
		CancellationTokenSource = new CancellationTokenSource();
		_tokenService = tokenService;
	}

	public async Task<AuthorizationData> SendAuthRequest()
	{
		string? clientId = App.Configuration!["SpotifyAPI:client-id"];
		string? callbackUrl = App.Configuration["SpotifyAPI:callback-url"];
		
		string responseType = "code";
			string scopes = "ugc-image-upload user-read-private user-read-email";

			string spotifyLoginUrl = $"https://accounts.spotify.com/authorize?response_type={responseType}" +
			                         $"&client_id={clientId}" +
			                         $"&scope={Uri.EscapeDataString(scopes)}" +
			                         $"&redirect_uri={Uri.EscapeDataString(callbackUrl!)}";

		var accessTokenTask = AccessTokenServer.StartAsync(CancellationTokenSource.Token);

		Process.Start(new ProcessStartInfo(spotifyLoginUrl)
		{
			UseShellExecute = true
		});
		
		var timeout = Task.Delay(30000); 
		var completedTask = await Task.WhenAny(accessTokenTask, timeout);

		if (completedTask == timeout)
		{
			Console.WriteLine("Login timeout. No authorization code received.");
			await CancellationTokenSource.CancelAsync(); 
		}
		else
		{
			AuthorizationData accessToken = await accessTokenTask;
			if (accessToken != null)
			{
				await CancellationTokenSource.CancelAsync();
				Console.WriteLine($"Access Token: {accessToken}");
				return accessToken;
			}
		}
		return new AuthorizationData();
	}
}