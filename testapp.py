from flask import Flask, jsonify, request
from flask_restful import Api, Resource

app = Flask(__name__)
api = Api(app)

# Sample data for demonstration purposes
pokemons = [
    {"id": 1, "name": "Pikachu", "type": "Electric"},
    {"id": 2, "name": "Charmander", "type": "Fire"},
    # ...
]

prompt = """Please provide me an answer for the question {question} based on the following {context}
            If a good answer can generated from your training data, then also use this data to answer the question. 
            
            How answers should be formatted:
            - The should be very short 
            - The should contain only the neccessary information
            - Use bullet points if possible 
            - Try to describe everything in a practical way (no complicated explainations)
            
            What to do when you don't no the answer:
            - Just say you can't answer the question with your available information/knowledge
            """

class Pokemon(Resource):
    def get(self, id):
        if id:
            pokemon = next((p for p in pokemons if p["id"] == id), None)
            if pokemon:
                return jsonify(pokemon)
            else:
                return jsonify({"error": "Pokemon not found"}), 404
        else:
            return jsonify([p for p in pokemons])

    def post(self):
        data = request.get_json()
        new_pokemon = {"id": len(pokemons) + 1, "name": data["name"], "type": data["type"]}
        pokemons.append(new_pokemon)
        return jsonify(new_pokemon), 201

    def put(self, id):
        if id:
            pokemon = next((p for p in pokemons if p["id"] == id), None)
            if pokemon:
                data = request.get_json()
                pokemon["name"] = data["name"]
                pokemon["type"] = data["type"]
                return jsonify(pokemon)
            else:
                return jsonify({"error": "Pokemon not found"}), 404
        else:
            return jsonify({"error": "Invalid ID"}), 400

    def delete(self, id):
        if id:
            pokemon = next((p for p in pokemons if p["id"] == id), None)
            if pokemon:
                pokemons.remove(pokemon)
                return jsonify({"message": "Pokemon deleted successfully"})
            else:
                return jsonify({"error": "Pokemon not found"}), 404
        else:
            return jsonify({"error": "Invalid ID"}), 400

api.add_resource(Pokemon, '/pokemons/<int:id>')

if __name__ == '__main__':
    app.run(debug=True)