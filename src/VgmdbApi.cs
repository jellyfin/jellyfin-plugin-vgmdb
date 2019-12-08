using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Vgmdb.Models;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Vgmdb
{
	public class VgmdbApi
	{
		private readonly IHttpClient _httpClient;
		private readonly IJsonSerializer _json;
		private const string RootUrl = @"https://vgmdb.info/";

		public VgmdbApi(IHttpClient httpClient, IJsonSerializer json)
		{
			_httpClient = httpClient;
			_json = json;
		}

		public async Task<ArtistResponse> GetArtistById(int id, CancellationToken cancellationToken)
		{
			using (var response = await _httpClient.Get(new HttpRequestOptions
			{
				Url = RootUrl + "/artist/" + id + "?format=json",
				CancellationToken = cancellationToken
			}).ConfigureAwait(false))
			{
				return _json.DeserializeFromStream<ArtistResponse>(response);
			}
		}

		public async Task<AlbumResponse> GetAlbumById(int id, CancellationToken cancellationToken)
		{
			using (var response = await _httpClient.Get(new HttpRequestOptions
			{
				Url = RootUrl + "/album/" + id + "?format=json",
				CancellationToken = cancellationToken
			}).ConfigureAwait(false))
			{
				return _json.DeserializeFromStream<AlbumResponse>(response);
			}
		}

		public async Task<SearchResponse> GetSearchResults(string name, CancellationToken cancellationToken)
		{
			using (var response = await _httpClient.Get(new HttpRequestOptions
			{
				Url = RootUrl + "/search?format=json&q=" + WebUtility.UrlEncode(name),
				CancellationToken = cancellationToken
			}).ConfigureAwait(false))
			{
				return _json.DeserializeFromStream<SearchResponse>(response);
			}
		}
	}
}