using System.Net;
using System.Net.Http;
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
		private readonly IJsonSerializer _json;
		private const string RootUrl = @"https://vgmdb.info/";

		public VgmdbApi(IHttpClientFactory httpClientFactory, IJsonSerializer json)
		{
			_httpClientFactory = httpClientFactory;
			_json = json;
		}

		public async Task<ArtistResponse> GetArtistById(int id, CancellationToken cancellationToken)
		{
			var httpClient = _httpClientFactory.CreateClient();
			using (var response = await httpClient.GetAsync(RootUrl + "/artist/" + id + "?format=json", cancellationToken).ConfigureAwait(false))
			{
				await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
				return await _json.DeserializeFromStreamAsync<ArtistResponse>(stream).ConfigureAwait(false);
			}
		}

		public async Task<AlbumResponse> GetAlbumById(int id, CancellationToken cancellationToken)
		{
			var httpClient = _httpClientFactory.CreateClient();
			using (var response = await httpClient.GetAsync(RootUrl + "/album/" + id + "?format=json", cancellationToken).ConfigureAwait(false))
			{
				await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
				return await _json.DeserializeFromStreamAsync<AlbumResponse>(stream).ConfigureAwait(false);
			}
		}

		public async Task<SearchResponse> GetSearchResults(string name, CancellationToken cancellationToken)
		{
			var httpClient = _httpClientFactory.CreateClient();
			using (var response = await httpClient.GetAsync(RootUrl + "/search?format=json&q=" + WebUtility.UrlEncode(name), cancellationToken).ConfigureAwait(false))
			{
				await using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
				return await _json.DeserializeFromStreamAsync<SearchResponse>(stream).ConfigureAwait(false);
			}
		}
	}
}
