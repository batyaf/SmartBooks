const apiUrl = "http://localhost:7145/api";

//Bring Customer Details From QB
document.getElementById("GetDetailsFromQb").addEventListener(onclick, async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");

    if (!token) {
        alert("Please login first!");
        return;
    }

    try {
        const response = await fetch(`${apiUrl}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}/Coustomer/getFromQB`
            }
        });

        if (response.ok) {
            const data = await response.json();
            createInfoSections(data.Customer);
        } else {
            const error = await response.json();
            alert(`Error: ${error.message}`);
        }
    } catch (err) {
        console.error("Error:", err);
        alert("Failed to fetch data.");
    }
});

document.getElementById("AuthorizeQb").addEventListener(onclick, async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");

    if (!token) {
        alert("Please login first!");
        return;
    }

    try {
        const response = await fetch(`${apiUrl}/QB/QBAuthorize`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });

        if (response.ok) {
            toggleAuthanticate(true);
        } else {
            const error = await response.json();
            alert(`Error: ${error.message}`);
        }
    } catch (err) {
        console.error("Error:", err);
        alert("Failed to Authorize Qb.");
    }
});

document.getElementById("body").addEventListener(onload, async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");

    if (!token) {
        alert("Please login first!");
        return;
    }

    try {
        const response = await fetch(`${apiUrl}/Coustomer`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            }
        });

        if (response.ok) {
            const data = await response.json();
            if (!data.IsAuthorticated) {
                toggleAuthanticate(false);
            }
            else {
                toggleAuthanticate(true);
                createInfoSections(data.Customer);
            }
        } else {
            const error = await response.json();
            alert(`Error: ${error.message}`);
        }
    } catch (err) {
        console.error("Error:", err);
        alert("Failed to fetch data.");
    }
});




function toggleAuthanticate(isQbAuthanticated) {
    document.getElementById("AuthorizeQb").style.display = !isQbAuthanticated ? "block" : "none";
    document.getElementById("CustomerDetails").style.display = isQbAuthanticated ? "block" : "none";
}
function createInfoSections(data) {
    const container = document.getElementById('customer-info');
    container.innerHTML = ''; 

    // Iterate through each section in the data
    for (const [sectionName, sectionData] of Object.entries(data)) {
        if (!sectionData || Object.keys(sectionData).length === 0) continue;
        const section = document.createElement('section');
        section.className = 'info-section';
        const header = document.createElement('h2');
        header.textContent = sectionName.replace(/([A-Z])/g, ' $1').trim();
        section.appendChild(header);

        // Create individual property entries
        for (const [key, value] of Object.entries(sectionData)) {
            const paragraph = document.createElement('p');

            const label = document.createElement('span');
            label.className = 'label';
            label.textContent = key.replace(/([A-Z])/g, ' $1').trim() + ':';

            const valueSpan = document.createElement('span');
            valueSpan.className = 'value';
            valueSpan.textContent = value ?? 'N/A';

            paragraph.appendChild(label);
            paragraph.appendChild(valueSpan);
            section.appendChild(paragraph);
        }

        container.appendChild(section);
    }
}

