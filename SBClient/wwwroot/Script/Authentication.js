const apiUrl = "https://localhost:5001/api"; 

// Register User
document.getElementById("registerForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const username = document.getElementById("registerUsername").value;
    const email = document.getElementById("registerEmail").value;
    const password = document.getElementById("registerPassword").value;

    if (!validatePassword(password)){
        return;
    }

    try {
        const response = await fetch(`${apiUrl}/Auth/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, email ,password })
        });

        if (response.ok) {
            const data = await response.json();
            alert("Registration successful!");
        } else {
            const error = await response.json();
            alert(`Error: ${error.message}`);
        }
    } catch (err) {
        console.error("Error:", err);
        console.error("Error:", error);
    }
});

// Login User
document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const username = document.getElementById("loginUsername").value;
    const password = document.getElementById("loginPassword").value;


    try {
        const response = await fetch(`${apiUrl}/Auth/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("token", data.token); //save token
            alert("Login successful!");
            //toggleForms("logout");
            window.location.href = "/page/CustomerDetails.html";

        } else {
            document.getElementById("loginText").style.display = "block";
            const error = await response.json();
            console.error("Error:",error);
        }
    } catch (err) {
        console.error("Error:", err);
        alert("Failed to login.");
    }
});



//choose sign in or sign up
document.getElementById("signInButton").addEventListener("click", () => toggleForms(false));
document.getElementById("signUpButton").addEventListener("click", () => toggleForms(true));




// Toggle Forms
function toggleForms(show) {
    document.getElementById("registerForm").style.display = show  ? "block" : "none";
    document.getElementById("loginForm").style.display = !show ? "block" : "none";
    document.getElementById("signInButton").style.display = show ? "block" : "none";
    document.getElementById("signUpButton").style.display = !show ? "block" : "none";
}

function validatePassword(password) {
    const passwordText = document.getElementById("passwordText");
    passwordText.textContent = "";

    // Define validation rules using regular expressions
    const hasUpperCase = /[A-Z]/.test(password);
    const hasLowerCase = /[a-z]/.test(password);
    const hasNumber = /[0-9]/.test(password);
    const hasSpecialChar = /[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(password);
    const isLengthValid = password.length >= 6;

    // Track overall password validity
    let isValid = true;

    // Validation checks with specific error messages
    if (!hasUpperCase) {
        passwordText.textContent += "Must contain at least one uppercase letter. ";
        isValid = false;
    }

    if (!hasLowerCase) {
        passwordText.textContent += "Must contain at least one lowercase letter. ";
        isValid = false;
    }

    if (!hasNumber) {
        passwordText.textContent += "Must contain at least one number. ";
        isValid = false;
    }

    if (!hasSpecialChar) {
        passwordText.textContent += "Must contain at least one special character. ";
        isValid = false;
    }

    if (!isLengthValid) {
        passwordText.textContent += "Password must be at least 6 characters long. ";
        isValid = false;
    }

    passwordText.style.color = isValid ? "green" : "red";

    return isValid;
}

toggleForms(false);

