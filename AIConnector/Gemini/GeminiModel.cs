using AIConnector.Common;

namespace AIConnector.Gemini;

public class GeminiModel : IModel
{
    private GeminiModelVariant Variant { get; }
    

    public GeminiModel(GeminiModelBuilder builder)
    {
        
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