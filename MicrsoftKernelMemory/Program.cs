using System.Collections.ObjectModel;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.AI.Ollama;
using Microsoft.KernelMemory.Configuration;

namespace MicrsoftKernelMemory;

public class ChatHistory
{
    private readonly Collection<string> _messages = [];
    public void AddUserMessage(string message) => _messages.Add($"User: {message}");
    public void AddAssistantMessage(string message) => _messages.Add($"Assistant: {message}");
    public string GetHistoryAsContext(int maxMessages = 10) => string.Join("\n", _messages.TakeLast(maxMessages));
}

class Program
{
    static async Task Main(string[] args)
    {
        var ollamaConfig = new OllamaConfig()
        {
            TextModel = new OllamaModelConfig("mistral-nemo:latest") { MaxTokenTotal = 125000, Seed = 42, TopK = 7 },
            EmbeddingModel = new OllamaModelConfig("nomic-embed-text:latest") { MaxTokenTotal = 2048 },
            Endpoint = "http://localhost:11434/"
        };
        
        var memoryBuilder = new KernelMemoryBuilder()
            .WithOllamaTextGeneration(ollamaConfig)
            .WithOllamaTextEmbeddingGeneration(ollamaConfig)
            .WithSearchClientConfig(new SearchClientConfig() { AnswerTokens = 4096 })
            .WithCustomTextPartitioningOptions(new TextPartitioningOptions() { MaxTokensPerParagraph = 50, OverlappingTokens = 40 });
        var memory = memoryBuilder.Build();

        List<string> files = new();
        for (int i = 0; i < args.Length; i++)
        {
            files.Add(args[i]);
        }
        
        var index = "ragwithollama";
        var document = new Document().AddFiles(files);
        var documentID = await memory.ImportDocumentAsync(document, index: index);
        var chatHistory = new ChatHistory();
        
        Console.WriteLine("Ask your question (type 'Exit' to quit):");
        while (true)
        {
            try
            {

                var userInput = Console.ReadLine();
                if (userInput == "Exit") break;

                var fullQuery = chatHistory.GetHistoryAsContext() + "\nUser: " + userInput;
                var answer = await memory.AskAsync(fullQuery, index, MemoryFilters.ByDocument(documentID),
                    minRelevance: .6f);

                chatHistory.AddUserMessage(userInput);
                chatHistory.AddAssistantMessage(answer.Result);
                Console.WriteLine(answer.Result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}