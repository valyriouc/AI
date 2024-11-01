using System.Text.Json.Serialization;

namespace AIConnector.Gemini.Tools;

[method: JsonConstructor]
public readonly struct FunctionDeclaration(
    string name,
    string description,
    FunctionDeclarationParameter parameter)
{
    public string Name { get; } = name;
    
    public string Description { get; } = description;

    public FunctionDeclarationParameter Parameters { get; } = parameter;
}

public enum FunctionDeclarationDataType
{
    Unspecified,
    String,
    Number, 
    Integer,
    Boolean,
    Array,
    Object
}

public class FunctionDeclarationParameter(
    FunctionDeclarationDataType type,
    string format,
    string description,
    bool nullable,
    List<string> parameterEnum,
    string maxItems,
    string minItems,
    List<string> required,
    Dictionary<string, object> properties,
    FunctionDeclarationParameter items)
{
    public FunctionDeclarationDataType Type { get; } = type;

    public string Format { get; } = format;
    
    public string Description { get; } = description;

    public bool Nullable { get; } = nullable;

    [JsonPropertyName("enum")]
    public List<string> ParameterEnum { get; } = parameterEnum;

    public string MaxItems { get; } = maxItems;

    public string MinItems { get; } = minItems;

    public List<string> Required { get; } = required;

    public Dictionary<string, object> Properties { get; } = properties;

    public FunctionDeclarationParameter Items { get; } = items;
}