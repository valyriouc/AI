import ollama
import chromadb

from langchain_community.document_loaders import PyPDFLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter

def load_document(path: str): 
    loader = PyPDFLoader(path)

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
    
def update_database(client, documents):
    col = client.get_or_create_collection(name="books")

    ids = [f"id{i}" for i in range(0, len(documents))]
    col.add(ids=ids, documents=[i.page_content for i in documents])


def main():
    client = chromadb.PersistentClient(path="./testing")
    olla = ollama.Client("localhost")

    query = input("Type in your question? ")
    query_embeddings = ollama.embeddings("llama3", prompt=query)["embedding"]

    col = client.get_collection(name="books")
    result = col.query(query_texts=query)

    result = ollama.generate(model="llama3", prompt=f'Please generate a response for the question {query} based on the following context {result["documents"]}')

    print(result)

if __name__ == "__main__":
    main()
