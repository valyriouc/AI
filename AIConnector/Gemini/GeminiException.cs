using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace AIConnector.Gemini;

public class GeminiException(string message) : Exception(message);

public sealed class GeminiApiException(uint error, string status, string errorMessage)
    : Exception(BuildMessage(error, status, errorMessage))
{
    public uint Error { get; } = error;

    public string Status { get; } = status;

    public string ErrorMessage { get; } = errorMessage;

    private static string BuildMessage(uint error, string status, string errorMessage)
    {
        StringBuilder sb = new();
        sb.AppendLine($"Gemini send back error {error}");
        sb.AppendLine($"Status: {status}");
        sb.AppendLine($"Message: {errorMessage}");
        return sb.ToString();
    }

    public static async Task<GeminiApiException> FromResponseAsync(HttpResponseMessage response)
    {
        GeminiErrorWrapper? errorWrapper = await response.Content
            .ReadFromJsonAsync<GeminiErrorWrapper>();

        if (errorWrapper is null)
        {
            throw new GeminiException("Could not get error message from response!");
        }

        GeminiError error = errorWrapper.Value.Error;

        return new GeminiApiException(
            error.Error,
            error.Status,
            error.Message);
    }
}

[method: JsonConstructor]
file readonly struct GeminiErrorWrapper(GeminiError error)
{
    [JsonPropertyName("error")]
    public GeminiError Error { get; } = error;
}

[method: JsonConstructor]
file readonly struct GeminiError(uint error, string status, string message)
{
    [JsonPropertyName("error")]
    public uint Error { get; } = error;

    [JsonPropertyName("status")]
    public string Status { get; } = status;

    [JsonPropertyName("message")]
    public string Message { get; } = message;
}