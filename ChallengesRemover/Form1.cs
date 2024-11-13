using ChallengesRemover.Services;
using Newtonsoft.Json;
namespace ChallengesRemover
{
    public partial class Form1 : Form
    {
        private LCU _lcu;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Challenge Remover";
            this.ShowIcon = true;
            this.Icon = new Icon("app.ico");
        }

        private async Task<(string authToken, string port)?> GetTokenAndPortAsync()
        {
            var (authToken, port) = LCUClient.GetClientDetails();
            if (string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(port))
            {
                MessageBox.Show("Cannot get data. Make sure that the League of Legends client is running.", "Error");
                return null;
            }
            return (authToken, port);
        }
        private async Task EnsureLCUInitializedAsync()
        {
            if (_lcu == null)
            {
                var credentials = await GetTokenAndPortAsync();
                if (credentials != null)
                {
                    _lcu = new LCU(credentials.Value.authToken, credentials.Value.port);
                }
            }
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            await EnsureLCUInitializedAsync();
            if (_lcu == null) return;

            var response = await _lcu.RequestAsync("/lol-chat/v1/me");
            var user_info = JsonConvert.DeserializeObject<dynamic>(response);
            GreetingLabel.Text = $"Hello, {user_info.gameName}! \n Welcome to Challenge Remover";
            GreetingLabel.Location = new Point((this.ClientSize.Width - GreetingLabel.Width) / 2, 9);
        }


        private async void removeChallengesButton_Click(object sender, EventArgs e)
        {
            await EnsureLCUInitializedAsync();
            if (_lcu == null) return;
            string jsonContent = "{\"challengeIds\": []}";
            await _lcu.PostRequestAsync("/lol-challenges/v1/update-player-preferences/", jsonContent);
            MessageBox.Show("Challenges have been removed");

        }

        private void GreetingLabel_Click(object sender, EventArgs e)
        {
        }

        private void GitHubLinkButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The browser will now open.");

            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https://github.com/kripistor/ChallengesRemover",
                UseShellExecute = true
            };
            System.Diagnostics.Process.Start(psi);
        }

        private void BannedInfoButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The app operates using the publicly available Riot Games API and built-in functionalities of the League client. It does not modify, interfere with, or give any gameplay advantages. All interactions are strictly limited to the client�s allowed commands and functionalities.");
        }
    }
}
