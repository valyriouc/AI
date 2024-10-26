﻿
using System.Reflection;
using AIConnector.Common;
using AIConnector.Gemini;

internal static class Program
{
    public static async Task Main()
    {
        string filepath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "secret.txt");
        string apiKey = (await File.ReadAllTextAsync(filepath)).Trim();

        GeminiModelBuilder builder = new GeminiModelBuilder()
            .WithModelMode(GeminiMode.ContentGenerationStream)
            .WithModelVariant(GeminiModelVariant.Flash15)
            .WithApiKey(apiKey);

        IModel model = builder.Build();
        await foreach (string t in model.StreamContentAsync())
        {
            Console.WriteLine(t);
            Console.WriteLine();    
        }
    }
}