namespace Ollama;

public interface ITool
{
    public string GetDescription();

    public Task<string> ExecuteAsync(string input);
}