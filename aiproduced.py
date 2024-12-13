from flask import Flask, render_template

app = Flask(__name__)

posts = [
    {'title': 'Mein erster Blogbeitrag', 'content': 'Hallo Welt!'},
    {'title': 'Zweiter Beitrag', 'content': 'Dies ist ein weiterer Beitrag.'}
]

@app.route('/')
def index():
    return render_template('index.html', posts=posts)

if __name__ == '__main__':
    app.run(debug=True)