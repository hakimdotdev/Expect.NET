using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace hExpect;

public class Expectation : IExpectation
{
    private readonly ICommandRunner _commandRunner;
    private readonly List<(Regex pattern, Action onMatched)> _expectations = new();
    private readonly StringBuilder _outputBuffer = new();

    public Expectation(ICommandRunner commandRunner)
    {
        _commandRunner = commandRunner;
        _commandRunner.OutputReceived += CommandRunner_OutputReceived;
        _commandRunner.ErrorReceived += CommandRunner_OutputReceived;
    }

    private void CommandRunner_OutputReceived(object sender, DataReceivedEventArgs e)
    {
        if (e.Data == null)
        {
            return;
        }

        _outputBuffer.AppendLine(e.Data);

        foreach (var (pattern, onMatched) in _expectations)
        {
            if (!pattern.IsMatch(_outputBuffer.ToString())) continue;
            onMatched();
            _outputBuffer.Clear();
        }
    }

    public IExpectation Expect(string pattern, Action onMatched)
    {
        _expectations.Add((new Regex(pattern), onMatched));
        return this;
    }

    public IExpectation Send(string input)
    {
        _commandRunner.SendAsync(input).Wait();
        return this;
    }

    public void Run()
    {
        _commandRunner.StartAsync().Wait();
    }
}