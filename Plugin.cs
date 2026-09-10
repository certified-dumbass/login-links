using Dreamstreaming.TermsPlugin.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Dreamstreaming.TermsPlugin;

public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    public Plugin(
        IApplicationPaths applicationPaths,
        IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    public override string Name =>
        "Dreamstreaming Terms";

    public override Guid Id =>
        Guid.Parse(
            "6F98C7A1-3D2B-4E17-A945-82C644BE7D31"
        );

    public override string Description =>
        "Adds configurable Terms of Service and Privacy Policy links to the Jellyfin login experience.";

    public static Plugin? Instance { get; private set; }

    public IEnumerable<PluginPageInfo> GetPages()
    {
        yield return new PluginPageInfo
        {
            Name = Name,

            EmbeddedResourcePath =
                $"{GetType().Namespace}.Configuration.configPage.html"
        };
    }
}