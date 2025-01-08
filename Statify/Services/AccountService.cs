using System.IO;
using Statify.Helpers;
using Statify.Interfaces;
using Statify.Models;

namespace Statify.Services;

public class AccountService : IAccountService
{
	private readonly string _filePath = "user_data.dat";
	private User _user;

	public User User
	{
		get => _user;
		set => _user = value;
	}

	public async Task SaveLoginAsync(User user)
	{
		string encryptedData = EncryptionHelper.Encrypt(System.Text.Json.JsonSerializer.Serialize(user));
		await File.WriteAllTextAsync(_filePath, encryptedData);
	}

	public void LoadLogin()
	{
		if (File.Exists(_filePath))
		{
			var encryptedUserData = File.ReadAllText(_filePath);
			var decryptedUserData = EncryptionHelper.Decrypt(encryptedUserData);
			User = System.Text.Json.JsonSerializer.Deserialize<User>(decryptedUserData)!;
		}
	}
}