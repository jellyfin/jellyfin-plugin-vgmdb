using System.Collections.Generic;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;

namespace Jellyfin.Plugin.Vgmdb.ExternalIds;

public class VgmdbExternalUrlProvider : IExternalUrlProvider
{
    public string Name => "VGMdb";

    public IEnumerable<string> GetExternalUrls(BaseItem item)
    {
        if (item.TryGetProviderId(VgmdbAlbumExternalId.ExternalId, out var externalId)
            && item is MusicAlbum)
        {
            yield return $"https://vgmdb.net/album/{externalId}";
        }

        if (item.TryGetProviderId(VgmdbArtistExternalId.ExternalId, out externalId)
            && item is MusicArtist)
        {
            yield return $"https://vgmdb.net/artist/{externalId}";
        }
    }
}
