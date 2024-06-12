using System.Diagnostics;

namespace hExpect;

public class ProcessCommandRunner : ICommandRunner
{
    private readonly Process _process;

    public ProcessCommandRunner(string path, string arguments)
    {
        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = path,
                Arguments = arguments,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };

        _process.OutputDataReceived += (sender, e) => OutputReceived?.Invoke(sender, e);
        _process.ErrorDataReceived += (sender, e) => ErrorReceived?.Invoke(sender, e);
    }

    public event DataReceivedEventHandler? OutputReceived;
    public event DataReceivedEventHandler? ErrorReceived;

    public Task StartAsync()
    {
        _process.Start();
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();
        return Task.CompletedTask;
    }

    public Task SendAsync(string input)
    {
        return _process.StandardInput.WriteLineAsync(input);
    }
}