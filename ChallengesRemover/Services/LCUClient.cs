using System;

namespace ChallengesRemover.Services
{
    public static class LCUClient
    {
        public static (string token, string port) GetClientDetails()
        {
            string commandLine = string.Empty;
            string token = string.Empty;
            string port = string.Empty;

            var searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE Name = 'LeagueClientUx.exe'");
            foreach (System.Management.ManagementObject process in searcher.Get())
            {
                commandLine = process["CommandLine"]?.ToString();
                if (!string.IsNullOrEmpty(commandLine))
                {
                    token = ExtractValue(commandLine, "--remoting-auth-token=");
                    port = ExtractValue(commandLine, "--app-port=");
                }
            }

            return (token, port);
        }

        private static string ExtractValue(string commandLine, string flag)
        {
            int startIndex = commandLine.IndexOf(flag) + flag.Length;
            int endIndex = commandLine.IndexOf(" ", startIndex);
            if (endIndex == -1) endIndex = commandLine.Length;


            string value = commandLine.Substring(startIndex, endIndex - startIndex);
            value = value.Trim('"');
            return value;
        }
    }
}
