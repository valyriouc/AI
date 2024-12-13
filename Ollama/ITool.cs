namespace Ollama;

public interface ITool
{
    public string GetDescription();

    public Task<string> ExecuteAsync(string input);
}

public class FileStorerTool : ITool
{
    public string GetDescription()
    {
        string desc =
            """
            This tool can store data in a file on the file system.
            The input for the tool:
            
            "file_name": "file_name",
            "data": "data"
            """;
        
        return desc;
    }

    public async Task<string> ExecuteAsync(string input)
    {
        Console.WriteLine(input);
        return string.Empty;
    }
}