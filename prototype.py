import ollama
import chromadb

class LocalOllamaEmbedding(chromadb.EmbeddingFunction[chromadb.Documents]):
    def __init__(self) -> None:
        pass

    def __call__(self, input: chromadb.Documents) -> chromadb.Embeddings:
        embeddings = [ollama.embeddings("llama3", prompt=doc)["embedding"] for doc in input]
        print(embeddings)
        return embeddings

client = chromadb.HttpClient(host="192.168.178.47")

col = client.get_or_create_collection(name="testing", embedding_function=LocalOllamaEmbedding)

col.add(ids=["id1"], documents=["Hello my friend, we are going to die!", "We are software engineers working on the goal to get big!"])

result = col.get(ids=["id1"], include="embedding")
print(result)