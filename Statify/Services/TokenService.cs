using Statify.Interfaces;
using Statify.Models;

namespace Statify.Services;

public class TokenService : ITokenService
{
	private AuthorizationData _accessToken;

	public AuthorizationData AccessToken
	{
		get => _accessToken;
		set => _accessToken = value;
	}
	
	public void SetAccessToken(AuthorizationData accessToken)
	{
		_accessToken = accessToken;
	}

	public AuthorizationData GetAccessToken()
	{
		return _accessToken;
	}
}