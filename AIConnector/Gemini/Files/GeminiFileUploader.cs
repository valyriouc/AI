using System.Net.Http.Headers;
using System.Text;
using AIConnector.Common;

namespace AIConnector.Gemini.Files;

public sealed class GeminiFileUploader(
    string apiKey) : IDisposable
{
    private const string UploadUrl = "https://generativelanguage.googleapis.com/upload/v1beta/files/";
    private const string FileUrl = "https://generativelanguage.googleapis.com/v1beta/files/";

    private readonly HttpClient httpClient = new();

    public async Task UploadFileAsync(GeminiFile file, CancellationToken cancellationToken)
    {
        using HttpRequestMessage initRequest = CreateInitUploadRequest(file);
        
        using HttpResponseMessage response = await httpClient.SendAsync(initRequest, cancellationToken);
        
        await response.ThrowOnGeminiErrorAsync();

        string uploadUrl = response.Headers.GetValues("x-goog-upload-url").First();

        using HttpRequestMessage finalizeRequest = CreateFinalizeUploadRequest(uploadUrl, file);
        
        using HttpResponseMessage finalize = await httpClient.SendAsync(finalizeRequest, cancellationToken);
        
        await response.ThrowOnGeminiErrorAsync();
    }

    private HttpRequestMessage CreateInitUploadRequest(GeminiFile file)
    {
        StringBuilder sb = new StringBuilder(UploadUrl);
        sb.Append($"?key={apiKey}");
        
        HttpRequestMessage request = new HttpRequestMessage(
            HttpMethod.Get, 
            sb.ToString());
        
        request.Headers.Add("X-Goog-Upload-Protocol", "resumable");
        request.Headers.Add("X-Goog-Upload-Command", "start");
        request.Headers.Add("X-Goog-Upload-Header-Content-Length", $"{file.FileSize}");
        request.Headers.Add("X-Goog-Upload-Header-Content-Type", file.MimeType.AsString());
        
        string json = 
            $$"""
            {
              "file": 
              {
                "display_name": {{file.FileName}}
              }  
            }
            """;

        request.Content = new StringContent(json);
        request.Content!.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        return request;
    }

    private HttpRequestMessage CreateFinalizeUploadRequest(string baseUrl, GeminiFile file)
    {
        HttpRequestMessage message = new HttpRequestMessage(
            HttpMethod.Get, 
            $"{baseUrl}?key={apiKey}");
        
        message.Headers.Add("Content-Length", file.FileSize.ToString());
        message.Headers.Add("X-Goog-Upload-Offset", "0");
        message.Headers.Add("X-Goog-Upload-Command", "upload, finalize");
        
        message.Content = new ByteArrayContent(file.FileBytes.ToArray());
        return message;
    }

    public async Task<string> GetFileMetadataAsync(string filename, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await httpClient.GetAsync(
            $"{FileUrl}?key={apiKey}", 
            cancellationToken);

        await response.ThrowOnGeminiErrorAsync();
        
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task<string> ListFilesAsync(CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await httpClient.GetAsync(
            $"{FileUrl}?key={apiKey}",
            cancellationToken);

        await response.ThrowOnGeminiErrorAsync();
        
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    public async Task DeleteFileAsync(string filename, CancellationToken cancellationToken)
    {
        using HttpResponseMessage response = await httpClient.DeleteAsync(
            $"{FileUrl}{filename}?key={apiKey}");
        
        await response.ThrowOnGeminiErrorAsync();
    }

    public void Dispose()
    {
        // TODO release managed resources here
        httpClient.Dispose();
    }
}
