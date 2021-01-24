using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Vgmdb.Models;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Vgmdb
{
	public class VgmdbApi
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private const string RootUrl = @"https://vgmdb.info/";

		public VgmdbApi(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		public async Task<ArtistResponse> GetArtistById(int id, CancellationToken cancellationToken)
		{
			var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
			using (var response = await httpClient.GetAsync(RootUrl + "/artist/" + id + "?format=json", cancellationToken).ConfigureAwait(false))
			{
				await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
				return await JsonSerializer.DeserializeAsync<ArtistResponse>(stream).ConfigureAwait(false);
			}
		}

		public async Task<AlbumResponse> GetAlbumById(int id, CancellationToken cancellationToken)
		{
			var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
			using (var response = await httpClient.GetAsync(RootUrl + "/album/" + id + "?format=json", cancellationToken).ConfigureAwait(false))
			{
				await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
				return await JsonSerializer.DeserializeAsync<AlbumResponse>(stream).ConfigureAwait(false);
			}
		}

		public async Task<SearchResponse> GetSearchResults(string name, CancellationToken cancellationToken)
		{
			var httpClient = _httpClientFactory.CreateClient(NamedClient.Default);
			using (var response = await httpClient.GetAsync(RootUrl + "/search?format=json&q=" + WebUtility.UrlEncode(name), cancellationToken).ConfigureAwait(false))
			{
				await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
				return await JsonSerializer.DeserializeAsync<SearchResponse>(stream).ConfigureAwait(false);
			}
		}
	}
}
