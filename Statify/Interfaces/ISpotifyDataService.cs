using Statify.Models;

namespace Statify.Interfaces;

public interface ISpotifyDataService
{
	Task<User> GetCurrentUserAsync();
}