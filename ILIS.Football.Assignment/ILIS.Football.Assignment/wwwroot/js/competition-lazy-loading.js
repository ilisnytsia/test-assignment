document.addEventListener("DOMContentLoaded", () => {
    const competitionsContainer = document.getElementById("competitions-container");
    const competitionsPerBatch = 2; 
    let currentSkip = 2; 
    let isLoading = false; 
    let allDataFetched = false; 

    let isRecent = false; 


    const toggle = document.querySelector("slide-toggle");
    if (toggle) {
        toggle.addEventListener("toggle-change", (event) => {
            isRecent = event.detail; 
            resetAndFetch(); 
        });
    }


    async function fetchCompetitions(skip, take) {
        try {
            const response = await fetch(`/api/football/matches/all?skip=${skip}&take=${take}&isRecent=${isRecent}`);
            if (!response.ok) {
                throw new Error("Failed to fetch competitions");
            }
            const data = await response.json();

            if (!Array.isArray(data)) {
                console.error("Unexpected response format:", data);
                return [];
            }


            return data.filter(item => item && item.competition);
        } catch (error) {
            console.error("Error fetching competitions:", error);
            return [];
        }
    }

    function renderCompetitions(data) {
        const fragment = document.createDocumentFragment();
        data.forEach(competition => {

            if (!competition || !competition.competition) {
                console.warn("Skipping invalid competition data:", competition);
                return;
            }

            const title = document.createElement("h2");
            title.className = "competition-title";
            title.textContent = competition.competition.name;

            const competitionBlock = document.createElement("competition-block");
            competitionBlock.setAttribute("competition-id", competition.competition.id);
            competitionBlock.setAttribute("competition-emblem", competition.competition.emblem);
            competitionBlock.setAttribute("matches", JSON.stringify(competition.matches));

            fragment.appendChild(title);
            fragment.appendChild(competitionBlock);
        });
        competitionsContainer.appendChild(fragment);
    }

    const observer = new IntersectionObserver(async entries => {
        const lastEntry = entries[0];
        if (lastEntry.isIntersecting && !isLoading && !allDataFetched) {
            isLoading = true;

            const data = await fetchCompetitions(currentSkip, competitionsPerBatch);

            if (data.length > 0) {
                renderCompetitions(data);
                currentSkip += competitionsPerBatch;
            } else {
                allDataFetched = true; 
            }

            isLoading = false;
        }
    });

    const observeLastElement = () => {
        const lastElement = competitionsContainer.lastElementChild;
        if (lastElement) observer.observe(lastElement);
    };

    const mutationObserver = new MutationObserver(observeLastElement);
    mutationObserver.observe(competitionsContainer, { childList: true });

    function resetAndFetch() {
        competitionsContainer.innerHTML = ""; 
        currentSkip = 0; 
        allDataFetched = false; 
        isLoading = false;

        fetchCompetitions(currentSkip, competitionsPerBatch).then(data => {
            renderCompetitions(data);
            currentSkip += competitionsPerBatch;
            observeLastElement(); // Reattach observer after resetting
        });
    }

    observeLastElement(); 
});
