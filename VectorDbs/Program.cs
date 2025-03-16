using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.AI;

namespace VectorDbs;

class ChromaRequestBody
{
    [JsonPropertyName("embeddings")]
    public List<string> Embeddings { get; }

    [JsonPropertyName("metadatas")] 
    public List<string>? Metadatas { get; } = null; 

    [JsonPropertyName("documents")]
    public List<string> Documents { get; }
    
    [JsonPropertyName("ids")]
    public List<string> Ids { get; }

    public ChromaRequestBody(List<string> embeddings, List<string> documents, List<string> ids)
    {
        Embeddings = embeddings;
        Documents = documents;
        Ids = ids;
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        List<string> documents = ["Hello world"];
        
        OllamaEmbeddingGenerator embeddingGenerator = new("http://localhost:11434/", "nomic-embed-text:latest");

        var result = await embeddingGenerator.GenerateAsync(
            documents,
            cancellationToken: CancellationToken.None);

        List<string> embeddings = new();
        StringBuilder sb = new();
        foreach (Embedding<float> embedding in result)
        {
            bool first = true;
            foreach (var emb in embedding.Vector.ToArray())
            {
                if (first)
                {
                    first = false;
                    sb.Append(emb);   
                }
                else
                    sb.Append(emb + ",");
            }
            
            embeddings.Add(sb.ToString());
            sb.Clear();
        }

        using HttpClient client = new();
        using HttpRequestMessage request = new();

        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri("http://localhost:8000/api/v2/tenants/default_tenant/databases/testdb/collections/4f7f8e61-d888-4ce5-99a9-1ae3cdf3f83f/add");
        
        ChromaRequestBody content = new(
            embeddings,
            documents,
            ["id1"]);

        Console.WriteLine(JsonSerializer.Serialize(content));
        request.Content = JsonContent.Create(content);
        
        using HttpResponseMessage response = await client.SendAsync(request);
        
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }
        else
        {
            Console.WriteLine("Embedding into chroma was successful!");
        }
    }
}