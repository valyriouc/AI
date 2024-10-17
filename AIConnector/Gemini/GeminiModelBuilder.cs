using AIConnector.Common;

namespace AIConnector.Gemini;


public class GeminiModelBuilder : IModelBuilder
{
    private const string BASEURL = "https://generativelanguage.googleapis.com/v1beta/models/";

    public GeminiModelVariant Variant { get; private set; }
    
    public GeminiMode GeminiMode { get; private set; }
    
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
        return 
    }
    public IModel Build()
    {
        throw new NotImplementedException();
    }
}