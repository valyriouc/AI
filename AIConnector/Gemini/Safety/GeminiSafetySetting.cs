using System.Text.Json.Serialization;

namespace AIConnector.Gemini;

public readonly struct GeminiSafetySetting(
    GeminiSafetyCategory category, 
    GeminiSafetyFiltering threshold)
{
    [JsonPropertyName("category")]
    public GeminiSafetyCategory Category { get; } = category;

    [JsonPropertyName("threshold")]
    public GeminiSafetyFiltering Threshold { get; } = threshold;
}
