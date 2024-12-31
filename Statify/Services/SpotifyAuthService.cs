using System.Net.Http;

namespace Statify.Services;

public class SpotifyAuthService
{
	private HttpClient _httpClient;

	public SpotifyAuthService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}
	
	
}