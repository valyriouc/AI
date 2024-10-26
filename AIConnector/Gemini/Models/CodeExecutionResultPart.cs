namespace AIConnector.Gemini;

public readonly struct CodeExecutionResultPart(
    GeminiCodeOutcomeType outcome, 
    string output) : IGeminiRequestPart
{
    public GeminiCodeOutcomeType Outcome { get; } = outcome;

    public string Output { get; } = output;
}

public enum GeminiCodeOutcomeType
{
    Unspecified,
    Ok,
    Failed,
    Deadline
}

internal static class GeminiCodeOutcomeTypeExtensions
{
    public static string AsString(this GeminiCodeOutcomeType self)
    {
        return self switch
        {
            GeminiCodeOutcomeType.Ok => "OUTCOME_OK",
            GeminiCodeOutcomeType.Unspecified => "OUTCOME_UNSPECIFIED",
            GeminiCodeOutcomeType.Failed => "OUTCOME_FAILED",
            GeminiCodeOutcomeType.Deadline => "OUTCOME_DEADLINE_EXCEEDED",
            _ => throw new NotImplementedException()
        };
    }
}