import flask

from flask import request

app = flask.Flask(__name__) 

@app.route("/", methods=["GET", "POST"])
def index():
    if request.method == "GET": 
        pass
    else:
        json = request.get_json()

@app.route("/embed", method=["GET", "POST"]) 
def embed():
    pass

if __name__ == "__main__":  
    app.run("0.0.0.0", "9222")
