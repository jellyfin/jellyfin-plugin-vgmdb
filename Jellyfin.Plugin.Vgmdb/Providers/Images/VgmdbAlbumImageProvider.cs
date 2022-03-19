using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Vgmdb.ExternalIds;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.Vgmdb.Providers.Images;

public class VgmdbAlbumImageProvider : IRemoteImageProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly VgmdbApi _api;

    public VgmdbAlbumImageProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _api = new VgmdbApi(httpClientFactory);
    }

    public string Name => "VGMdb";

    public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
    {
        return _httpClientFactory.CreateClient(NamedClient.Default).GetAsync(url, cancellationToken);
    }

    public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
    {
        var images = new List<RemoteImageInfo>();

        var id = item.GetProviderId(VgmdbAlbumExternalId.ExternalId);

        // todo use a search to find id
        if (id == null)
        {
            return images;
        }

        var album = await _api.GetAlbumById(int.Parse(id, CultureInfo.InvariantCulture), cancellationToken).ConfigureAwait(false);

        images.Add(new RemoteImageInfo
        {
            Url = album.PictureFull,
            ThumbnailUrl = album.PictureSmall
        });

        return images;
    }

    public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
    {
        return new List<ImageType>
        {
            ImageType.Primary
        };
    }

    public bool Supports(BaseItem item) => item is MusicAlbum;
}
