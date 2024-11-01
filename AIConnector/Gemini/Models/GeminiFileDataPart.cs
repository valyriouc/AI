namespace AIConnector.Gemini;

public readonly struct GeminiFileDataPart(
    string mimeType, 
    string fileUri) : IGeminiRequestPart
{
    public string MimeType { get; } = mimeType;
    
    public string FileUri { get; } = fileUri;
}