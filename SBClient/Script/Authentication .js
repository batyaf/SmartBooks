const apiUrl = "http://localhost:7145"; 

// Register User
document.getElementById("registerForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const username = document.getElementById("registerUsername").value;
    const password = document.getElementById("registerPassword").value;

    try {
        const response = await fetch(`${apiUrl}/register`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("token", data.token);
            alert("Registration successful!");
            window.location.href = "/page/CustomerDetails";
        } else {
            const error = await response.json();
            alert(`Error: ${error.message}`);
        }
    } catch (err) {
        console.error("Error:", err);
        alert("Failed to register.");
    }
});

// Login User
document.getElementById("loginForm").addEventListener("submit", async (e) => {
    e.preventDefault();
    const username = document.getElementById("loginUsername").value;
    const password = document.getElementById("loginPassword").value;

    try {
        const response = await fetch(`${apiUrl}/login`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ username, password })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("token", data.token); //save token
            alert("Login successful!");
            //toggleForms("logout");
            window.location.href = "/page/CustomerDetails";

        } else {
            const error = await response.json();
            alert(`Error: ${error.message}`);
        }
    } catch (err) {
        console.error("Error:", err);
        alert("Failed to login.");
    }
});

// Logout User
document.getElementById("logoutButton").addEventListener("click", () => {
    localStorage.removeItem("token"); // delete token
    alert("Logged out!");
    toggleForms("login");
});

//choose sign in or sign up
document.getElementById("signInButton").addEventListener("click", () => toggleForms("login"));
document.getElementById("signUpButton").addEventListener("click", () => toggleForms("register"));

//check if there are token already
document.addEventListener('DOMContentLoaded', () => {
    const token = localStorage.getItem('token');
    if (token) {
        toggleForms('logout');
    } else {
        toggleForms('login');
    }
});


// Toggle Forms
function toggleForms(show) {
    document.getElementById("registerForm").style.display = show === "register" ? "block" : "none";
    document.getElementById("loginForm").style.display = show === "login" ? "block" : "none";
    document.getElementById("logoutSection").style.display = show === "logout" ? "block" : "none";
}


