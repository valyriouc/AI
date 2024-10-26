namespace AIConnector.Common;

public interface IModel : IDisposable
{
    public Task<string> GenerateContentAsync();

    public Task<T> GenerateContentAsync<T>();

    public IAsyncEnumerable<T> StreamContentAsync<T>();
}
