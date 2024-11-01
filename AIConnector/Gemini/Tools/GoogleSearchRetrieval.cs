namespace AIConnector.Gemini.Tools;

public enum GoogleSearchRetrievalMode
{
    Unspecified,
    Dynamic
}

public readonly struct GoogleSearchRetrieval(
    GoogleSearchRetrievalMode mode,
    double dynamicThreshold)
{
    public GoogleSearchRetrievalMode Mode { get; } = mode;    
    
    public double DynamicThreshold { get; } = dynamicThreshold;
}