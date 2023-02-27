import requests
url = "http://localhost:8089/"
data = {
    "command": "armlift",
    "text": "Das ist ein längerer test",
    "expression": "1_Happy_",
    "expression_value": 100
}
requests.post(url, json=data, timeout=5)