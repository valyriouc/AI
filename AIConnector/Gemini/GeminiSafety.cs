namespace AIConnector.Gemini;

public enum GeminiSafetyCategory
{
    Harassment,
    HateSpeech,
    SexuallyExplicit,
    DangerousContent,
    CivicIntegrity
}

internal static class GeminiSafetyCategoryExtensions
{
    public static string AsString(this GeminiSafetyCategory self)
    {
        return self switch
        {
            GeminiSafetyCategory.Harassment => "HARM_CATEGORY_HARASSMENT",
            GeminiSafetyCategory.HateSpeech => "HATE_CATEGORY_HATE_SPEECH",
            GeminiSafetyCategory.SexuallyExplicit => "HARM_CATEGORY_SEXUALLY_EXPLICIT",
            GeminiSafetyCategory.DangerousContent => "HARM_CATEGORY_DANGEROUS_CONTENT",
            GeminiSafetyCategory.CivicIntegrity => "HARM_CATEGORY_CIVIC_INTEGRITY",
            _ => throw new NotImplementedException()
        };
    }
}

public enum GeminiSafetyFiltering
{
    Unclear,
    None,
    High, 
    Medium,
    Low
}

internal static class GeminiSafetyFilteringExtensions
{
    public static string AsString(this GeminiSafetyFiltering self)
    {
        return self switch
        {
            GeminiSafetyFiltering.Unclear => "HARM_BLOCK_THRESHOLD_UNSPECIFIED",
            GeminiSafetyFiltering.None => "BLOCK_NONE",
            GeminiSafetyFiltering.High => "BLOCK_ONLY_HIGH",
            GeminiSafetyFiltering.Medium => "BLOCK_MEDIUM_AND_ABOVE",
            GeminiSafetyFiltering.Low => "BLOCK_LOW_AND_ABOVE",
            _ => throw new NotImplementedException()
        };
    }
}