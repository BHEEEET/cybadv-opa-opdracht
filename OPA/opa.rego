package barmanagement
import future.keywords

# Default deny policy
default allow := false  

# Decode the JWT token
jwt := io.jwt.decode(input.access_token)
claims := jwt[1]  # Access the claims, which is the second array

# Check if the JWT is not empty
valid_jwt := jwt != null

allow {
    print("Included Headers By Default")
    print(input.request.headers.Authorization)

    valid_jwt
    input.request.path == "/api/bar"
    input.request.method == "POST"
    input.request.body.DrinkName == "Beer"

    # Check age condition
    to_number(claims.age) >= 16 
    
    # Ensure claims has role, could be multiple roles thats why its a itteration
    some i in claims.role  
    # Check if any role in JWT is in the allowed roles for Beer
    i == "customer"         
}

allow {
    valid_jwt
    input.request.path == "/api/bar"
    input.request.method == "POST"
    input.request.body.DrinkName == "Fristi"
    
    some i in claims.role 
    i == "customer"
}

allow {
    valid_jwt
    input.request.path == "/api/managebar"
    input.request.method == "POST"
    
    some i in claims.role  
    i == "bartender" 
}