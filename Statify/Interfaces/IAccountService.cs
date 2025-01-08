using Statify.Models;

namespace Statify.Interfaces;

public interface IAccountService
{
	Task SaveLoginAsync(User user);
	void LoadLogin();
}