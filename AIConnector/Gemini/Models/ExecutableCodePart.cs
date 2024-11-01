namespace AIConnector.Gemini;

public readonly struct ExecutableCodePart(
    GeminiExeCodeLanguage language, 
    string code) : IGeminiRequestPart
{
    public GeminiExeCodeLanguage Language { get; } = language;

    public string Code { get; } = code;
}

public enum GeminiExeCodeLanguage
{
    Unspecified,
    Python    
}

internal static class GeminiExeCodeLanguageExtensions
{
    public static string AsString(this GeminiExeCodeLanguage self)
    {
        return self switch
        {
            GeminiExeCodeLanguage.Unspecified => "LANGUAGE_UNSPECIFIED",
            GeminiExeCodeLanguage.Python => "PYTHON",
            _ => throw new NotImplementedException()
        };
    }
}