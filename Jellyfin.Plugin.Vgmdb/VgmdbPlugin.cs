using System;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Vgmdb;

/// <inheritdoc />
public class VgmdbPlugin : BasePlugin<PluginConfiguration>
{
    public VgmdbPlugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
    }

    /// <inheritdoc />
    public override string Name => "VGMdb";

    /// <inheritdoc />
    public override Guid Id => Guid.Parse("44616595-5798-47ad-8658-3c09f3030505");
}
