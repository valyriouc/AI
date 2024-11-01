using System.Text;
using AIConnector.Gemini;

namespace AiConnectorTests.Gemini;

public class GeminiStreamPartParserShould
{
    [Fact]
    public void Test1()
    {
        string parts =
            """
            Data: {
                "hello": 100,
                "nice": "world"
            }
            
            """;
        
        ReadOnlySpan<byte> buffer = Encoding.UTF8.GetBytes(parts);
        buffer = GeminiStreamPartParser.Parse(buffer, out List<string> remains);
        
        Assert.Equal(0, buffer.Length);
        Assert.Single(remains);

        string expected =
            """
            {
                "hello": 100,
                "nice": "world"
            }
            """;
        
        Assert.Equal(expected, remains[0]);
        
    }
}