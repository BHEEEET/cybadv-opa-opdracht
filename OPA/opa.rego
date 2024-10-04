package barmanagement
import future.keywords

# Default deny policy
default allow := false  

# Decode the JWT token
jwt := io.jwt.decode(input.access_token)
claims := jwt[1]  # Access the claims, which is the second array

# Check if the JWT is valid
valid_jwt := jwt != null


# VALIDATION NOG
# !!!!!!!!!!!
# !!!!!!!!!!!!

allow {
    print("AHTAOIZERIDQFD")
    print(input.request.headers.Authorization)

    valid_jwt
    input.request.path == "/api/bar"
    input.request.method == "POST"
    input.request.body.DrinkName == "Beer"
    to_number(claims.age) >= 16       # Check age condition
    
    some i in claims.role  # Ensure claims has role
    i == "customer"         # Check if any role in JWT is in the allowed roles for Beer
}

allow {
    valid_jwt
    input.request.path == "/api/bar"
    input.request.method == "POST"
    input.request.body.DrinkName == "Fristi"
    
    some i in claims.role  # Ensure claims has role
    i == "customer"         # Check if any role in JWT is in the allowed roles for Fristi
}

allow {
    valid_jwt
    input.request.path == "/api/managebar"
    input.request.method == "POST"
    
    some i in claims.role  # Ensure claims has role
    i == "bartender"        # Check if any role in JWT is in the allowed roles for managebar
}