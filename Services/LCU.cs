using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LoLHelper.Services
{
    public class LCU
    {
        public bool IsConnected { get; private set; } = false;
        private readonly string credentialsAuth;
        private readonly string credentialsPort;
        private readonly HttpClient httpClient;

        public LCU(string authToken, string port)
        {
            credentialsAuth = authToken;
            credentialsPort = port;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = 
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            httpClient = new HttpClient(handler);

            var byteArray = Encoding.ASCII.GetBytes("riot:" + credentialsAuth);
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            IsConnected = true;
        }

        public async Task<string> RequestAsync(string uri)
        {
            if (!IsConnected) return "ERR: Not Connected to LCU";

            if (!uri.StartsWith("/")) uri = "/" + uri;
            var fullUri = $"https://127.0.0.1:{credentialsPort}{uri}";
            var response = await httpClient.GetAsync(fullUri);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostRequestAsync(string uri, string jsonContent)
        {
            if (!IsConnected) return "ERR: Not Connected to LCU";

            if (!uri.StartsWith("/")) uri = "/" + uri;
            var fullUri = $"https://127.0.0.1:{credentialsPort}{uri}";

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(fullUri, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
