$URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:streamGenerateContent?alt=sse&key=$($args[0])"
Write-Host $URL;
curl $URL -H 'Content-Type: application/json' --no-buffer -d '{ "contents":[{"parts":[{"text": "Write a story about a magic backpack."}]}]}'