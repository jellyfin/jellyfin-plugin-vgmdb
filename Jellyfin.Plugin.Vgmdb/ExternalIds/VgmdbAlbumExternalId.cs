using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Vgmdb.ExternalIds;

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
public class VgmdbAlbumExternalId : IExternalId
{
    public const string ExternalId = "VGMdbAlbum";

    public string ProviderName => "VGMdb Album";

    public string Key => ExternalId;

    public ExternalIdMediaType? Type => ExternalIdMediaType.Album;

    public string UrlFormatString => "https://vgmdb.net/album/{0}";

    public bool Supports(IHasProviderIds item) => item is MusicAlbum;
}
