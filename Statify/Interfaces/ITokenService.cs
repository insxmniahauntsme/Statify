using Statify.Models;

namespace Statify.Interfaces;

public interface ITokenService
{
	void SetAccessToken(AuthorizationData accessToken);
	AuthorizationData GetAccessToken();
}