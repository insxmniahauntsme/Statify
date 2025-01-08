using Statify.Models;

namespace Statify.Interfaces;

public interface ISpotifyAuthService
{
	Task<AuthorizationData> SendAuthRequest();
}