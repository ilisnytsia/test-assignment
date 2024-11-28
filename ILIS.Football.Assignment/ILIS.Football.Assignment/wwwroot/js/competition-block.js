class CompetitionBlock extends HTMLElement {
    constructor() {
        super();
        this.matchesData = [];
        this.currentIndex = 0; 
        this.matchesPerBatch = 3;
        this.isConnectedCallbackExecuted = false;
    }

    static get observedAttributes() {
        return ["competition-id", "competition-emblem", "matches", "is-recent"];
    }

    connectedCallback() {
        this.isConnectedCallbackExecuted = true;
        this.loadInitialMatches();
        this.render();
        this.initializeLazyLoading();
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (!this.isConnectedCallbackExecuted) return;

        if (name === "matches") {
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

        const fragment = document.createDocumentFragment();

        for (let i = this.currentIndex; i < this.currentIndex + this.matchesPerBatch; i++) {
            if (i >= this.matchesData.length) break;

            const match = this.matchesData[i];
            const matchCard = document.createElement("match-card");
            matchCard.setAttribute("match-data", JSON.stringify(match));
            fragment.appendChild(matchCard);
        }

        carousel.appendChild(fragment);

        this.currentIndex += this.matchesPerBatch;
    }

    initializeLazyLoading() {
        const carousel = this.querySelector(".carousel");
        if (!carousel) return;

        const observer = new IntersectionObserver(entries => {
            const lastEntry = entries[0];
            if (lastEntry.isIntersecting) {
                this.renderMatches();

                if (this.currentIndex >= this.matchesData.length) {
                    observer.disconnect();
                }
            }
        });

        const observeLastElement = () => {
            const lastElement = carousel.lastElementChild;
            if (lastElement) observer.observe(lastElement);
        };

        observeLastElement();

        const mutationObserver = new MutationObserver(observeLastElement);
        mutationObserver.observe(carousel, { childList: true });
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
                    flex: 0 0 calc(100% / 3 - 10px); /* Adjust width for 3 cards per view */
                    margin-right: 15px;
                    background: #ff5733;
                    color: #fff;
                    border-radius: 10px;
                    padding: 10px;
                    text-align: center;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
                }
            </style>
        `;
    }
}

customElements.define("competition-block", CompetitionBlock);
