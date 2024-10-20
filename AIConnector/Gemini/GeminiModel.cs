using AIConnector.Common;

namespace AIConnector.Gemini;

public class GeminiModel : IModel, IDisposable
{
    private HttpClient Client { get; }
    
    private Uri Url { get; }

    internal GeminiModel(GeminiModelBuilder builder)
    {
        Client = new();
        Url = builder.BuildUri();
    }
    
    public async Task<string> GenerateContentAsync()
    {
        using HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, Url);

        using HttpResponseMessage response = await Client.SendAsync(message);

        return await response.Content.ReadAsStringAsync();
    }

    public async Task<T> GenerateContentAsync<T>()
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {   
        this.Client.Dispose();
    }
}