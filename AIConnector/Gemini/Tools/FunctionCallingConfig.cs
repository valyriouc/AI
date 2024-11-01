namespace AIConnector.Gemini.Tools;

public enum FunctionCallingConfigMode
{
    Unspecified,
    Auto,
    Any,
    None
}

public readonly struct FunctionCallingConfig(
    FunctionCallingConfigMode mode,
    List<string> allowedFunctionNames)
{
    public FunctionCallingConfigMode Mode { get; } = mode;
    
    public List<string> AllowedFunctionNames { get; } = allowedFunctionNames;
}