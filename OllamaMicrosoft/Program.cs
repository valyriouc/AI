using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;

namespace OllamaMicrosoft;

public class ToolRequest
{
    public string Tool { get; }

    public string Input { get; }

    [JsonConstructor]
    public ToolRequest(string tool, string input)
    {
        Tool = tool;
        Input = input;
    }
}

public interface IAgentTool
{
    public Task<ChatMessage> RunAsync(ToolRequest request);
}

public class FileStorageTool : IAgentTool
{
    private const string ToolName = nameof(FileStorageTool);
    
    public async Task<ChatMessage> RunAsync(ToolRequest request)
    {
        if (request.Tool != ToolName)
        {
            throw new Exception("Invalid tool name");
        }
        
        var guid = Guid.NewGuid();
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, $"{guid}.txt");
        await File.WriteAllTextAsync(path, request.Input);
        return new ChatMessage(ChatRole.Tool, "File was created under path !"); 
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        using IChatClient chatClient = new OllamaChatClient(
            new Uri("http://localhost:11434/"),
            "llama3.2");

        FileStorageTool tool = new();
        List<ChatMessage> messages = new();

        messages.Add(
            new ChatMessage(
                ChatRole.Assistant, 
                "You are an assistant that is able to create files. If you are asked to create a file, please respond with the following json scheme: { \"tool\": \"FileStorageTool\", \"input\": \"The content which should be stored\" }. The json must be valid so it can be understood by the tool. You only use this tool if it is necessary or when the user asks you to do so."));
       
        while (true)
        {
            Console.WriteLine("Your prompt: ");
            string? userPrompt = Console.ReadLine();
            messages.Add(new ChatMessage(ChatRole.User, userPrompt));

            CancellationTokenSource cts = new();
            Console.WriteLine("Ai response: ");
            string response = "";
            
            await foreach (StreamingChatCompletionUpdate item in chatClient.CompleteStreamingAsync(
                               messages, new ChatOptions() { ResponseFormat = new ChatResponseFormatText()},
                               cancellationToken: cts.Token))
            {
                Console.Write(item.Text);
                response += item.Text;
            }

            try
            {
                ToolRequest? tooling = JsonSerializer.Deserialize<ToolRequest>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
                if (tooling is null)
                {
                    continue;
                }
                var toolResponse = await tool.RunAsync(tooling);
                messages.Add(toolResponse);
                Console.WriteLine(toolResponse.Text);
                continue;
            }
            catch (JsonException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            messages.Add(new ChatMessage(ChatRole.Assistant, response));
            Console.WriteLine();
        }
    }
}