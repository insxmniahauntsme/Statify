using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Statify.Helpers;

public class EncryptionHelper
{
	private static readonly string _encryptionKey = Environment.GetEnvironmentVariable("ENCRYPTION_KEY")!;

	public static string Encrypt(string plainText)
	{
		using (Aes aesAlg = Aes.Create())
		{
			aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionKey);
			aesAlg.IV = new byte[16];
			
			ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
			using (MemoryStream stream = new MemoryStream())
				using (CryptoStream cs = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
					using (StreamWriter sw = new StreamWriter(cs))
					{
						sw.Write(plainText);
						return Convert.ToBase64String(stream.ToArray());
					}
		}
	}

	public static string Decrypt(string cipherText)
	{
		using (Aes aesAlg = Aes.Create())
		{
			aesAlg.Key = Encoding.UTF8.GetBytes(_encryptionKey);
			aesAlg.IV = new byte[16];
			
			ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
			using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(cipherText)))
				using (CryptoStream cs = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
					using (StreamReader sr = new StreamReader(cs))
					{
						return sr.ReadToEnd();
					}
		}
	}
}