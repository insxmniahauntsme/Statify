using System.IO;
using System.Net;
using System.Threading.Channels;
using Statify.Models;

namespace Statify.Servers
{
    public class AccessTokenServer
    {
        private readonly HttpListener _listener;

        public AccessTokenServer(HttpListener listener)
        {
            _listener = listener;
        }

        public async Task<AuthorizationData> StartAsync(CancellationToken token)
        {
            _listener.Prefixes.Add("https://localhost:8081/");
            if (!_listener.IsListening)
            {
                _listener.Start();
                Console.WriteLine("Listening for connections on port 8081");
            }
            else
            {
                Console.WriteLine("Already listening");
            }

            while (!token.IsCancellationRequested)
            {
                var context = await _listener.GetContextAsync();
                var request = context.Request;
                var response = context.Response;

                try
                { 
                    if (request.HttpMethod == "POST")
                    {
                        using (var reader = new StreamReader(request.InputStream))
                        {
                            var formData = await reader.ReadToEndAsync();
                            var parsedData = ParseFormData(formData);
                            
                            var authData = new AuthorizationData
                            {
                                AccessToken = parsedData["access_token"],
                                TokenType = parsedData["token_type"],
                                Scope = parsedData["scope"],
                                ExpiresIn = int.TryParse(parsedData["expires_in"], out var expiresIn) ? expiresIn : 0,
                                RefreshToken = parsedData["refresh_token"]
                            };

                            Console.WriteLine($"Access Token: {authData.AccessToken}");
                            Console.WriteLine($"Token Type: {authData.TokenType}");
                            Console.WriteLine($"Scope: {authData.Scope}");
                            Console.WriteLine($"Expires In: {authData.ExpiresIn}");
                            Console.WriteLine($"Refresh Token: {authData.RefreshToken}");

                            response.StatusCode = 200;
                            using (var writer = new StreamWriter(response.OutputStream))
                            {
                                await writer.WriteAsync($"Data received successfully from {request.Url}");
                            }

                            return authData;
                        }
                    }
                    else
                    {
                        response.StatusCode = 400;
                        using (var writer = new StreamWriter(response.OutputStream))
                        {
                            await writer.WriteAsync("Error: Invalid HTTP method. Only POST is allowed.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    using (var writer = new StreamWriter(response.OutputStream))
                    {
                        await writer.WriteAsync($"Error: {ex.Message}");
                    }

                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    response.OutputStream.Close();
                }
                
            }
            _listener.Stop();
            return new AuthorizationData();
        }
        
        private Dictionary<string, string> ParseFormData(string formData)
        {
            var parsedData = new Dictionary<string, string>();

            var pairs = formData.Split('&');
            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    var key = WebUtility.UrlDecode(keyValue[0]);
                    var value = WebUtility.UrlDecode(keyValue[1]);
                    parsedData[key] = value;
                }
            }

            return parsedData;
        }
        
    }
}
