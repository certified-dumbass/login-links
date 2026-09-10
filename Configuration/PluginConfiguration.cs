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
}