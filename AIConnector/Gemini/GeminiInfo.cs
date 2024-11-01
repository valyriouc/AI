using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace AIConnector.Gemini;

public static class GeminiInfo
{
    public static async IAsyncEnumerable<GeminiMetadata> GetModelsAsync(
        string apiKey,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new GeminiException("Api key is required.");
        }

        StringBuilder sb = new StringBuilder(GeminiConstants.ModelBaseUrl);
        sb.Append($"?key={apiKey}");
        
        using HttpRequestMessage request = new HttpRequestMessage(
            HttpMethod.Get, 
            sb.ToString());
        
        using HttpClient client = new HttpClient();
        using HttpResponseMessage response = await client.SendAsync(
            request, 
            cancellationToken);
        
        GeminiPagedResult result = await response.Content
            .ReadFromJsonAsync<GeminiPagedResult>(
                cancellationToken: cancellationToken);

        foreach (GeminiMetadata model in result.Models)
        {
            yield return model;
        }
    }

    public static async Task<GeminiMetadata> GetModelMetadataAsync(
        string apiKey, 
        string modelName,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new GeminiException("Api key is required.");
        }

        if (string.IsNullOrWhiteSpace(modelName))
        {
            throw new GeminiException("Model name is required.");
        }

        StringBuilder sb = new StringBuilder(GeminiConstants.ModelBaseUrl);
        
        sb.Append($"{modelName}?key={apiKey}");
        
        using HttpRequestMessage request = new HttpRequestMessage(
            HttpMethod.Get, 
            sb.ToString());
        
        using HttpClient client = new HttpClient();
        using HttpResponseMessage response = await client.SendAsync(
            request, 
            cancellationToken);
        
        return await response.Content.ReadFromJsonAsync<GeminiMetadata>(
            cancellationToken: cancellationToken);
    }
}

[method: JsonConstructor]
internal readonly struct GeminiPagedResult(
    List<GeminiMetadata> models,
    string nextPageToken)
{
    public List<GeminiMetadata> Models { get; } = models;

    public string NextPageToken { get; } = nextPageToken;
}

[method: JsonConstructor]
public readonly struct GeminiMetadata(
    string name,
    string baseModelId,
    string version,
    string displayName,
    string description,
    int inputTokenLimit,
    int outputTokenLimit,
    List<string> supportedGenerationMethods,
    double temperature,
    double maxTemperature,
    double topP,
    int topK)
{
    public string Name { get; } = name;

    public string BaseModelId { get; } = baseModelId;

    public string Version { get; } = version;

    public string DisplayName { get; } = displayName;

    public string Description { get; } = description;

    public int InputTokenLimit { get; } = inputTokenLimit;
    
    public int OutputTokenLimit { get; } = outputTokenLimit;

    public List<string> SupportedGenerationMethods { get; } = supportedGenerationMethods;

    public double Temperature { get; } = temperature;

    public double MaxTemperature { get; } = maxTemperature;

    public double TopP { get; } = topP;

    public int TopK { get; } = topK;
}