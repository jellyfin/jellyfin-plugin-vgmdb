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

namespace Jellyfin.Plugin.Vgmdb.Providers.Info;

public class VgmdbAlbumProvider : IRemoteMetadataProvider<MusicAlbum, AlbumInfo>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly VgmdbApi _api;

    public VgmdbAlbumProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _api = new VgmdbApi(httpClientFactory);
    }

    public string Name => "VGMdb";

    public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
    {
        return _httpClientFactory.CreateClient(NamedClient.Default).GetAsync(url, cancellationToken);
    }

    public async Task<MusicAlbum> GetAlbumByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _api.GetAlbumById(id, cancellationToken).ConfigureAwait(false);

        if (response == null)
        {
            return null;
        }

        var album = new MusicAlbum
        {
            ProviderIds =
            {
                [VgmdbAlbumExternalId.ExternalId] = response.Id.ToString(CultureInfo.InvariantCulture)
            },
            Name = response.Names.GetPreferred()
        };

        // todo better date parsing
        _ = int.TryParse(response.ReleaseDate.Split('-')[0], out var productionYear);
        if (productionYear > 0)
        {
            album.ProductionYear = productionYear;
        }

        var image = new ItemImageInfo
        {
            Path = response.PictureFull,
            Type = ImageType.Primary
        };
        album.SetImage(image, 0);

        album.Overview = response.Notes;

        if (response.Categories != null)
        {
            foreach (var category in response.Categories)
            {
                album.AddGenre(category);
            }
        }

        if (response.Organizations != null)
        {
            foreach (var organisation in response.Organizations)
            {
                album.AddStudio(organisation.Names.GetPreferred());
            }
        }

        return album;
    }

    public async Task<int?> GetIdAsync(AlbumInfo info, CancellationToken cancellationToken)
    {
        var providedId = info.GetProviderId(VgmdbAlbumExternalId.ExternalId);
        if (providedId != null)
        {
            return int.Parse(providedId, CultureInfo.InvariantCulture);
        }

        var searchResults = await GetSearchResults(info, cancellationToken).ConfigureAwait(false);

        foreach (var result in searchResults)
        {
            var id = result.GetProviderId(VgmdbAlbumExternalId.ExternalId);

            if (id != null)
            {
                return int.Parse(id, CultureInfo.InvariantCulture);
            }
        }

        return null;
    }

    public async Task<MetadataResult<MusicAlbum>> GetMetadata(AlbumInfo info, CancellationToken cancellationToken)
    {
        var id = await GetIdAsync(info, cancellationToken).ConfigureAwait(false);

        if (id != null)
        {
            return new MetadataResult<MusicAlbum>
            {
                Item = await GetAlbumByIdAsync(id.Value, cancellationToken).ConfigureAwait(false)
            };
        }

        return new MetadataResult<MusicAlbum>();
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(AlbumInfo searchInfo, CancellationToken cancellationToken)
    {
        var response = await _api.GetSearchResultsAsync(searchInfo.Name, cancellationToken).ConfigureAwait(false);

        var searchResults = new List<RemoteSearchResult>();
        if (response == null)
        {
            return null;
        }

        foreach (var albumEntry in response.Results.Albums)
        {
            var album = await GetAlbumByIdAsync(albumEntry.Id, cancellationToken).ConfigureAwait(false);
            var result = new RemoteSearchResult
            {
                ProviderIds = album.ProviderIds,
                Name = album.Name,
                ProductionYear = album.ProductionYear,
                ImageUrl = album.PrimaryImagePath
            };

            searchResults.Add(result);
        }

        return searchResults;
    }
}
