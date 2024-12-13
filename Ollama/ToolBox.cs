using System.Text;

namespace Ollama;

public class ToolBox
{
    private readonly Dictionary<string, ITool> _tools = new();

    public void Add(string name, ITool tool)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (!_tools.TryAdd(name, tool))
        {
            throw new ArgumentException($"Tool with name {name} already exists.");
        }
    }

    public void Remove(string name)
    {
        if (!_tools.Remove(name, out ITool? _))
        {
            throw new ArgumentException($"Cannot find tool with name {name}.");
        }
    }

    public ITool Get(string name)
    {
        if (!_tools.TryGetValue(name, out ITool? tool))
        {
            throw new ArgumentException($"Cannot find tool with name {name}.");
        }

        if (tool is null)
        {
            throw new Exception("The tool is null.");
        }

        return tool;
    }

    public string CreateDescription()
    {
        StringBuilder sb = new();

        foreach (KeyValuePair<string, ITool> item in _tools)
        {
            sb.AppendLine($"- {item.Key}: {item.Value.GetDescription()}");
        }

        return sb.ToString();
    }
}