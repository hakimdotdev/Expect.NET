namespace hExpect;

public interface IExpectation
{
    IExpectation Expect(string pattern, Action onMatched);
    IExpectation Send(string input);
    void Run();
}