document.addEventListener("DOMContentLoaded", () => {
    const toggle = document.querySelector("slide-toggle");
    const container = document.getElementById("competitions-container");

    /**
     * Render competitions and set up listeners
     */
    function renderCompetitions(data) {
        container.innerHTML = ""; // Clear existing content

        data.forEach((competition) => {
            const competitionDiv = document.createElement("div");
            competitionDiv.className = "competition";
            competitionDiv.setAttribute("data-competition-id", competition.Competition.Id);

            const title = document.createElement("h2");
            title.className = "competition-title";
            title.textContent = competition.Competition.Name;
            competitionDiv.appendChild(title);

            const carousel = document.createElement("div");
            carousel.className = "carousel";
            carousel.innerHTML = `<p>Loading matches...</p>`; // Placeholder
            competitionDiv.appendChild(carousel);

            // Add competition to the DOM
            container.appendChild(competitionDiv);

            // Add a listener for the toggle-change event
            competitionDiv.addEventListener("toggle-change", (event) => {
                const isRecent = event.detail; // Get toggle state
                fetchMatchesForCompetition(competition.Competition.Id, isRecent, carousel);
            });
        });
    }

    /**
     * Fetch matches for a specific competition and update the carousel
     */
    function fetchMatchesForCompetition(competitionId, isRecent, carousel) {
        // Fetch data for this specific competition
        fetch(`/football/matches?competitionId=${competitionId}&isRecent=${isRecent}`, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
            },
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`Failed to fetch matches for competition ${competitionId}`);
                }
                return response.json();
            })
            .then((matches) => {
                carousel.innerHTML = ""; // Clear placeholder

                matches.forEach((match) => {
                    const matchCard = document.createElement("div");
                    matchCard.className = "match-card";

                    matchCard.innerHTML = `
                        <div class="match-details">
                            <h3>${match.HomeTeam.Name} vs ${match.AwayTeam.Name}</h3>
                            <p>${new Date(match.UtcDate).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })}</p>
                        </div>
                        <div class="odds">
                            <div>1: ${match.Odds.Home}</div>
                            <div>X: ${match.Odds.Draw}</div>
                            <div>2: ${match.Odds.Away}</div>
                        </div>
                    `;
                    carousel.appendChild(matchCard);
                });
            })
            .catch((error) => {
                carousel.innerHTML = `<p>Failed to load matches.</p>`;
            });
    }
   

    // Listen for the toggle-change event and propagate it
    toggle.addEventListener("toggle-change", (event) => {
        const isRecent = event.detail;
        const toggleEvent = new CustomEvent("toggle-change", { detail: isRecent });
        document.querySelectorAll(".competition").forEach((competition) => {
            competition.dispatchEvent(toggleEvent); // Notify each competition
        });
    });


});
