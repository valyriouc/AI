namespace OllamaMicrosoft;

public interface IAgentTool 
{
    public string Name { get; }

    public string GetDescription();
    
    public Task<ToolResponse> RunAsync(ToolRequest request, CancellationToken cancellationToken);
}
