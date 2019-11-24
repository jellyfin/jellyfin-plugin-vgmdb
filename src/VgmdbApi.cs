using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Vgmdb.Models;
using MediaBrowser.Common.Net;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Vgmdb
{
	public class VgmdbApi
	{
		private readonly IHttpClient _httpClient;
		private readonly IJsonSerializer _json;
		private readonly ILogger _logger;
		internal const string RootUrl = @"https://vgmdb.info/";

		public VgmdbApi(IHttpClient httpClient, IJsonSerializer json, ILogger logger)
		{
			_httpClient = httpClient;
			_json = json;
			_logger = logger;
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
			var results = new List<RemoteSearchResult>();

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