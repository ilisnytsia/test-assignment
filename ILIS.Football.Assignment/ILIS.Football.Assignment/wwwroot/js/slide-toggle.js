class SlideToggle extends HTMLElement {
    constructor() {
        super();

        this.attachShadow({ mode: "open" });

        const link = document.createElement("link");
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("href", "/css/toggle-switch.css");

        const container = document.createElement("div");
        container.innerHTML = `
            <div class="toggle-container">
                <div class="toggle">
                    <div class="toggle-thumb"></div>
                </div>
                <label class="label"><slot>Upcoming Matches</slot></label>
            </div>
        `;

        this.shadowRoot.appendChild(link);
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