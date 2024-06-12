using System.Diagnostics;

namespace hExpect;

public interface ICommandRunner
{
    Task StartAsync();
    Task SendAsync(string input);
    event DataReceivedEventHandler OutputReceived;
    event DataReceivedEventHandler ErrorReceived;
}