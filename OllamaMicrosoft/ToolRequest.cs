using System.Text.Json.Serialization;

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