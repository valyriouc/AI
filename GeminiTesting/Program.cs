
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using AIConnector.Gemini.Files;

internal static class Program
{
    public static async Task Main()
    {
        string filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "secret.txt");
        string apiKey = (await File.ReadAllTextAsync(filepath)).Trim();

        await TestingStreaming(apiKey);
    }

    private static async Task TestingStreaming(string apiKey)
    {
        using HttpRequestMessage request = new(HttpMethod.Post,
            $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:streamGenerateContent?alt=sse&key={apiKey}");

        string json =
            $$"""
            {
              "contents": 
              [
                {
                    "parts": 
                    [
                        {
                            "text": "Write a story about a magic backpack"
                        }
                    ]
                }
              ]  
            }
            """;

        request.Content = new StringContent(json);
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

        using HttpClient client = new();
        
        using var response = await client.SendAsync(request);

        // await using Stream stream = await response.Content.ReadAsStreamAsync();

        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);
        // Memory<byte> buffer = new byte[1024];
        // while (true)
        // {
        //     int wrote = stream.Read(buffer.Span);
        //
        //     if (wrote == 0)
        //     {
        //         break;
        //     }
        //     
        //     Console.WriteLine($"Wrote {wrote} bytes");
        //
        //     string content = Encoding.UTF8.GetString(buffer[..wrote].Span);
        //     Console.WriteLine(content);
        // }
    }
    
    private static async Task FileApiTesting(string apiKey)
    {
        GeminiFileUploader uploader = new(apiKey);
        //
        // GeminiFile file = new(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "joke.txt"));
        // GeminiFileMetadata metadata = await uploader.UploadFileAsync(file, CancellationToken.None);
        //
        // string name = metadata.Name.Split("/")[1];
        await foreach (GeminiFileMetadata meta in uploader.ListFilesAsync(CancellationToken.None))
        {
            Console.WriteLine(meta.Name);
        }
    }
}