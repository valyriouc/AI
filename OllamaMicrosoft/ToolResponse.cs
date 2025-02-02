using System.Text.Json.Serialization;

namespace OllamaMicrosoft;

public class ToolResponse
{
    public string Output { get; }
    
    [JsonConstructor]
    public ToolResponse(string output)
    {
        Output = output;
    }
    
    
}
