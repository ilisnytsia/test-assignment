class MatchCard extends HTMLElement {
    static get observedAttributes() {
        return ["match-data"];
    }

    constructor() {
        super();
    }

    connectedCallback() {
        if (this.hasAttribute("match-data")) {
            this.loadDataFromAttribute();
        }
    }

    attributeChangedCallback(name, oldValue, newValue) {
        if (name === "match-data" && oldValue !== newValue) {
            this.loadDataFromAttribute();
        }
    }

    loadDataFromAttribute() {
        try {
            const data = JSON.parse(this.getAttribute("match-data"));
            this.render(data);
        } catch (e) {
            console.error("Invalid JSON data provided to 'match-data' attribute.");
        }
    }

    render(data) {
        // Random odds
        const homeOdds = ((Math.random() < 0.5 ? 1 : 2) + Math.random()).toFixed(2);
        const awayOdds = ((Math.random() < 0.5 ? 1 : 2) + Math.random()).toFixed(2);

        this.innerHTML = `
            <style>
                .match-card {
                    position: relative;
                    display: flex;
                    flex: 0 0 auto;
                    width: 250px;
                    background: linear-gradient(135deg, #ff5733, #ff8d72);
                    color: #333333;
                    border-radius: 10px;
                    margin-right: 15px;
                    padding: 10px;
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
                    text-align: center;
                    min-height: 7em;
                }
                .match-details {
                    max-width: 200px;
                }
                .match-details p {
                    margin: 0px;
                }
                .card-image {
                    position: absolute;
                    top: 10px;
                    right: 10px;
                    width: 50px;
                    height: 50px;
                    border-radius: 50%;
                    overflow: hidden;
                    background-size: contain;
                }
                .score {
                    position: absolute;
                    right: 10px;
                    top: 70px;
                    padding: 0 10px;
                    font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;
                    font-size: 0.7em;
                    font-weight: 700;
                }
                .odds {
                    position: absolute;
                    right: 0;
                    top: 70px;
                    padding: 0 10px;
                    font-family: 'Lucida Sans', 'Lucida Sans Regular', 'Lucida Grande', 'Lucida Sans Unicode', Geneva, Verdana, sans-serif;
                    font-size: 0.7em;
                    font-weight: 700;
                }
                .odds span {
                    flex: 1;
                    text-align: center;
                }
            </style>
            <div class="match-card">
                <div class="match-details">
                    <p>${data.utcDateFormatted}</p>
                    <p>${data.homeTeam.name}</p> 
                    vs
                    <p>${data.awayTeam.name}</p>
                </div>
                <div class="card-image" style="background-image: url('${data.homeTeam.crest}');"></div>
                ${data.score?.winner
                ? `<div class="score">${data.score.fullTime.home} : ${data.score.fullTime.away}</div>`
                : `<div class="odds">
                            <span> ${homeOdds}</span> : <span> ${awayOdds}</span>
                   </div>`}
            </div>
        `;
    }
}

customElements.define("match-card", MatchCard);
