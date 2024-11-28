class CompetitionBlock extends HTMLElement {
    constructor() {
        super();
        this.matchesData = [];
        this.isConnectedCallbackExecuted = false;
    }

    static get observedAttributes() {
        return ["competition-id", "competition-emblem", "matches", "is-recent"];
    }

    connectedCallback() {
        this.isConnectedCallbackExecuted = true;
        this.loadInitialMatches();
        this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (!this.isConnectedCallbackExecuted) return; 

        if (name === "is-recent" && oldValue !== newValue) {
            this.fetchData(); 
        } else if (name === "matches") {
            this.loadInitialMatches();
            this.render();
        }
    }

    loadInitialMatches() {
        const matchesJson = this.getAttribute("matches");
        try {
            this.matchesData = JSON.parse(matchesJson) || [];
        } catch (error) {
            console.error("Invalid JSON for matches:", error);
            this.matchesData = [];
        }
    }

    async fetchData() {
        const competitionId = this.getAttribute("competition-id");
        const isRecent = this.getAttribute("is-recent") === "true";

        if (!competitionId) {
            console.error("Competition ID is missing for competition-block");
            return;
        }

        try {
            const response = await fetch(`/api/football/matches?competitionsId=${competitionId}&isRecent=${isRecent}`);
            if (!response.ok) {
                throw new Error(`Failed to fetch data for competition ID ${competitionId}`);
            }

            const data = await response.json();
            this.matchesData = data.matches || [];
            this.render();
        } catch (error) {
            console.error(`Error fetching data for competition ID ${competitionId}:`, error);
        }
    }

    render() {
        this.innerHTML = `
            ${this.getStyles()}
            <div class="competition">
                <div class="carousel"></div>
            </div>
        `;

        this.renderMatches();
    }

    renderMatches() {
        const carousel = this.querySelector(".carousel");
        if (!carousel) return;

        carousel.innerHTML = ""; 
        this.matchesData.forEach(match => {
            const matchCard = document.createElement("match-card");
            matchCard.setAttribute("match-data", JSON.stringify(match));
            carousel.appendChild(matchCard);
        });
    }

    getStyles() {
        return `
            <style>
                .carousel {
                    display: flex;
                    overflow-x: auto;
                    scroll-behavior: smooth;
                    padding: 10px 0;
                }
                .carousel::-webkit-scrollbar {
                    display: none;
                }
                .match-card {
                    flex: 0 0 auto;
                    margin-right: 15px;
                }
            </style>
        `;
    }
}

customElements.define("competition-block", CompetitionBlock);
