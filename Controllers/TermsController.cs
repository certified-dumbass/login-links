using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dreamstreaming.TermsPlugin.Controllers;

[ApiController]
[Route("Dreamstreaming/Terms")]
public class TermsController : ControllerBase
{
    private readonly ILogger<TermsController> _logger;

    public TermsController(
        ILogger<TermsController> logger)
    {
        _logger = logger;
    }


    // ============================================================
    // PUBLIC CONFIGURATION
    // ============================================================

    [HttpGet("PublicConfig")]
    [AllowAnonymous]
    public IActionResult GetPublicConfiguration()
    {
        var plugin = Plugin.Instance;

        if (plugin is null)
        {
            return StatusCode(
                500,
                "Plugin is not available."
            );
        }

        var configuration =
            plugin.Configuration;

        return Ok(
            new
            {
                // ====================================================
                // GENERAL
                // ====================================================

                pluginEnabled =
                    configuration.PluginEnabled,


                // ====================================================
                // TERMS OF SERVICE
                // ====================================================

                termsEnabled =
                    configuration.TermsEnabled,

                termsLinkText =
                    configuration.TermsLinkText,

                termsLinkType =
                    configuration.TermsLinkType,

                termsUrl =
                    configuration.TermsUrl,

                termsLocalUrl =
                    "/Dreamstreaming/Terms/Terms",

                termsOpenInNewTab =
                    configuration.TermsOpenInNewTab,


                // ====================================================
                // PRIVACY POLICY
                // ====================================================

                privacyEnabled =
                    configuration.PrivacyEnabled,

                privacyLinkText =
                    configuration.PrivacyLinkText,

                privacyLinkType =
                    configuration.PrivacyLinkType,

                privacyUrl =
                    configuration.PrivacyUrl,

                privacyLocalUrl =
                    "/Dreamstreaming/Terms/Privacy",

                privacyOpenInNewTab =
                    configuration.PrivacyOpenInNewTab,


                // ====================================================
                // DISPLAY
                // ====================================================

                showSeparator =
                    configuration.ShowSeparator,


                // ====================================================
                // CUSTOMIZATION
                // ====================================================

                linkColor =
                    configuration.LinkColor,

                linkHoverColor =
                    configuration.LinkHoverColor,

                fontSize =
                    configuration.FontSize,

                fontWeight =
                    configuration.FontWeight,

                underlineLinks =
                    configuration.UnderlineLinks,

                linkOpacity =
                    configuration.LinkOpacity,

                linkSpacing =
                    configuration.LinkSpacing,

                marginTop =
                    configuration.MarginTop,

                marginBottom =
                    configuration.MarginBottom
            }
        );
    }


    // ============================================================
    // TERMS OF SERVICE
    // ============================================================

    [HttpGet("Terms")]
    [AllowAnonymous]
    public IActionResult GetTerms()
    {
        var plugin = Plugin.Instance;

        if (plugin is null)
        {
            return StatusCode(
                500,
                "Plugin is not available."
            );
        }

        var configuration =
            plugin.Configuration;

        if (!configuration.PluginEnabled ||
            !configuration.TermsEnabled)
        {
            return NotFound();
        }

        if (!string.Equals(
                configuration.TermsLinkType,
                "File",
                StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(
                "Terms of Service is not configured to use a local file."
            );
        }

        return ServeHtmlFile(
            configuration.TermsLocalFilePath,
            "Terms of Service"
        );
    }


    // ============================================================
    // PRIVACY POLICY
    // ============================================================

    [HttpGet("Privacy")]
    [AllowAnonymous]
    public IActionResult GetPrivacy()
    {
        var plugin = Plugin.Instance;

        if (plugin is null)
        {
            return StatusCode(
                500,
                "Plugin is not available."
            );
        }

        var configuration =
            plugin.Configuration;

        if (!configuration.PluginEnabled ||
            !configuration.PrivacyEnabled)
        {
            return NotFound();
        }

        if (!string.Equals(
                configuration.PrivacyLinkType,
                "File",
                StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(
                "Privacy Policy is not configured to use a local file."
            );
        }

        return ServeHtmlFile(
            configuration.PrivacyLocalFilePath,
            "Privacy Policy"
        );
    }


    // ============================================================
    // LOCAL HTML FILE
    // ============================================================

    private IActionResult ServeHtmlFile(
        string? filePath,
        string documentName)
    {
        if (string.IsNullOrWhiteSpace(
                filePath))
        {
            return NotFound(
                $"{documentName} file path is not configured."
            );
        }

        var cleanPath =
            filePath.Trim();


        if (!System.IO.File.Exists(
                cleanPath))
        {
            _logger.LogWarning(
                "{DocumentName} file was not found at {FilePath}.",
                documentName,
                cleanPath
            );

            return NotFound(
                $"{documentName} file was not found."
            );
        }


        var extension =
            Path.GetExtension(
                cleanPath
            );


        if (!string.Equals(
                extension,
                ".html",
                StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(
                extension,
                ".htm",
                StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(
                "Only .html and .htm files are supported."
            );
        }


        try
        {
            var html =
                System.IO.File.ReadAllText(
                    cleanPath
                );


            return Content(
                html,
                "text/html; charset=utf-8"
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to read {DocumentName} file at {FilePath}.",
                documentName,
                cleanPath
            );


            return StatusCode(
                500,
                $"Failed to read {documentName} file."
            );
        }
    }
}