namespace AIConnector.Gemini;

public readonly struct GeminiFunctionCallPart(
    string name, 
    GeminiFunctionCallArgs args) : IGeminiRequestPart
{
    public string Name { get; } = name;
    
    public GeminiFunctionCallArgs Args { get; } = args;
}

public readonly struct GeminiFunctionCallArgs
{
    
}