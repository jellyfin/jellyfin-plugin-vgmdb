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
    private const string DefaultRootUrl = @"https://vgmdb.info";
    private readonly IHttpClientFactory _httpClientFactory;

    public VgmdbApi(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// Gets the configured server URL, falling back to the public instance.
    /// </summary>
    /// <remarks>
    /// Read per request rather than cached, so changing it on the plugin
    /// configuration page takes effect without restarting the server. Any
    /// trailing slash is trimmed because every caller appends an absolute
    /// path, and "host//album/1" would 404.
    /// </remarks>
    private static string RootUrl
    {
        get
        {
            var configured = VgmdbPlugin.Instance?.Configuration?.ServerUrl;
            if (string.IsNullOrWhiteSpace(configured))
            {
                return DefaultRootUrl;
            }

            return configured.Trim().TrimEnd('/');
        }
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
