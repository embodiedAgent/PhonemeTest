import requests
url = "http://localhost:8081/"
data = {
    "command": "armlift",
    "text": "Test",
    "expression": "1_Happy_",
    "expression_value": 100
}
requests.post(url, json=data, timeout=5)