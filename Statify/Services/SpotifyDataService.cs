using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Statify.Interfaces;
using Statify.Models;

namespace Statify.Services;

public class SpotifyDataService : ISpotifyDataService
{
	private readonly HttpClient _httpClient;
	private readonly AuthorizationData _accessToken;

	public SpotifyDataService(ITokenService tokenService, HttpClient httpClient)
	{
		_accessToken = tokenService.GetAccessToken();
		_httpClient = httpClient;
		
	}

	public async Task<User> GetCurrentUserAsync()
	{
		var requestUrl = "v1/me";

		var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
		requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken.AccessToken);

		var response = await _httpClient.SendAsync(requestMessage);

		Console.WriteLine($"Response Code: {response.StatusCode}");
		
		if (!response.IsSuccessStatusCode)
		{
			var errorContent = await response.Content.ReadAsStringAsync();
			throw new HttpRequestException($"Request failed with status code {response.StatusCode}. Response: {errorContent}");
		}

		var content = await response.Content.ReadAsStringAsync();
		Console.WriteLine(content);
		var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };
		return JsonSerializer.Deserialize<User>(content, options) ??
		       throw new InvalidOperationException("Failed to deserialize User.");
		
	}
}