namespace AIConnector.Gemini;

public sealed class GeminiRequestBuilder
{
    private List<IGeminiRequestPart> parts = new();
    private string? role = null;
    
    private List<GeminiSafetySetting> safetySettings = new();
    private List<string> stopSequences = new();

    // https://ai.google.dev/api/generate-content#v1beta.GenerationConfig
    private string? responseMimeType;
    private int? candiateCount = null;
    private int? maxOutputTokens = null;
    private double? temperature = null;
    private double? topP = null;
    private int? topK = null;
    private double? presencePenalty = null;
    private double? frequencyPenalty = null;
    private bool? responseLogprobs = null;
    private int? logProbs = null;

    public GeminiRequestBuilder WithRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
        {
            throw new ArgumentNullException(nameof(role));
        }

        this.role = role;
        return this;
    }
    
    public GeminiRequestBuilder AddPart(IGeminiRequestPart part)
    {
        this.parts.Add(part);
        return this;
    }
    
    public GeminiRequestBuilder WithTopP(double value)
    {
        this.topP = value;
        return this;
    }
    
    public GeminiRequestBuilder WithTopK(int value)
    {
        this.topK = value;
        return this;
    }

    public GeminiRequestBuilder WithPresencePenalty(double? value)
    {
        this.presencePenalty = value;
        return this;
    }
    
    public GeminiRequestBuilder WithFrequencyPenalty(double value)
    {
        this.frequencyPenalty = value;
        return this;
    }
    
    public GeminiRequestBuilder UseLogProbs()
    {
        responseLogprobs = true;
        return this;
    }
    
    public GeminiRequestBuilder WithLogProbs(int value)
    {
        if (this.responseLogprobs is null or false)
        {
            throw new GeminiException("Enable response log probs first!");
        }
        
        this.logProbs = value;
        return this;
    }
    
    public GeminiRequestBuilder WithTemperature(double value)
    {
        if (value < 0.0 || value > 2.0)
        {
            throw new GeminiException("Temperature must be between 0.0 and 2.0.");
        }

        this.temperature = value;
        return this;
    }
    
    public GeminiRequestBuilder WithMaxOutputTokens(int value)
    {
        this.maxOutputTokens = value;
        return this;
    }
    
    public GeminiRequestBuilder WithResponseMimeType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        this.responseMimeType = value;
        return this;
    }
    
    public GeminiRequestBuilder WithStopSequence(string sequence)
    {
        if (string.IsNullOrWhiteSpace(sequence))
        {
            throw new ArgumentNullException(nameof(sequence));  
        }

        if (stopSequences.Count == 5)
        {
            throw new GeminiException("Gemini models has a limit of 5 stop sequences.");
        }
        
        stopSequences.Add(sequence);
        return this;
    }

    public GeminiRequestBuilder WithStopSequence(List<string> sequences)
    {
        foreach (string sequence in sequences)
        {
            _ = WithStopSequence(sequence);
        }
        
        return this;
    }

    public GeminiRequest Build()
    {
        return new GeminiRequest();
    }
    
    #region Quick create

    public static GeminiRequest Create(string question)
    {
        var builder = new GeminiRequestBuilder();

        builder.AddPart(new GeminiTextPart(question));
        return builder.Build();
    }

    public static GeminiRequest Create(List<IGeminiRequestPart> parts)
    {
        var builder = new GeminiRequestBuilder();
        
        foreach (var part in parts)
        {
            builder.AddPart(part);
        }

        return builder.Build();
    }
    
    #endregion
}