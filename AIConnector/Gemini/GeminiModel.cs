using AIConnector.Common;

namespace AIConnector.Gemini;

public class GeminiModel : IModel
{
    private Uri Url { get; }

    public GeminiModel(GeminiModelBuilder builder)
    {
        Url = builder.BuildUri();
        
    }
    
    public Task<string> GenerateContentAsync()
    {
        throw new NotImplementedException();
    }

    public Task<T> GenerateContentAsync<T>()
    {
        throw new NotImplementedException();
    }
}