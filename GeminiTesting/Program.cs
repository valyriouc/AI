
using System.Reflection;
using AIConnector.Common;
using AIConnector.Gemini;

internal static class Program
{
    public static async Task Main()
    {
        string filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "secret.txt");
        string apiKey = File.ReadAllText(filepath).Trim();

        GeminiModelBuilder builder = new GeminiModelBuilder()
            .WithModelMode(GeminiMode.ContentGeneration)
            .WithModelVariant(GeminiModelVariant.Flash15)
            .WithApiKey(apiKey);

        IModel model = builder.Build();
        string response = await model.GenerateContentAsync();
        Console.WriteLine(response);
    }
}