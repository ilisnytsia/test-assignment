class SlideToggle extends HTMLElement {
    constructor() {
        super();

        this.attachShadow({ mode: "open" });

        const container = document.createElement("div");
        container.innerHTML = `
         <style>
.toggle-container {
    display: flex;
    align-items: center;
    gap: 10px;
    margin-top: 2em;
}

.toggle {
    position: relative;
    width: 50px;
    height: 24px;
    background-color: #ccc;
    border-radius: 12px;
    cursor: pointer;
    transition: background-color 0.3s ease;
}

    .toggle.active {
        background-color: #3f51b5;
    }

.toggle-thumb {
    position: absolute;
    top: 2px;
    left: 2px;
    width: 20px;
    height: 20px;
    background-color: white;
    border-radius: 50%;
    transition: left 0.3s ease;
}

.toggle.active .toggle-thumb {
    left: 26px;
}

.label {
    font-size: 20px;
    color: #333333;
    font-family: monospace;
}

         </style>
            <div class="toggle-container">
                <div class="toggle">
                    <div class="toggle-thumb"></div>
                </div>
                <label class="label"><slot>Upcoming Matches</slot></label>
            </div>
        `;

        this.shadowRoot.appendChild(container);
    }
    connectedCallback() {
        const toggle = this.shadowRoot.querySelector(".toggle");

        this.isActive = false;

        toggle.addEventListener("click", () => {
            this.isActive = !this.isActive;
            toggle.classList.toggle("active");
            this.updateLabel();
            this.dispatchEvent(new CustomEvent("toggle-change", {
                detail: this.isActive,
            }));
        });
    }

    updateLabel() {
        const slot = this.shadowRoot.querySelector("slot");
        if (this.isActive) {
            slot.textContent = "Recent Matches";
        } else {
            slot.textContent = "Upcoming Matches";
        }
    }
}

customElements.define("slide-toggle", SlideToggle);