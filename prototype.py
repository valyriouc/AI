import ollama
import chromadb

from langchain_community.document_loaders import PyPDFLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter

import sys

def load_document(path: str): 
    loader = PyPDFLoader(path)
    documents = loader.load()
    split = RecursiveCharacterTextSplitter(chunk_size=1000, chunk_overlap=200)
    documents = split.split_documents(documents=documents)
    return documents

class LocalOllamaEmbedding(chromadb.EmbeddingFunction):
    def __init__(self) -> None:
        pass

    def __call__(self, input: chromadb.Documents) -> chromadb.Embeddings:
        embeddings = [ollama.embeddings("nomic-embed-text", prompt=doc)["embedding"] for doc in input]
        return embeddings
    
def update_database(client, documents):
    col = client.get_or_create_collection(name="books", embedding_function=LocalOllamaEmbedding())
    ids = [f"id{i}" for i in range(0, len(documents))]
    col.add(ids=ids, documents=[i.page_content for i in documents])

def parse_args(args: list[str]) -> dict:
    parsed = {"embed": None}
    for i in range(0, len(args)):
        if args[i] == "-e":
            parsed["embed"] = args[i+1]
            i += 1
    return parsed

def main(args: list[str]):
    parsed = parse_args(args)
    
    client = chromadb.PersistentClient(path="./testing")
    
    if (parsed["embed"] != None):
        documents = load_document(parsed["embed"])
        update_database(client, documents)

    question = input("Type in your question? ")
    question_emb = ollama.embeddings("nomic-embed-text", prompt=question)["embedding"]
    
    col = client.get_collection(name="books", embedding_function=LocalOllamaEmbedding())
    result = col.query(query_embeddings=question_emb)

    prompt = f"""
    Please generate a repsonse for the questions {question} based on the given context {result["documents"]}"
    """

    result = ollama.generate(model="llama3", prompt=prompt)
    
    print(result["response"])

if __name__ == "__main__":
    main(sys.argv)