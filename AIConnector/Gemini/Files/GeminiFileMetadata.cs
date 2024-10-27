using System.Text.Json.Serialization;
using AIConnector.Common;

namespace AIConnector.Gemini.Files;

[method: JsonConstructor]
public readonly struct GeminiFileMetadata(
    string name,
    string displayName,
    string mimeType,
    string sizeInBytes,
    DateTime createTime,
    DateTime updateTime,
    DateTime expirationTime,
    string sha256Hash,
    string uri,
    string state)
{
    [JsonPropertyName("name")]
    public string Name { get; } = name;
    
    [JsonPropertyName("displayName")]
    public string DisplayName { get; } = displayName;

    [JsonPropertyName("mimeType")]
    public string MimeType { get; } = mimeType;

    [JsonPropertyName("sizeInBytes")]
    public string SizeInBytes { get; } = sizeInBytes;

    [JsonPropertyName("createTime")]
    public DateTime CreateTime { get; } = createTime;

    [JsonPropertyName("updateTime")]
    public DateTime UpdateTime { get; } = updateTime;

    [JsonPropertyName("expirationTime")]
    public DateTime ExpirationTime { get; } = expirationTime;

    [JsonPropertyName("sha256Hash")]
    public string Sha256Hash { get; } = sha256Hash;

    [JsonPropertyName("uri")]
    public string Uri { get; } = uri;
    
    [JsonPropertyName("state")]
    public string State { get; } = state;
}