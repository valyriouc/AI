import ollama
import chromadb

from langchain_community.document_loaders import PyPDFLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter

loader = PyPDFLoader("C:/Hackerspace/Books/Business/Adam2020_Book_Blockchain-TechnologieFürUnter.pdf")

documents = loader.load()

split = RecursiveCharacterTextSplitter(chunk_size=1000, chunk_overlap=200)
documents = split.split_documents(documents=documents)

# TODO: Wrap this up into a usable application
class LocalOllamaEmbedding(chromadb.EmbeddingFunction):
    def __init__(self) -> None:
        pass

    def __call__(self, input: chromadb.Documents) -> chromadb.Embeddings:
        embeddings = [ollama.embeddings("llama3", prompt=doc)["embedding"] for doc in input]
        print(len(embeddings))
        return embeddings

client = chromadb.PersistentClient(path="./testing")
col = client.get_or_create_collection(name="books", embedding_function=LocalOllamaEmbedding())

ids = [f"id{i}" for i in range(0, len(documents))]
col.add(ids=ids, documents=[i.page_content for i in documents])

olla = ollama.Client("localhost")

query = input("Type in your question? ")
query_embeddings = ollama.embeddings("llama3", prompt=query)["embedding"]

result = col.query(query_texts=query)

result = ollama.generate(model="llama3", prompt=f'Please generate a response for the question {query} based on the following context {result["documents"]}')

print(result)
col.delete(ids=["id1", "id2"])