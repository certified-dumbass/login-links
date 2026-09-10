<p align="center">
  <img src="banner.png" alt="Dreamstreaming Terms" width="100%">
</p>

# 🔗 Dreamstreaming Terms

A Jellyfin plugin that adds configurable **Terms of Service** and **Privacy Policy** links to the Jellyfin login screen.

The plugin allows server administrators to link to an external website or serve a local HTML document directly through Jellyfin.

Built for **Jellyfin 10.11.x**.

---

## ⚠️ Important: JavaScript Injector is required

Dreamstreaming Terms requires the **JavaScript Injector** plugin to display its links on the Jellyfin login screen.

For Jellyfin **10.11.x**, add the JavaScript Injector repository in:

**Jellyfin Dashboard → Plugins → Repositories**

```text
https://raw.githubusercontent.com/n00bcodr/jellyfin-plugins/main/10.11/manifest.json
```

Install **JavaScript Injector** from the plugin catalog and restart Jellyfin.

> **Current v1.0.1 note:** automatic script registration is still being improved. If Dreamstreaming Terms does not automatically appear in JavaScript Injector after restarting Jellyfin, the Dreamstreaming Terms login script must currently be added to JavaScript Injector manually. This is planned to be improved in a future update.

---

## ✨ Features

- 🔗 Add a **Terms of Service** link to the Jellyfin login screen
- 🔒 Add an optional **Privacy Policy** link
- 🌐 Use an external website URL
- 📄 Use a local `.html` or `.htm` file
- ✏️ Customize the displayed link text
- 🎨 Configure link color, hover color, font size, font weight, underline, opacity and spacing
- 🪟 Choose whether links open in a new tab
- • Optional separator between Terms and Privacy links
- ⚙️ Configure everything from the Jellyfin dashboard
- 🔌 Uses JavaScript Injector for Jellyfin Web UI integration
- 🛡️ Local server file paths are never exposed to unauthenticated clients

---

## 📋 Requirements

- Jellyfin **10.11.x**
- **JavaScript Injector** for Jellyfin
- .NET 9 compatible Jellyfin installation

JavaScript Injector is not optional if you want the links to appear on the Jellyfin login screen. Dreamstreaming Terms handles the configuration and legal-document endpoints; JavaScript Injector runs the client-side script that adds the links to Jellyfin Web.

---

## 📦 Installation

### 1. Install JavaScript Injector

Open:

**Jellyfin Dashboard → Plugins → Repositories**

Add the following repository for Jellyfin 10.11.x:

```text
https://raw.githubusercontent.com/n00bcodr/jellyfin-plugins/main/10.11/manifest.json
```

Then open:

**Dashboard → Plugins → Catalog**

Install **JavaScript Injector** and fully restart Jellyfin.

### 2. Add the Dreamstreaming Terms repository

Open:

**Jellyfin Dashboard → Plugins → Repositories**

Add a new repository and use:

```text
https://raw.githubusercontent.com/certified-dumbass/login-links/main/manifest.json
```

You can name the repository:

```text
Dreamstreaming Terms
```

### 3. Install Dreamstreaming Terms

Go to:

**Dashboard → Plugins → Catalog**

Find **Dreamstreaming Terms** and install it.

Restart Jellyfin after installation.

### 4. Check JavaScript Injector

After restarting Jellyfin, check JavaScript Injector for a Dreamstreaming Terms login script.

For the current **v1.0.1** release, automatic registration may not occur on every installation. If the script is not present, it currently needs to be added manually to JavaScript Injector. When adding it manually, make sure it is **enabled** and does **not require authentication**, because the script must run before a user has logged in.

---

## ⚙️ Configuration

After installation, open:

**Dashboard → Plugins → Dreamstreaming Terms**

The plugin can be enabled or disabled completely from its configuration page.

The plugin also contains customization options for the appearance of the login links, including link color, hover color, font size, font weight, underline, opacity, spacing and margins.

---

## 📜 Terms of Service

The Terms of Service link can be configured independently.

Available options include:

- Enable or disable the link
- Custom link text
- Website URL
- Local HTML file
- Open in a new tab

### External website

Select:

```text
Link type: Website URL
```

Then enter a URL, for example:

```text
https://example.com/terms
```

### Local HTML file

Select:

```text
Link type: Local HTML file
```

Then enter the full path to an HTML file on the Jellyfin server.

Example on Windows:

```text
C:\Dreamstreaming\Legal\terms.html
```

Supported file types:

```text
.html
.htm
```

The actual filesystem path is not sent to the browser. The document is instead served through the plugin's Jellyfin endpoint.

---

## 🔒 Privacy Policy

A Privacy Policy link can optionally be enabled as well.

It has the same options as the Terms of Service link:

- External URL
- Local HTML file
- Custom link text
- Open in new tab

Terms of Service and Privacy Policy can use different link types.

For example:

```text
Terms of Service
→ Local HTML file

Privacy Policy
→ External website
```

---

## 🎨 Link Customization

Dreamstreaming Terms v1.0.1 includes configurable styling options for the login links:

- Link color
- Hover color
- Font size
- Font weight
- Underline on/off
- Link opacity
- Link spacing
- Margin above and below the links

These values are configured from the Dreamstreaming Terms plugin settings and are provided to the login script through the public configuration endpoint.

---

## 🖥️ Example

A configured Jellyfin login screen can display:

```text
                 Sign In

        Terms of Service • Privacy Policy
```

The exact text and appearance can be changed from the plugin settings.

---

## 📁 Local HTML Example

A very simple Terms of Service document could look like this:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Terms of Service</title>
</head>

<body>

    <h1>Terms of Service</h1>

    <p>
        Welcome to our Jellyfin server.
    </p>

    <p>
        By using this server, you agree to the Terms of Service.
    </p>

</body>
</html>
```

Save the file somewhere accessible by the Jellyfin server and enter its full path in the plugin configuration.

---

## 🔧 How It Works

Dreamstreaming Terms consists of a server-side Jellyfin plugin and a small client-side login script.

The Dreamstreaming Terms plugin stores the configuration, provides the public display configuration and serves configured local HTML documents.

**JavaScript Injector is responsible for running the client-side script inside Jellyfin Web.** The script detects the Jellyfin login page, retrieves the Dreamstreaming Terms configuration and adds the configured legal links to the login form.

```text
Dreamstreaming Terms
        ↓
Public configuration
        ↓
JavaScript Injector
        ↓
loginlinks.js
        ↓
Jellyfin login screen
```

For local documents, clicking a link uses the Dreamstreaming Terms endpoint rather than exposing the server filesystem path.

---

## 🔐 Security

The public configuration endpoint only exposes information required to display the login links.

Local filesystem paths are not included in the public configuration.

When using a local document, clients only receive a plugin route such as:

```text
/Dreamstreaming/Terms/Terms
```

instead of the actual server path.

Only `.html` and `.htm` files are accepted as local legal documents.

The JavaScript Injector script must be configured to run without requiring authentication because it is used on the login screen.

---

## 🧩 Compatibility

The current release is designed for:

```text
Jellyfin 10.11.x
.NET 9
```

Dreamstreaming Terms also requires a JavaScript Injector version compatible with Jellyfin 10.11.x.

Because the plugin modifies the Jellyfin Web login experience through JavaScript Injector, future Jellyfin Web UI changes may require plugin updates.

---

## 🐛 Issues & Feedback

Found a bug or have an idea for the plugin?

Please open an issue in this GitHub repository and include:

- Jellyfin version
- Dreamstreaming Terms version
- JavaScript Injector version
- Relevant Jellyfin log output
- Steps to reproduce the problem

---

## 🤖 AI Assistance

Parts of this project were developed with the assistance of AI.

AI was used as a development tool for code generation, debugging, documentation, and implementation guidance.

The plugin should still be considered community software and is not officially affiliated with Jellyfin.

---

## ⚠️ Disclaimer

Dreamstreaming Terms is an unofficial Jellyfin plugin.

This project is not affiliated with, endorsed by, or maintained by the Jellyfin project.

JavaScript Injector is a separate community plugin and is not bundled with Dreamstreaming Terms.

Use it at your own risk.

---

## 💜 Enjoy!

Made for Jellyfin servers that want their legal links somewhere slightly more useful than buried in a random webpage.

Enjoy your server! 💜
