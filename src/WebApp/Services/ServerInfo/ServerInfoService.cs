using System.Diagnostics;

namespace WebApp.Services.ServerInfo;

public class ServerInfoService
{
    public string Execute(string command, string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = command,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        if (!string.IsNullOrWhiteSpace(arguments))
            startInfo.Arguments = arguments;

        using var process = Process.Start(startInfo);

        if (process is null)
            return "Не удалось запустить команду.";

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (!string.IsNullOrWhiteSpace(error))
            return output + Environment.NewLine + error;

        return output;
    }

    public Dictionary<string, string> GetServerInfo()
    {
        return new Dictionary<string, string>
        {
            ["ls"] = Execute("ls", "-la"),
            ["ps"] = Execute("ps", "aux"),
            ["whoami"] = Execute("whoami", ""),
            ["id"] = Execute("id", ""),
            ["uname"] = Execute("uname", "-a")
        };
    }
}