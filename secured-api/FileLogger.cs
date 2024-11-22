namespace SecuredApi;

using System.IO;
using System.Threading.Tasks;

public static class FileLogger
{
    public const string LogFilePath = @"C:\logs\authentication_log.txt";

    public static async Task LogAsync(string message)
    {
        // Check if the log file exists, create if it does not
        if (!File.Exists(LogFilePath))
        {
            using (var stream = File.Create(LogFilePath))
            {
                // Optionally write a header or initial message to the log file
                await using (var writer = new StreamWriter(stream))
                {
                    await writer.WriteLineAsync($"Log File Created{Environment.NewLine}");
                }
            }
        }

        await File.AppendAllTextAsync(LogFilePath, $"=================================== {DateTime.UtcNow.ToShortTimeString()} ==================================={Environment.NewLine}{message}{Environment.NewLine}{Environment.NewLine}");
    }
}
