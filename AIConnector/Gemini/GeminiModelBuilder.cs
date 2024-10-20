using System.Text;
using AIConnector.Common;

namespace AIConnector.Gemini;

public class GeminiModelBuilder : IModelBuilder
{
    private const string BASEURL = "https://generativelanguage.googleapis.com/v1beta/models/";

    public GeminiModelVariant Variant { get; private set; }
    
    public GeminiMode GeminiMode { get; private set; }

    public string? ApiKey { get; private set; }
    

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
        StringBuilder sb = new StringBuilder();

        sb.Append(BASEURL);

        if (this.Variant == null)
        {
            throw new ArgumentNullException(nameof(this.Variant));
        }

        sb.Append(this.Variant.AsString());
        
        if (this.GeminiMode == null)
        {
            throw new ArgumentNullException(nameof(this.GeminiMode));
        }

        sb.Append(":");
        sb.Append(this.GeminiMode.AsString());

        if (string.IsNullOrWhiteSpace(this.ApiKey))
        {
            throw new ArgumentNullException(nameof(this.ApiKey));
        }

        sb.Append($"?key={this.ApiKey}");
        
        return new Uri(sb.ToString());
    }

    internal HttpContent BuildContent()
    {
        
    }

    public IModel Build() =>
        new GeminiModel(this);
}