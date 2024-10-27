
using System.Reflection;
using AIConnector.Gemini.Files;

internal static class Program
{
    public static async Task Main()
    {
        string filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "secret.txt");
        string apiKey = (await File.ReadAllTextAsync(filepath)).Trim();

        GeminiFileUploader uploader = new(apiKey);
        
        var result = await uploader.ListFilesAsync(CancellationToken.None);
        Console.WriteLine(result);
        
        GeminiFile file = new(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "joke.txt"));
        await uploader.UploadFileAsync(file, CancellationToken.None);
        
        result = await uploader.ListFilesAsync(CancellationToken.None);
        Console.WriteLine(result);
    }
}