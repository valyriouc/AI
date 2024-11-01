namespace AIConnector.Gemini;

public class GeminiGenerationConfig(
    List<string> stopSequence,
    string resposneMimeType,
    int candidateCount,
    int maxOutputTokens,
    double temperature,
    double topP,
    int topK,
    double presencePenalty,
    double frequencyPenalty,
    bool responseLogProbs,
    int logProbs)
{
    public List<string> StopSequence { get; } = stopSequence;

    public string ResponseMimeType { get; } = resposneMimeType;

    public int CandidateCount { get; } = candidateCount;

    public int MaxOutputTokens { get; } = maxOutputTokens;

    public double Temperature { get; } = temperature;

    public double TopP { get; } = topP;

    public int TopK { get; } = topK;

    public double PresencePenalty { get; } = presencePenalty;

    public double FrequencyPenalty { get; } = frequencyPenalty;

    public bool ResponseLogProbs { get; } = responseLogProbs;

    public int LogProbs { get; } = logProbs;
}