namespace AIConnector.Gemini;
 
public readonly struct GeminiInlineDataPart 
    (string mimeType, string data) : IGeminiRequestPart
{
    public string MimeType { get; } = mimeType;

    public string Data { get; } = data;
}