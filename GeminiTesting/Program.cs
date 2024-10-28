
using System.Reflection;
using AIConnector.Gemini.Files;

internal static class Program
{
    public static async Task Main()
    {
        string filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "secret.txt");
        string apiKey = (await File.ReadAllTextAsync(filepath)).Trim();

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