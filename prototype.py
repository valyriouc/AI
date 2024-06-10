import ollama
import chromadb

class LocalOllamaEmbedding(chromadb.EmbeddingFunction):
    def __init__(self) -> None:
        pass

    def __call__(self, input: chromadb.Documents) -> chromadb.Embeddings:
        embeddings = [ollama.embeddings("llama3", prompt=doc)["embedding"] for doc in input]
        print(len(embeddings))
        return embeddings

client = chromadb.PersistentClient(path="./testing")
col = client.get_or_create_collection(name="nice", embedding_function=LocalOllamaEmbedding())

col.add(ids=["id1", "id2"], documents=["Hello my friend, we are going to die!", "We are software engineers working on the goal to get big!"])

result = col.get(ids=["id1", "id2"], include=["embeddings"])

olla = ollama.Client("localhost")

query = input("Type in your question? ")
query_embeddings = ollama.embeddings("llama3", prompt=query)["embedding"]

result = col.query(query_texts=query)

print(result)

result = ollama.generate(model="llama3", prompt=f'Please generate a response for the question {query} based on the following context {result["documents"]}')

print(result)
col.delete(ids=["id1", "id2"])