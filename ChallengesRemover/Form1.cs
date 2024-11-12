using ChallengesRemover.Services;
using Newtonsoft.Json;
namespace ChallengesRemover
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            var (authToken, port) = LCUClient.GetClientDetails();
            if (string.IsNullOrEmpty(authToken) || string.IsNullOrEmpty(port))
            {
                MessageBox.Show("Не удалось получить токен или порт. Убедитесь, что клиент League of Legends запущен.", "Ошибка");
                return;
            }
            var lcu = new LCU(authToken, port);
            var response = await lcu.RequestAsync("/lol-chat/v1/me");
            var user_info = JsonConvert.DeserializeObject<dynamic>(response);
            GreetingLabel.Text = $"Hello, {user_info.gameName}! \n Welcome to Challenge Remover";
            GreetingLabel.Location = new Point((this.ClientSize.Width - GreetingLabel.Width) / 2,
                                   9);



        }


        private void removeChallengesButton_Click(object sender, EventArgs e)
        {

        }

        private void GreetingLabel_Click(object sender, EventArgs e)
        {
        }

    }
}
