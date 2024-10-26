using AIConnector.Common;

namespace AIConnector.Gemini;

public sealed class GeminiModel : IModel
{
    private HttpClient Client { get; }
    
    private Uri Url { get; }

    internal GeminiModel(GeminiModelBuilder builder)
    {
        Client = new();
        Url = builder.BuildUri();
    }

    public GeminiModel(Uri url)
    {
        Client = new();
        Url = url;
    }

    /// <summary>
    /// Asynchronously generates content using the Gemini model.
    /// </summary>
    /// <exception cref="GeminiApiException">Thrown when the Gemini API returns an error response.</exception>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated content as a string.</returns>
    public async Task<string> GenerateContentAsync()
    {
        using HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, Url);

        using HttpResponseMessage response = await Client.SendAsync(message);

        await response.ThrowOnGeminiErrorAsync();

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