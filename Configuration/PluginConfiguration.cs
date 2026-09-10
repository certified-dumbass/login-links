using MediaBrowser.Model.Plugins;

namespace Dreamstreaming.TermsPlugin.Configuration;

public class PluginConfiguration : BasePluginConfiguration
{
    public PluginConfiguration()
    {
        PluginEnabled = true;

        TermsEnabled = true;
        TermsLinkText = "Terms of Service";
        TermsLinkType = "Url";
        TermsUrl = string.Empty;
        TermsLocalFilePath = string.Empty;
        TermsOpenInNewTab = true;

        PrivacyEnabled = false;
        PrivacyLinkText = "Privacy Policy";
        PrivacyLinkType = "Url";
        PrivacyUrl = string.Empty;
        PrivacyLocalFilePath = string.Empty;
        PrivacyOpenInNewTab = true;

        ShowSeparator = true;

        LinkColor = "#b78cff";
        LinkHoverColor = "#ffffff";

        FontSize = 15;
        FontWeight = 500;

        UnderlineLinks = false;

        LinkOpacity = 0.9;

        LinkSpacing = 10;

        MarginTop = 18;
        MarginBottom = 8;
    }


    // ==============================
    // GENERAL
    // ==============================

    public bool PluginEnabled { get; set; }


    // ==============================
    // TERMS OF SERVICE
    // ==============================

    public bool TermsEnabled { get; set; }

    public string TermsLinkText { get; set; }

    /// <summary>
    /// Supported values:
    /// Url
    /// File
    /// </summary>
    public string TermsLinkType { get; set; }

    public string TermsUrl { get; set; }

    public string TermsLocalFilePath { get; set; }

    public bool TermsOpenInNewTab { get; set; }


    // ==============================
    // PRIVACY POLICY
    // ==============================

    public bool PrivacyEnabled { get; set; }

    public string PrivacyLinkText { get; set; }

    /// <summary>
    /// Supported values:
    /// Url
    /// File
    /// </summary>
    public string PrivacyLinkType { get; set; }

    public string PrivacyUrl { get; set; }

    public string PrivacyLocalFilePath { get; set; }

    public bool PrivacyOpenInNewTab { get; set; }


    // ==============================
    // DISPLAY
    // ==============================

    public bool ShowSeparator { get; set; }


    // ==============================
    // CUSTOMIZATION
    // ==============================

    /// <summary>
    /// Default link text color.
    /// Example:
    /// #b78cff
    /// </summary>
    public string LinkColor { get; set; }


    /// <summary>
    /// Link color when hovering over it.
    /// Example:
    /// #ffffff
    /// </summary>
    public string LinkHoverColor { get; set; }


    /// <summary>
    /// Font size in pixels.
    /// </summary>
    public int FontSize { get; set; }


    /// <summary>
    /// CSS font weight.
    ///
    /// Common values:
    /// 300 = Light
    /// 400 = Normal
    /// 500 = Medium
    /// 600 = Semi Bold
    /// 700 = Bold
    /// </summary>
    public int FontWeight { get; set; }


    /// <summary>
    /// Whether links should be underlined.
    /// </summary>
    public bool UnderlineLinks { get; set; }


    /// <summary>
    /// Link opacity.
    ///
    /// Recommended range:
    /// 0.1 - 1.0
    /// </summary>
    public double LinkOpacity { get; set; }


    /// <summary>
    /// Space around each link in pixels.
    /// </summary>
    public int LinkSpacing { get; set; }


    /// <summary>
    /// Space above the legal links container in pixels.
    /// </summary>
    public int MarginTop { get; set; }


    /// <summary>
    /// Space below the legal links container in pixels.
    /// </summary>
    public int MarginBottom { get; set; }
}