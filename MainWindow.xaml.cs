using LoLHelper.Services;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace LoLHelper
{
    public partial class MainWindow : Window
    {
        private LCU _lcu;
        private AutoAcceptService _autoAcceptService;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            this.Opacity = 0;
            var fadeIn = new System.Windows.Media.Animation.DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(300)));
            this.BeginAnimation(Window.OpacityProperty, fadeIn);
            
            await EnsureLCUInitializedAsync();
            if (_lcu == null)
            {
                MessageBox.Show("Cannot get data. Make sure that the League of Legends client is running.", "Error");
                Application.Current.Shutdown();
                return;
            }

            try
            {
                var response = await _lcu.RequestAsync("/lol-chat/v1/me");
                dynamic userInfo = JsonConvert.DeserializeObject(response);
                GreetingLabel.Content = $"Hello, {userInfo.gameName}!\nWelcome to LoLHelper";
            }
            catch
            {
                GreetingLabel.Content = "Failed to get user info.";
            }
        }

        private async Task<(string authToken, string port)?> GetTokenAndPortAsync()
        {
            var (token, port) = LCUClient.GetClientDetails();
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(port))
            {
                return null;
            }
            return (token, port);
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

        private async void RemoveChallengesButton_Click(object sender, RoutedEventArgs e)
        {
            await EnsureLCUInitializedAsync();
            if (_lcu == null) return;

            string jsonContent = "{\"challengeIds\": []}";
            try
            {
                await _lcu.PostRequestAsync("/lol-challenges/v1/update-player-preferences/", jsonContent);
                MessageBox.Show("Challenges have been removed");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to remove challenges:\n{ex.Message}");
            }
        }

        private void GitHubLinkButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/kripistor/LoLHelper",
                UseShellExecute = true
            });
        }

        private void BannedInfoButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The app operates using the publicly available Riot Games API and built-in functionalities of the League client. It does not modify, interfere with, or give any gameplay advantages. All interactions are strictly limited to the client’s allowed commands and functionalities.");
        }
        private async void AutoAcceptCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Auto Accept Enabled");
            await EnsureLCUInitializedAsync();
            if (_lcu != null)
            {
                _autoAcceptService = new AutoAcceptService(_lcu);
                _autoAcceptService.Start();
            }
            else
            {
                MessageBox.Show("League Client not detected.");
                AutoAcceptCheckBox.IsChecked = false;
            }
        }
        
        private void AutoAcceptCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _autoAcceptService?.Stop();
        }

    }
}
