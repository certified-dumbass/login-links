(function () {

    const CONTAINER_ID = "dreamstreaming-legal-links";

    async function getConfig() {
        try {
            const response = await fetch(
                "/Dreamstreaming/Terms/PublicConfig",
                {
                    method: "GET",
                    credentials: "same-origin"
                }
            );

            if (!response.ok) {
                return null;
            }

            return await response.json();
        }
        catch {
            return null;
        }
    }

    function isLoginPage() {
        return !!document.querySelector(
            ".loginPage, #loginPage, [data-role='page'].loginPage"
        );
    }

    function createLink(text, href, newTab) {

        const link =
            document.createElement("a");

        link.textContent =
            text;

        link.href =
            href;

        link.style.margin =
            "0 10px";

        link.style.opacity =
            "0.8";

        link.style.textDecoration =
            "none";

        if (newTab) {
            link.target =
                "_blank";

            link.rel =
                "noopener noreferrer";
        }

        return link;
    }

    async function addLinks() {

        if (!isLoginPage()) {
            return;
        }

        if (document.getElementById(CONTAINER_ID)) {
            return;
        }

        const config =
            await getConfig();

        if (!config ||
            !config.pluginEnabled) {
            return;
        }

        const loginForm =
            document.querySelector(
                ".loginPage form, #loginPage form"
            );

        if (!loginForm) {
            return;
        }

        const container =
            document.createElement("div");

        container.id =
            CONTAINER_ID;

        container.style.textAlign =
            "center";

        container.style.marginTop =
            "18px";

        container.style.fontSize =
            "0.95em";

        const links = [];

        if (config.termsEnabled) {

            const href =
                config.termsLinkType === "File"
                    ? config.termsLocalUrl
                    : config.termsUrl;

            if (href) {
                links.push(
                    createLink(
                        config.termsLinkText || "Terms of Service",
                        href,
                        config.termsOpenInNewTab
                    )
                );
            }
        }

        if (config.privacyEnabled) {

            const href =
                config.privacyLinkType === "File"
                    ? config.privacyLocalUrl
                    : config.privacyUrl;

            if (href) {
                links.push(
                    createLink(
                        config.privacyLinkText || "Privacy Policy",
                        href,
                        config.privacyOpenInNewTab
                    )
                );
            }
        }

        links.forEach(
            function (link, index) {

                if (index > 0 &&
                    config.showSeparator) {

                    const separator =
                        document.createElement("span");

                    separator.textContent =
                        "•";

                    separator.style.opacity =
                        "0.5";

                    container.appendChild(
                        separator
                    );
                }

                container.appendChild(
                    link
                );
            }
        );

        if (links.length > 0) {
            loginForm.appendChild(
                container
            );
        }
    }

    const observer =
        new MutationObserver(
            function () {
                addLinks();
            }
        );

    observer.observe(
        document.documentElement,
        {
            childList: true,
            subtree: true
        }
    );

    addLinks();

})();