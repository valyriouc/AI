namespace AIConnector.Gemini.Tools;

public class GeminiTool(
    List<FunctionDeclaration> functionDeclarations,
    GoogleSearchRetrieval googleSearchRetrieval,
    CodeExecution codeExecution)
{
    public List<FunctionDeclaration> FunctionDeclarations { get; } = functionDeclarations;

    public GoogleSearchRetrieval GoogleSearchRetrieval { get; } = googleSearchRetrieval;

    public CodeExecution CodeExecution { get; } = codeExecution;
}