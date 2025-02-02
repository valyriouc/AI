using System.Text;
using System.Text.Json;
using Microsoft.Extensions.AI;

namespace OllamaMicrosoft;

public class Agent : IDisposable
{
    private readonly Dictionary<string, IAgentTool> _tools;

    private readonly IChatClient _chatClient;
    
    private IEnumerable<(string, string)> ToolDescriptions => _tools.Select(x => (x.Value.Name, x.Value.GetDescription()));

    private List<ChatMessage> _chatHistory = [];
    
    public Agent(
        IChatClient chatClient,
        Dictionary<string, IAgentTool> tools)
    {
        this._chatClient = chatClient;
        this._tools = tools;
    }

    private string GetAgentDescription()
    {
        StringBuilder sb = new StringBuilder();

        foreach ((string name, string description) in ToolDescriptions)
        {
            sb.AppendLine(
                $"""
                {name}:
                {description}
                """);
        }

        string prompt =
            $$"""
              You are an assistant that helps processing user requests by determining the appropriate action and arguments based 
              on the users request. You have access to the following tools:

              Available tools:
              {{sb.ToString()}}    

              Instructions:
              - Decide whether to use a tool or respond to the user directly.
              - You always respond with the following JSON object with "tool" and "input" fields
              - If you choose to respond to the user directly, set "tool" to "user_response" and "input" to the content of the response
              - **Important**: Provide the response **only** as a valid JSON object. Do not include any additional text or formatting. Do **not** use a markdown code block to wrap the valid JSON object in
              - Ensure that the JSON is properly formatted without any syntax errors.

              Response format:
              {
                  "tool": "<tool_name>",
                  "input": "<input>",
              }
              """;

        return prompt;
    }
    
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _chatHistory.Add(new ChatMessage(ChatRole.System, GetAgentDescription()));
        
        while (true)
        {
            Console.WriteLine("Your prompt: ");
            string? userPrompt = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(userPrompt))
            {
                Console.WriteLine("Input cannot be empty. Please try again.");
                continue;
            }
            
            _chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

            CancellationTokenSource cts = new();
            Console.WriteLine("Ai response: ");
            string response = "";
            
            await foreach (StreamingChatCompletionUpdate item in _chatClient.CompleteStreamingAsync(
                               _chatHistory, new ChatOptions() { ResponseFormat = new ChatResponseFormatText()},
                               cancellationToken: cts.Token))
            {
                response += item.Text;
            }

            await HandleResponseAsync(response, cancellationToken);
            
            _chatHistory.Add(new ChatMessage(ChatRole.Assistant, response));
            Console.WriteLine();
        }
    }

    private async Task HandleResponseAsync(string response, CancellationToken cancellationToken)
    {
        Console.WriteLine(response);
        var request = JsonSerializer.Deserialize<ToolRequest>(response, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true});
        if (this._tools.TryGetValue(request!.Tool, out IAgentTool? tool))
        {
            await tool!.RunAsync(request, cancellationToken);
            return;
        }
        
        Console.WriteLine(request.Input);
    }

    public void Dispose() => this._chatClient.Dispose();
}

class Program
{
    static async Task Main(string[] args)
    {
        IChatClient chatClient = new OllamaChatClient(
            new Uri("http://localhost:11434/"),
            "llama3.2");
        
        FileStorageTool tool = new();
        
        using Agent agent = new(chatClient, new Dictionary<string, IAgentTool>()
        {
            { tool.Name, tool}
        });
        
        await agent.RunAsync(CancellationToken.None);
    }
}