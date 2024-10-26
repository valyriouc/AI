using AIConnector.Gemini;

namespace AIConnector.Common;

internal static class HttpResponseExtensions
{
    public static async Task ThrowOnGeminiErrorAsync(this HttpResponseMessage self)
    {
        if (!self.IsSuccessStatusCode)
        {
            throw await GeminiApiException.FromResponseAsync(self);
        }
    }
}