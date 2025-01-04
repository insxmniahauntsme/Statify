using System.IO;
using System.Net;

namespace Statify.Servers
{
    public class CallbackServer
    {
        private readonly HttpListener _listener;

        public CallbackServer(HttpListener listener)
        {
            _listener = listener;
        }

        public async Task<string> StartAsync(CancellationToken token)
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
                    var accessToken = request.QueryString["access_token"];

                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        Console.WriteLine($"Received token: {accessToken}");

                        response.StatusCode = 200;

                        using (var writer = new StreamWriter(response.OutputStream))
                        {
                            await writer.WriteAsync("Access token received");
                        }

                        return accessToken;
                    }

                    response.StatusCode = 400;

                    using (var writer = new StreamWriter(response.OutputStream))
                    {
                        await writer.WriteAsync("Missing access code.");
                    }
                }
                catch (Exception ex)
                {
                    response.StatusCode = 500;
                    using (var writer = new StreamWriter(response.OutputStream))
                    {
                        await writer.WriteAsync($"Error: {ex.Message}");
                    }
                }
                finally
                {
                    response.OutputStream.Close();
                }
                
            }
            _listener.Stop();
            return string.Empty;
        }
        
    }
}
