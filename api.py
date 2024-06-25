import chromadb
import flask
import ollama
import uuid
import os 
from langchain_community.document_loaders import PyPDFLoader
from langchain.text_splitter import RecursiveCharacterTextSplitter

app = flask.Flask(__name__)

# This class is a custom embedding function for local Ollama embeddings.
class LocalOllamaEmbedding(chromadb.EmbeddingFunction):
    def __init__(self) -> None:
        pass

    def __call__(self, input: chromadb.Documents) -> chromadb.Embeddings:
        embeddings = [ollama.embeddings("nomic-embed-text", prompt=doc)["embedding"] for doc in input]
        return embeddings
    
def update_database(client, topic: str, documents) -> chromadb.Collection:
    col = client.get_or_create_collection(name=topic, embedding_function=LocalOllamaEmbedding())
    count = col.count()
    ids = [f"id{i}" for i in range(count, count + len(documents))]
    col.add(ids=ids, documents=[i.page_content for i in documents])
    return col

def load_document(path: str): 
    loader = PyPDFLoader(path)
    documents = loader.load()
    split = RecursiveCharacterTextSplitter(chunk_size=1000, chunk_overlap=200)
    documents = split.split_documents(documents=documents)
    return documents

def handle_question(client: chromadb.PersistentClient, context: dict):
    question_emb = ollama.embeddings("nomic-embed-text", prompt=context["question"])["embedding"]
    col = client.get_collection(name=context["topic"], embedding_function=LocalOllamaEmbedding())
    result = col.query(query_embeddings=question_emb)
    prompt = f"""Please provide me an answer for the question {context['question']} based on the following {result['documents']}
            If a good answer can generated from your training data, then also use this data to answer the question. 
            
            How answers should be formatted:
            - The should be very short 
            - The should contain only the neccessary information
            - Use bullet points if possible 
            - Try to describe everything in a practical way (no complicated explainations)
            
            What to do when you don't no the answer:
            - Just say you can't answer the question with your available information/knowledge

            What to do when getting asked to provide examples: 
            - Try to find code samples in the given context {result['documents']}
            - Program a simple example to show me how it can look like 
            - Try to find simple real-world examples 

            What to do when getting asked to do a programming task:
            - Please reject this 
            
            What to do when you getting ask to provide a prototype solution for an idea described to you:
            - Tell how to start out by providing links, resources, tools
            - Provide a quick list of things to do, to setup a testing environment 
            - Provide program code if needed 
            - Provide explanations about the resources, links and tools you suggested
            """
    return ollama.generate(model="llama3", prompt=prompt)

@app.route("/api/", methods=["GET", "POST"])
def index():
    if flask.request.method == "GET":
        pass
    elif flask.request.method == "POST":
        client = chromadb.PersistentClient()
        context = flask.request.get_json()
        print(context)
        return handle_question(client, context)["response"]

@app.route("/api/embed/", methods=["POST"]) 
def embed():
    topic = flask.request.args.get("topic")
    raw_bytes = flask.request.get_data()
    filename = None
    while True:
        filename = str(uuid.uuid4())
        if (not os.path.exists(os.path.join(".", "tmp", f"{filename}.pdf"))):
            break
    filepath = os.path.join(".", "tmp", f"{filename}.pdf")
    with open(filepath, "wb") as fobj:
        fobj.write(raw_bytes)
    documents = load_document(filepath)
    client = chromadb.PersistentClient()
    update_database(client, topic, documents)
    os.remove(filepath)
    return {"status": "200", "msg": "Embedded document!"}

@app.route("/api/list/collections/", methods=["GET"])
def list_collections():
    client = chromadb.PersistentClient()
    return [c.name for c in client.list_collections()]

def parse_args(args):
    parsed = {
        "address": "127.0.0.1",
        "port": 7120,
    }
    for i in range(0, len(args)):
        if args[i] == "-h":
            parsed["address"] = args[i+1]
            i += 1
        if args[i] == "-p":
            parsed["port"] = int(args[i+1])
            i += 1
    return parsed

def main(args):
    parsed = parse_args(args)
    app.run(host=parsed["address"], port=parsed["port"])

if __name__ == "__main__":
    import sys 
    main(sys.argv)
