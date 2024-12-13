namespace Ollama;

public interface ITool
{
    public string GetDescription();

    public string GetInputFormat();

    public Task<string> ExecuteAsync(string input);
}

public class FileStorerTool : ITool
{
    public string GetDescription()
    {
        string desc =
            """
            This tool can store data in a file on the file system.
            """;
        
        return desc;
    }

    public string GetInputFormat()
    {
        string desc =
            """ 
            {
                "file_name": "the_name_of_the_file",
                "data": "the_data_to_store",
                "file_extension": "the_file_extension"
            }
            """;

        return desc;
    }

    public async Task<string> ExecuteAsync(string input)
    {
        Console.WriteLine(input);
        return string.Empty;
    }
}