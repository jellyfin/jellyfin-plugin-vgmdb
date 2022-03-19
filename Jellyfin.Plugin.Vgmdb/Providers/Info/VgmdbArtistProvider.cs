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

public class VgmdbArtistProvider : IRemoteMetadataProvider<MusicArtist, ArtistInfo>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly VgmdbApi _api;

    public VgmdbArtistProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _api = new VgmdbApi(httpClientFactory);
    }

    public string Name => "VGMdb";

    public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
    {
        return _httpClientFactory.CreateClient(NamedClient.Default).GetAsync(url, cancellationToken);
    }

    public async Task<MusicArtist> GetArtistByIdAsync(int id, CancellationToken cancellationToken)
    {
        var response = await _api.GetArtistByIdAsync(id, cancellationToken).ConfigureAwait(false);

        if (response == null)
        {
            return null;
        }

        var artist = new MusicArtist
        {
            ProviderIds =
            {
                [VgmdbArtistExternalId.ExternalId] = response.Id.ToString(CultureInfo.InvariantCulture)
            },
            Name = response.Name
        };

        var image = new ItemImageInfo
        {
            Path = response.PictureFull,
            Type = ImageType.Primary
        };
        artist.SetImage(image, 0);

        artist.Overview = response.Notes;

        return artist;
    }

    public async Task<int?> GetIdAsync(ArtistInfo info, CancellationToken cancellationToken)
    {
        var providedId = info.GetProviderId(VgmdbArtistExternalId.ExternalId);
        if (providedId != null)
        {
            return int.Parse(providedId, CultureInfo.InvariantCulture);
        }

        var searchResults = await GetSearchResults(info, cancellationToken).ConfigureAwait(false);

        foreach (var result in searchResults)
        {
            var id = result.GetProviderId(VgmdbArtistExternalId.ExternalId);

            if (id != null)
            {
                return int.Parse(id, CultureInfo.InvariantCulture);
            }
        }

        return null;
    }

    public async Task<MetadataResult<MusicArtist>> GetMetadata(ArtistInfo info, CancellationToken cancellationToken)
    {
        var id = await GetIdAsync(info, cancellationToken).ConfigureAwait(false);

        if (id != null)
        {
            return new MetadataResult<MusicArtist>
            {
                Item = await GetArtistByIdAsync(id.Value, cancellationToken).ConfigureAwait(false)
            };
        }

        return new MetadataResult<MusicArtist>();
    }

    public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(ArtistInfo searchInfo, CancellationToken cancellationToken)
    {
        var response = await _api.GetSearchResultsAsync(searchInfo.Name, cancellationToken).ConfigureAwait(false);
        if (response == null)
        {
            return null;
        }

        var searchResults = new List<RemoteSearchResult>();

        foreach (var artistEntry in response.Results.Artists)
        {
            var artist = await GetArtistByIdAsync(artistEntry.Id, cancellationToken).ConfigureAwait(false);
            var result = new RemoteSearchResult
            {
                ProviderIds = artist.ProviderIds,
                Name = artist.Name,
                ImageUrl = artist.PrimaryImagePath
            };

            searchResults.Add(result);
        }

        return searchResults;
    }
}
