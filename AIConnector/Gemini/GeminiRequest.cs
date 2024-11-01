using AIConnector.Gemini.Tools;

namespace AIConnector.Gemini;

public sealed class GeminiRequest(
    List<GeminiRequestContent> contents,
    List<GeminiTool> tools,
    FunctionCallingConfig toolConfig,
    List<GeminiSafetySetting> safetySettings,
    GeminiRequestContent systemInstruction,
    GeminiGenerationConfig generationConfig,
    string cachedContent)
{
    public List<GeminiRequestContent> Contents { get; } = contents;

    public List<GeminiTool> Tools { get; } = tools;

    public FunctionCallingConfig ToolConfig { get; } = toolConfig;

    public List<GeminiSafetySetting> SafetySettings { get; } = safetySettings;

    public GeminiRequestContent SystemInstruction { get; } = systemInstruction;

    public GeminiGenerationConfig GenerationConfig { get; } = generationConfig;
    
    public string CachedContent { get; } = cachedContent;
}
