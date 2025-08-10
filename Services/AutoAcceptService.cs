using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Threading;

namespace LoLHelper.Services
{
    public class AutoAcceptService
    {
        private readonly LCU _lcu;
        private bool _isRunning;

        public AutoAcceptService(LCU lcu)
        {
            _lcu = lcu;
        }

        public void Start()
        {
            if (_isRunning) return;
            _isRunning = true;
            Task.Run(RunAsync);
        }

        public void Stop()
        {
            _isRunning = false;
        }

        private async Task RunAsync()
        {
            while (_isRunning)
            {
                try
                {
                    var response = await _lcu.RequestAsync("/lol-gameflow/v1/session");
                    Console.WriteLine($"Session response: {response}");
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        await Task.Delay(1000);
                        continue;
                    }

                    // Попытка парсинга вручную, как у того чела
                    string phase = "";
                    try
                    {
                        phase = response.Split("phase").Last().Split('"')[2];
                    }
                    catch
                    {
                        // fallback - если не получилось
                        var sessionObj = JsonConvert.DeserializeObject<dynamic>(response);
                        phase = sessionObj?.phase;
                    }

                    Console.WriteLine($"Current phase: {phase}");

                    if (phase == "ReadyCheck")
                    {
                        Console.WriteLine("Accepting match...");
                        await _lcu.PostRequestAsync("/lol-matchmaking/v1/ready-check/accept", "{}");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"AutoAccept error: {ex.Message}");
                }
                await Task.Delay(1000);
            }
        }
    }
}
