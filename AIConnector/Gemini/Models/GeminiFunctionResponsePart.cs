namespace AIConnector.Gemini;

public readonly struct GeminiFunctionResponsePart(string name) : IGeminiRequestPart
{
    public string Name { get; } = name;

    // TODO: Impl. response struct 
}