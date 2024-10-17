namespace AIConnector.Common;

public interface IModel
{
    public Task<string> GenerateContentAsync();

    public Task<T> GenerateContentAsync<T>();
    
}
