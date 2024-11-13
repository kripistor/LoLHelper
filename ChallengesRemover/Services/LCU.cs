using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengesRemover.Services
{
    public class LCU
    {
        public bool IsConnected { get; private set; } = false;
        private string credentialsAuth;
        private string credentialsPort;
        private HttpClient httpClient;

        public LCU(string authToken, string port)
        {
            credentialsAuth = authToken;
            credentialsPort = port;
            httpClient = new HttpClient();
            Connect();
        }

        private void Connect()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };

            httpClient = new HttpClient(handler);

            var byteArray = System.Text.Encoding.ASCII.GetBytes("riot:" + credentialsAuth);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            IsConnected = true;
        }

        public async Task<string> RequestAsync(string uri)
        {
            if (!IsConnected) return "ERR: Not Connected to LCU";

            if (uri[0] != '/') uri = '/' + uri;
            var fullUri = $"https://127.0.0.1:{credentialsPort}{uri}";
            var response = await httpClient.GetAsync(fullUri);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> PostRequestAsync(string uri, string jsonContent)
        {
            if (!IsConnected) return "ERR: Not Connected to LCU";

            if (uri[0] != '/') uri = '/' + uri;
            var fullUri = $"https://127.0.0.1:{credentialsPort}{uri}";

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(fullUri, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
