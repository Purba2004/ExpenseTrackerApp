let currentUser  = "";

// Helper to get element value
function val(id) {
    return document.getElementById(id).value;
}

// Helper to set headers for JSON
function headers() {
    return { "Content-Type": "application/json" };
}

// Sign up a new user
function signup() {
    const user = { username: val("username"), password: val("password") };
    fetch("/api/auth/signup", {
        method: "POST",
        headers: headers(),
        body: JSON.stringify(user)
    })
    .then(r => r.ok ? alert("Signed up successfully!") : alert("Username already exists"))
    .catch(err => console.error("Signup error:", err));
}

// Log in an existing user
function login() {
    const user = { username: val("username"), password: val("password") };
    fetch("/api/auth/login", {
        method: "POST",
        headers: headers(),
        body: JSON.stringify(user)
    })
    .then(r => {
        if (r.ok) {
            currentUser  = user.username;
            document.getElementById("auth").style.display = "none";
            document.getElementById("main").style.display = "block";
            document.getElementById("welcome").innerText = "Welcome, " + currentUser ;
            load();
        } else {
            alert("Login failed");
        }
    })
    .catch(err => console.error("Login error:", err));
}

// Add an expense
function addExpense() {
    const expense = {
        category: val("category"),
        amount: parseFloat(val("amount")),
        type: val("type"),
        date: new Date().toISOString()
    };
    fetch(`/api/expense/${currentUser }`, {
        method: "POST",
        headers: headers(),
        body: JSON.stringify(expense)
    })
    .then(() => load());
}

// Load all expenses
function load() {
    console.log("Loading expenses for user:", currentUser); // <-- DEBUG
    fetch(`/api/expense/${currentUser}`)
        .then(r => r.json())
        .then(data => {
            console.log("Data received:", data); // <-- DEBUG
            let html = "<tr><th>Date</th><th>Category</th><th>Amount</th><th>Type</th><th>Remaining</th></tr>";
            data.forEach(d => {
                html += `<tr>
                    <td>${d.date}</td>
                    <td>${d.category}</td>
                    <td>${d.amount}</td>
                    <td>${d.type}</td>
                    <td>${d.remainingBalance}</td>
                </tr>`;
            });
            document.getElementById("table").innerHTML = html;
        })
        .catch(err => console.error("Error loading expenses:", err));
}

