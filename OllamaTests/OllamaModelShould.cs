using Ollama;

namespace OllamaTests;

public class OllamaModelShould
{
    [Fact]
    public async Task ShouldWorkOnLocalhost()
    {
        OllamaModel model = new OllamaModel(
            "llama3.2");

        AiResponse response = await model.RunAsync(
            "Write me a one-liner that prints \"Hello world\" in python.", 
            CancellationToken.None);
        
        Assert.Equal(ResponseState.Success, response.State);
    }
    
    
}