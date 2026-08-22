using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.Vgmdb;

/// <inheritdoc />
public class PluginConfiguration : BasePluginConfiguration
{
    /// <summary>
    /// Gets or sets the base URL of the VGMdb API server.
    /// </summary>
    /// <remarks>
    /// The backend was previously a compile-time constant pointing at
    /// https://vgmdb.info, so there was no way to redirect the plugin when
    /// that host became unreachable. Any deployment of hufman/vgmdb serves
    /// the same endpoints, so pointing this at a self-hosted instance works
    /// without any other change.
    /// </remarks>
    public string ServerUrl { get; set; } = "https://vgmdb.info";
}
