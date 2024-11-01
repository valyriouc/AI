using System.Text;
using AIConnector.Common;

namespace AIConnector.Gemini;

public class GeminiModelBuilder : IModelBuilder
{ 
    private GeminiModelVariant _variant = GeminiModelVariant.None;

    private GeminiMode _geminiMode = GeminiMode.None;

    private string? _apiKey;
    
    public GeminiModelBuilder WithApiKey(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        this._apiKey = key;
        return this;
    }

    public GeminiModelBuilder WithModelVariant(GeminiModelVariant variant)
    {
        this._variant = variant;
        return this;
    }

    public GeminiModelBuilder WithModelMode(GeminiMode geminiMode)
    {
        this._geminiMode = geminiMode;
        return this;
    }

    internal Uri BuildUri()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(GeminiConstants.ModelBaseUrl);
        
        if (this._variant == GeminiModelVariant.None)
        {
            throw new InvalidOperationException(
                "The model variant is required in order to use gemini models.");
        }
        
        sb.Append(this._variant.AsString());
        
        if (this._geminiMode == GeminiMode.None)
        {
            throw new InvalidOperationException(
                "The gemini mode is required in order to use gemini models.");
        }
        
        sb.Append(":");
        sb.Append(this._geminiMode.AsString());

        if (string.IsNullOrWhiteSpace(this._apiKey))
        {
            throw new InvalidOperationException(
                "The api key is required in order to use gemini models.");
        }

        sb.Append($"?key={this._apiKey}");
        
        return new Uri(sb.ToString());
    }

    public IModel Build() =>
        new GeminiModel(this);
}