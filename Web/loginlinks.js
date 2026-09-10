(function () {

    const CONTAINER_ID =
        "dreamstreaming-legal-links";

    const CONFIG_URL =
        "/Dreamstreaming/Terms/PublicConfig";

    let isAdding =
        false;


    // ============================================================
    // CONFIGURATION
    // ============================================================

    async function getConfig() {

        try {

            const response =
                await fetch(
                    CONFIG_URL,
                    {
                        method: "GET",
                        credentials: "same-origin",
                        cache: "no-store"
                    }
                );


            if (!response.ok) {

                console.warn(
                    "[Dreamstreaming Terms] PublicConfig failed:",
                    response.status
                );

                return null;
            }


            return await response.json();
        }
        catch (error) {

            console.error(
                "[Dreamstreaming Terms] Failed to load configuration:",
                error
            );

            return null;
        }
    }


    // ============================================================
    // LOGIN PAGE DETECTION
    // ============================================================

    function isLoginPage() {

        const url =
            (
                window.location.pathname +
                window.location.hash
            ).toLowerCase();


        if (
            url.includes("login") ||
            url.includes("signin")
        ) {
            return true;
        }


        return !!document.querySelector(
            ".loginPage, " +
            "#loginPage, " +
            "[data-role='page'].loginPage, " +
            ".loginPageContainer"
        );
    }


    // ============================================================
    // FIND LOGIN FORM
    // ============================================================

    function findLoginForm() {

        const selectors = [

            ".loginPage form",

            "#loginPage form",

            ".loginPageContainer form"

        ];


        for (const selector of selectors) {

            const element =
                document.querySelector(
                    selector
                );


            if (element) {
                return element;
            }
        }


        return null;
    }


    // ============================================================
    // SAFE NUMBER
    // ============================================================

    function safeNumber(
        value,
        fallback
    ) {

        const number =
            Number(value);


        if (
            Number.isNaN(number) ||
            !Number.isFinite(number)
        ) {
            return fallback;
        }


        return number;
    }


    // ============================================================
    // CREATE LINK
    // ============================================================

    function createLink(
        text,
        href,
        newTab,
        config
    ) {

        const link =
            document.createElement("a");


        // --------------------------------------------------------
        // CONTENT
        // --------------------------------------------------------

        link.textContent =
            text;


        link.href =
            href;


        link.className =
            "dreamstreaming-legal-link";


        // --------------------------------------------------------
        // CUSTOMIZATION VALUES
        // --------------------------------------------------------

        const linkColor =
            config.linkColor ||
            "#b78cff";


        const hoverColor =
            config.linkHoverColor ||
            "#ffffff";


        const fontSize =
            safeNumber(
                config.fontSize,
                15
            );


        const fontWeight =
            safeNumber(
                config.fontWeight,
                500
            );


        const opacity =
            Math.min(
                1,
                Math.max(
                    0.1,
                    safeNumber(
                        config.linkOpacity,
                        0.9
                    )
                )
            );


        const spacing =
            Math.max(
                0,
                safeNumber(
                    config.linkSpacing,
                    10
                )
            );


        // --------------------------------------------------------
        // STYLING
        // --------------------------------------------------------

        link.style.display =
            "inline-block";


        link.style.color =
            linkColor;


        link.style.fontSize =
            fontSize + "px";


        link.style.fontWeight =
            String(
                fontWeight
            );


        link.style.margin =
            "0 " +
            spacing +
            "px";


        link.style.opacity =
            String(
                opacity
            );


        link.style.textDecoration =
            config.underlineLinks
                ? "underline"
                : "none";


        link.style.cursor =
            "pointer";


        link.style.transition =
            "color 0.2s ease, " +
            "opacity 0.2s ease";


        // --------------------------------------------------------
        // HOVER
        // --------------------------------------------------------

        link.addEventListener(
            "mouseenter",
            function () {

                link.style.color =
                    hoverColor;


                link.style.opacity =
                    "1";
            }
        );


        link.addEventListener(
            "mouseleave",
            function () {

                link.style.color =
                    linkColor;


                link.style.opacity =
                    String(
                        opacity
                    );
            }
        );


        // --------------------------------------------------------
        // NEW TAB
        // --------------------------------------------------------

        if (newTab) {

            link.target =
                "_blank";


            link.rel =
                "noopener noreferrer";
        }


        return link;
    }


    // ============================================================
    // REMOVE DUPLICATES
    // ============================================================

    function removeDuplicates() {

        const containers =
            document.querySelectorAll(
                "#" + CONTAINER_ID
            );


        if (containers.length <= 1) {
            return;
        }


        for (
            let i = 1;
            i < containers.length;
            i++
        ) {

            containers[i].remove();
        }
    }


    // ============================================================
    // CREATE LOGIN LINKS
    // ============================================================

    async function addLinks() {

        removeDuplicates();


        if (!isLoginPage()) {
            return;
        }


        if (
            document.getElementById(
                CONTAINER_ID
            ) ||
            isAdding
        ) {
            return;
        }


        /*
         * Lock immediately.
         *
         * Without this, several MutationObserver events can start
         * multiple async config requests before the first link is
         * added.
         */

        isAdding =
            true;


        try {

            const config =
                await getConfig();


            if (
                !config ||
                !config.pluginEnabled
            ) {
                return;
            }


            /*
             * Check again after the async request.
             */

            if (
                document.getElementById(
                    CONTAINER_ID
                )
            ) {
                return;
            }


            const loginForm =
                findLoginForm();


            if (!loginForm) {

                console.warn(
                    "[Dreamstreaming Terms] Login form not found."
                );

                return;
            }


            // ====================================================
            // CONTAINER
            // ====================================================

            const container =
                document.createElement(
                    "div"
                );


            container.id =
                CONTAINER_ID;


            container.style.textAlign =
                "center";


            container.style.width =
                "100%";


            container.style.marginTop =
                Math.max(
                    0,
                    safeNumber(
                        config.marginTop,
                        18
                    )
                ) +
                "px";


            container.style.marginBottom =
                Math.max(
                    0,
                    safeNumber(
                        config.marginBottom,
                        8
                    )
                ) +
                "px";


            const links =
                [];


            // ====================================================
            // TERMS OF SERVICE
            // ====================================================

            if (config.termsEnabled) {

                const linkType =
                    String(
                        config.termsLinkType ||
                        ""
                    ).toLowerCase();


                const href =
                    linkType === "file"
                        ? config.termsLocalUrl
                        : config.termsUrl;


                if (href) {

                    links.push(
                        createLink(
                            config.termsLinkText ||
                                "Terms of Service",

                            href,

                            config.termsOpenInNewTab,

                            config
                        )
                    );
                }
            }


            // ====================================================
            // PRIVACY POLICY
            // ====================================================

            if (config.privacyEnabled) {

                const linkType =
                    String(
                        config.privacyLinkType ||
                        ""
                    ).toLowerCase();


                const href =
                    linkType === "file"
                        ? config.privacyLocalUrl
                        : config.privacyUrl;


                if (href) {

                    links.push(
                        createLink(
                            config.privacyLinkText ||
                                "Privacy Policy",

                            href,

                            config.privacyOpenInNewTab,

                            config
                        )
                    );
                }
            }


            // ====================================================
            // ADD LINKS
            // ====================================================

            links.forEach(
                function (
                    link,
                    index
                ) {

                    if (
                        index > 0 &&
                        config.showSeparator
                    ) {

                        const separator =
                            document.createElement(
                                "span"
                            );


                        separator.textContent =
                            "•";


                        separator.style.opacity =
                            "0.5";


                        separator.style.fontSize =
                            safeNumber(
                                config.fontSize,
                                15
                            ) +
                            "px";


                        separator.style.margin =
                            "0 3px";


                        container.appendChild(
                            separator
                        );
                    }


                    container.appendChild(
                        link
                    );
                }
            );


            if (links.length === 0) {

                console.warn(
                    "[Dreamstreaming Terms] No legal links are enabled."
                );

                return;
            }


            loginForm.appendChild(
                container
            );


            console.log(
                "[Dreamstreaming Terms] Login links added successfully."
            );
        }
        finally {

            isAdding =
                false;
        }
    }


    // ============================================================
    // JELLYFIN SPA OBSERVER
    // ============================================================

    const observer =
        new MutationObserver(
            function () {

                if (
                    !isAdding &&
                    !document.getElementById(
                        CONTAINER_ID
                    )
                ) {

                    addLinks();
                }
            }
        );


    observer.observe(
        document.documentElement,
        {
            childList: true,
            subtree: true
        }
    );


    // ============================================================
    // JELLYFIN NAVIGATION
    // ============================================================

    window.addEventListener(
        "hashchange",
        function () {

            setTimeout(
                addLinks,
                250
            );
        }
    );


    window.addEventListener(
        "popstate",
        function () {

            setTimeout(
                addLinks,
                250
            );
        }
    );


    // ============================================================
    // INITIAL LOAD
    // ============================================================

    setTimeout(
        addLinks,
        500
    );


    setTimeout(
        addLinks,
        1500
    );


})();