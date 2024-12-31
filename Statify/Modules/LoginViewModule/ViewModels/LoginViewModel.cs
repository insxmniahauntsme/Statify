using System.Diagnostics;
using System.Net.Http;
using System.Windows.Input;
using Statify.Servers;

namespace Statify.Modules.LoginViewModule.ViewModels;

public class LoginViewModel
{
	public ICommand LoginCommand { get; }
	public CallbackServer CallbackServer { get; }
	public CancellationTokenSource CancellationTokenSource { get; }
	private readonly HttpClient _httpClient;
	
	public LoginViewModel(CallbackServer callbackServer, HttpClient httpClient)
	{
		LoginCommand = new DelegateCommand(LoginWithSpotify);
		CallbackServer = callbackServer;
		CancellationTokenSource = new CancellationTokenSource();
		_httpClient = httpClient;
	}
	
	public async void LoginWithSpotify()
	{
		string? clientId = App.Configuration!["API:client-id"];
		string? callbackUrl = App.Configuration["API:callback-url"];
		string? clientSecret = App.Configuration["API:client-secret"];
		
		string responseType = "code";
		string scopes = "ugc-image-upload";

		string spotifyLoginUrl = $"https://accounts.spotify.com/authorize?client_id={clientId}&response_type=" +
		                         $"{responseType}&redirect_uri={Uri.EscapeDataString(callbackUrl!)}" +
		                         $"&scope={Uri.EscapeDataString(scopes)}";

		var authorizationCodeTask = CallbackServer.StartAsync(CancellationTokenSource.Token);

		Process.Start(new ProcessStartInfo(spotifyLoginUrl)
		{
			UseShellExecute = true
		});
		
		var timeout = Task.Delay(30000); 
		var completedTask = await Task.WhenAny(authorizationCodeTask, timeout);

		if (completedTask == timeout)
		{
			Console.WriteLine("Login timeout. No authorization code received.");
			CancellationTokenSource.Cancel(); 
		}
		else
		{
			string authorizationCode = await authorizationCodeTask;
			if (!string.IsNullOrEmpty(authorizationCode))
			{
				var accessToken = await GetAccessTokenAsync(clientId!, clientSecret!);
				Console.WriteLine($"Access Token: {accessToken}");
			}
		}
	}
	
	private async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
	{
		var tokenRequestData = new Dictionary<string, string>
		{
			{ "grant_type", "client_credentials" },
			{ "client_id", clientId },
			{ "client_secret", clientSecret }
		};

		var requestContent = new FormUrlEncodedContent(tokenRequestData);
		var response = await _httpClient.PostAsync("https://accounts.spotify.com/api/token", requestContent);
		var responseContent = await response.Content.ReadAsStringAsync();

		if (response.IsSuccessStatusCode)
		{
			var tokenData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
			return tokenData!["access_token"].ToString()!;
		}

		throw new Exception($"Failed to exchange client credentials for access token. Response: {responseContent}");
	}
}