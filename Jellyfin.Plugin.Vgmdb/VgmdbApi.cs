using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Vgmdb.Models;
using MediaBrowser.Common.Net;

namespace Jellyfin.Plugin.Vgmdb;

public class VgmdbApi
{
    private const string RootUrl = @"https://vgmdb.info";
    private readonly IHttpClientFactory _httpClientFactory;

    public VgmdbApi(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ArtistResponse> GetArtistByIdAsync(int id, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
        using var response = await httpClient.GetAsync(RootUrl + "/artist/" + id + "?format=json", cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<ArtistResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<AlbumResponse> GetAlbumById(int id, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
        using var response = await httpClient.GetAsync(RootUrl + "/album/" + id + "?format=json", cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<AlbumResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    public async Task<SearchResponse> GetSearchResultsAsync(string name, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
        using var response = await httpClient.GetAsync(RootUrl + "/search?format=json&q=" + WebUtility.UrlEncode(name), cancellationToken).ConfigureAwait(false);
        return await response.Content.ReadFromJsonAsync<SearchResponse>(cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
