using System;
using System.Collections.Generic;
using System.Globalization;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Vgmdb;

/// <inheritdoc />
public class VgmdbPlugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    public VgmdbPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    /// <summary>
    /// Gets the current plugin instance, so the API client can read the
    /// configured server URL.
    /// </summary>
    public static VgmdbPlugin Instance { get; private set; }

    /// <inheritdoc />
    public override string Name => "VGMdb";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("44616595-5798-47ad-8658-3c09f3030505");

    /// <inheritdoc />
    public IEnumerable<PluginPageInfo> GetPages()
    {
        yield return new PluginPageInfo
        {
            Name = Name,
            EmbeddedResourcePath = string.Format(
                CultureInfo.InvariantCulture,
                "{0}.Configuration.configPage.html",
                GetType().Namespace),
        };
    }
}
