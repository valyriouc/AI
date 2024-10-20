using AIConnector.Common;

namespace AIConnector.Gemini;

public class GeminiModelBuilder : IModelBuilder
{
    private const string BASEURL = "https://generativelanguage.googleapis.com/v1beta/models/";

    public GeminiModelVariant Variant { get; private set; }
    
    public GeminiMode GeminiMode { get; private set; }

    public string ApiKey { get; private set; }

    public GeminiModelBuilder WithApiKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        ApiKey = key;
        return this;
    }

    public GeminiModelBuilder WithModelVariant(GeminiModelVariant variant)
    {
        this.Variant = variant;
        return this;
    }

    public GeminiModelBuilder WithModelMode(GeminiMode geminiMode)
    {
        this.GeminiMode = geminiMode;
        return this;
    }

    internal Uri BuildUri()
    {
        return new Uri("");
    }
    
    public IModel Build()
    {
        throw new NotImplementedException();
    }
}