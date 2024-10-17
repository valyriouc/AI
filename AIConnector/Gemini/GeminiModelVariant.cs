namespace AIConnector.Gemini;

public enum GeminiModelVariant
{
    Flash15,
    Pro15,
    Pro10,
    Embedding,
    AQA
}

public enum GeminiMode
{
    ContentGeneration,
    ContentGenerationStream,
    Embed
}

internal static class EnumExtensions
{
    public static string AsString(this GeminiModelVariant self)
    {
        switch (self)
        {
            case GeminiModelVariant.Flash15:
                return "gemini-1.5-flash";
            case GeminiModelVariant.Pro15:
                return "gemini-1.5-pro";
            case GeminiModelVariant.Pro10:
                return "gemini-1.0-pro";
            case GeminiModelVariant.Embedding:
                return "text-embedding-004";
            case GeminiModelVariant.AQA:
                return "aqa";
        }
    }
}