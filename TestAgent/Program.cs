
using Ollama;

internal class Program
{
    public static async Task Main()
    {
        FileStorerTool tool = new();
        ToolBox toolBox = new();
        
        toolBox.Add("fileStorer", tool);

        string prompt =
            $$"""
             You are an agent with access to a toolbox. Given a user query,
             you will determine which tool, if any, is best suited to answer the query.
             If you need to use a tool only generate the following JSON response:
             {
                "tool_name": "name_of_the_tool",
                "tool_input": "json_object_with_input_described_for_the_tool_in_the_description"
             }
             
             Here is the list of the tools you have access to:
             {{toolBox.CreateDescription()}}"

             If you get a specific request from the user, which needs the help of a tool please select 
             one from the list. Read the description carefully. There you also is specified what input the tool needs.
             If no tool is required just answer the prompt or if you dont find an appropriate tool just say that you dont know
             how to handle it.
             """;

        Console.WriteLine(prompt);
        using OllamaModel model = new("llama3.2", promptTemplate: prompt);
        while (true)
        {
            Console.Write("Please give me a task");
            string task = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(task))
            {
                continue;
            }

            if (task == "END")
            {
                break;
            }

            var response = await model.RunAsync(task, CancellationToken.None);
            Console.WriteLine(response.Content);
        }

    }
}