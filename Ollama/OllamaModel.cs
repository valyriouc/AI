using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Ollama;

public enum ResponseState
{
    Success,
    Failure
}

public struct AIResponse
{
    public ResponseState State { get; }
    
    public string Content { get; }

    internal AIResponse(ResponseState state, string content)
    {
        State = state;
        Content = content;
    }
}

public class OllamaModel : IDisposable
{
    internal struct ModelRequest
    {
        public string Model { get; }

        public string Format { get; }
        
        public string Prompt { get; }
        
        public string SystemMessage { get; }

        public string PromptTemplate { get; }
        
        public bool Streaming { get; } = false;

        public ModelRequest(string model, string prompt, string format, string systemMessage, string promptTemplate, bool streaming)
        {
            Model = model;
            Prompt = prompt;
            Format = format;
            SystemMessage = systemMessage;
            PromptTemplate = promptTemplate;
            Streaming = streaming;
        }
    }
    
    internal struct ModelResponse
    {
        public string Model { get; }
        
        public string Response { get; }

        [JsonConstructor]
        public ModelResponse(string model, string response)
        {
            Model = model;
            Response = response;
        }
    }
    
    public string Name { get; }

    public string BaseUrl { get; }
    
    public string Format { get; }

    public string SystemMessage { get; }

    public string PromptTemplate { get; }

    public bool Streaming { get; } = false;
    
    private readonly HttpClient _httpClient;
    
    public OllamaModel(
        string name, 
        string format="Json",
        string baseUrl = "http://localhost:11434/api/generate",
        string systemMessage="", 
        string promptTemplate="",
        bool streaming=false)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Model name is required!");
        }
        
        Name = name;
        BaseUrl = baseUrl;
        Format = format;
        SystemMessage = systemMessage;
        PromptTemplate = promptTemplate;

        if (streaming)
        {
            throw new ArgumentException("Streaming is currently not supported");
        }
        
        Streaming = streaming;
        _httpClient = new HttpClient();
    }
    
    public async Task<AIResponse> RunAsync(string prompt, CancellationToken cancellationToken)
    {
        ModelRequest request = new ModelRequest(
            Name,
            prompt,
            Format,
            SystemMessage, 
            PromptTemplate, 
            Streaming);

        Uri url = new Uri(BaseUrl);
        
        try
        {
            using HttpResponseMessage response = await _httpClient.PostAsync(
                url, 
                JsonContent.Create<ModelRequest>(request), cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                string errorText =
                    $"""
                     Request to ollama model {Name} failed with status code {response.StatusCode}
                     Response: {await response.Content.ReadAsStringAsync(cancellationToken)}
                     """;
                return new AIResponse(
                    ResponseState.Failure,
                    errorText);
            }
            
            ModelResponse result = await response.Content.ReadFromJsonAsync<ModelResponse>(cancellationToken);
            return new AIResponse(
                ResponseState.Success,
                result.Response);
        }
        catch (Exception e)
        {
            return new AIResponse(
                ResponseState.Failure,
                e.Message);
        }
    }

    public void Dispose()
    {
        // TODO release managed resources here
        _httpClient.Dispose();
    }
}