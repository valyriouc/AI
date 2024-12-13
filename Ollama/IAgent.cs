namespace Ollama;

public interface IAgent
{
    public Task ThinkAsync(CancellationToken cancellationToken);

    public Task<string> WorkAsync(string prompt, CancellationToken cancellationToken);
}