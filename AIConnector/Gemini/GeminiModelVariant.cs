namespace AIConnector.Gemini;

public enum GeminiModelVariant
{
    Flash15,
    Pro15,
    Pro10,
    Embedding,
    Aqa,
    None
}

public enum GeminiMode
{
    ContentGeneration,
    ContentGenerationStream,
    Embed,
    None
}

internal static class EnumExtensions
{
    public static string AsString(this GeminiModelVariant self)
    {
        return self switch
        {
            GeminiModelVariant.Flash15 => "gemini-1.5-flash",
            GeminiModelVariant.Pro15 => "gemini-1.5-pro",
            GeminiModelVariant.Pro10 => "gemini-1.0-pro",
            GeminiModelVariant.Embedding => "text-embedding-004",
            GeminiModelVariant.Aqa => "aqa",
            _ => throw new NotSupportedException()
        };
    }

    public static string AsString(this GeminiMode self)
    {
        return self switch
        {
            GeminiMode.ContentGeneration => "generateContent",
            GeminiMode.ContentGenerationStream => "streamGenerateContent",
            GeminiMode.Embed => "embedContent",
            _ => throw new NotSupportedException()
        };
    }
}