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
            _listener.Prefixes.Add(App.Configuration!["API:callback-url"]!);
            _listener.Start();
            
            Console.WriteLine("Listener started on https://localhost:8080/callback/");

            string authorizationCode;

            while (true)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    var query = context.Request.Url!.Query;
                    authorizationCode = GetQueryValue(query, "code");
                    Console.WriteLine(authorizationCode);

                    if (!string.IsNullOrEmpty(authorizationCode))
                    {
                        var response = context.Response;
                        string responseString = "<HTML><BODY><h2>Authorization was successfully completed. You can close this window.</h2></BODY></HTML>";
                        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                        response.ContentLength64 = buffer.Length;
                        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                        response.Close();
                        break; 
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            _listener.Stop(); 
            return authorizationCode!;
        }

        private string GetQueryValue(string query, string key)
        {
            var parameters = System.Web.HttpUtility.ParseQueryString(query);
            return parameters[key]!;
        }
    }
}
