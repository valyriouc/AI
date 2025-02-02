using System.Reflection;

namespace OllamaMicrosoft;

public class FileStorageTool : IAgentTool
{
    private const string ToolName = nameof(FileStorageTool);
    
    public string Name => ToolName;

    public string GetDescription() =>
        "Requires an input text and stores this in a file on the local file system.";

    public async Task<ToolResponse> RunAsync(ToolRequest request, CancellationToken cancellationToken)
    {
        if (request.Tool != ToolName)
        {
            throw new Exception("Invalid tool name");
        }
        
        var guid = Guid.NewGuid();
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, $"{guid}.txt");
        await File.WriteAllTextAsync(path, request.Input);
        return new ToolResponse($"File was created under path {path}!");
    }
}