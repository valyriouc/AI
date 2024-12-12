import ollama 
import tools 
import agent

if __name__ == "__main__":

    tooling = [ 
        {
            "name": "calculator",
            "doc": "A simple calculator to perform very basic math operations",
            "method": tools.basic_calculator
        },
        {
            "name": "string reverser",
            "doc": "Reverses a given string",
            "method": tools.reverse_string
        },
        {
            "name": "code executor",
            "doc": "Takes a source code as an input and then executes this source code",
            "method": tools.code_executor
        }
    ]


    model_service = ollama.OllamaModel
    model_name = "llama3.1"
    stop = "<|eot_id|>"

    worker = agent.Agent(tools=tooling, model_service=model_service, model_name=model_name)

    while True:
        prompt = input("Ask me anything: ")
        if prompt.lower() == "exit":
            break
        worker.work(prompt)