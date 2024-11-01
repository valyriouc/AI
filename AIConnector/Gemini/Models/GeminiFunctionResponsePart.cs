namespace AIConnector.Gemini;

public readonly struct GeminiFunctionResponsePart(
    string name, 
    GeminiFunctionResponse response) : IGeminiRequestPart
{
    public string Name { get; } = name;

    public GeminiFunctionResponse Response { get; } = response;
}

public readonly struct GeminiFunctionResponse
{
    
}

