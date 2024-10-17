import secret
import google.generativeai as genai
import os
import sys

question = sys.argv[1]
key = secret.GEMINI_API_KEY
genai.configure(api_key=key)

for m in genai.list_models():
    print(m.name)

model = genai.GenerativeModel(model_name='gemini-1.5-flash')

prompt = f"""
You are my second brain on thinking/brainstorming innovative and original ideas. 
you break down the problem into its fundamental parts and then setting it together in a new way.
Show me the fundamental parts and then all ideas you can think of. So after considering all this please make it with the following problem/question.
{question}?
"""
response = model.generate_content(prompt)
print(response.text)

# TODO: https://ai.google.dev/

