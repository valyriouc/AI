using System.Text.Json.Serialization;

namespace AIConnector.Gemini;

public readonly struct GeminiTextPart(string text) : IGeminiRequestPart
{
    [JsonPropertyName("text")]
    public string Text { get; } = text;
}