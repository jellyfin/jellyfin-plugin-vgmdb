using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Jellyfin.Plugin.Vgmdb.Models;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.Vgmdb.Providers
{
	// todo: implement IHasOrder, but find out what it does first
	public class VgmdbArtistProvider : IRemoteMetadataProvider<MusicArtist, ArtistInfo>
	{
		private readonly IHttpClient _httpClient;
		private readonly IJsonSerializer _json;
		private readonly ILogger _logger;
		internal const string RootUrl = @"https://vgmdb.info/";

		public VgmdbArtistProvider(IHttpClient httpClient, IJsonSerializer json, ILogger logger)
		{
			_httpClient = httpClient;
			_json = json;
			_logger = logger;
		}

		public string Name => "VGMdb";

		public Task<HttpResponseInfo> GetImageResponse(string url, CancellationToken cancellationToken)
		{
			return _httpClient.GetResponse(new HttpRequestOptions
			{
				Url = url,
				CancellationToken = cancellationToken
			});
		}

		public async Task<MusicArtist> GetArtistById(int id, CancellationToken cancellationToken)
		{
			ArtistResponse result;

			using (var response = await _httpClient.Get(new HttpRequestOptions
			{
				Url = RootUrl + "/artist/" + id + "?format=json",
				CancellationToken = cancellationToken
			}).ConfigureAwait(false))
			{
				result = _json.DeserializeFromStream<ArtistResponse>(response);
			}

			if (result != null)
			{
				var artist = new MusicArtist();
				artist.ProviderIds[VgmdbArtistExternalId.KEY] = result.Id.ToString();
				artist.Name = result.name;

				var image = new ItemImageInfo();
				image.Path = result.picture_full;
				image.Type = ImageType.Primary;
				artist.SetImage(image, 0);

				artist.Overview = result.notes;

				return artist;
			}

			return null;
		}

		public async Task<int?> GetId(ArtistInfo info, CancellationToken cancellationToken)
		{
			var providedId = info.GetProviderId(VgmdbArtistExternalId.KEY);
			if (providedId != null) return int.Parse(providedId);

			var searchResults = await GetSearchResults(info, cancellationToken);

			foreach (var result in searchResults)
			{
				var id = result.GetProviderId(VgmdbArtistExternalId.KEY);

				if (id != null) return int.Parse(id);
			}

			return null;
		}

		public async Task<MetadataResult<MusicArtist>> GetMetadata(ArtistInfo info, CancellationToken cancellationToken)
		{
			var id = await GetId(info, cancellationToken);

			if (id != null)
			{
				return new MetadataResult<MusicArtist>
				{
					Item = await GetArtistById((int)id, cancellationToken)
				};
			}

			return new MetadataResult<MusicArtist>();
		}

		public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(ArtistInfo searchInfo, CancellationToken cancellationToken)
		{
			var results = new List<RemoteSearchResult>();

			using (var response = await _httpClient.Get(new HttpRequestOptions
			{
				Url = RootUrl + "/search/artists?format=json&q=" + WebUtility.UrlEncode(searchInfo.Name),
				CancellationToken = cancellationToken
			}).ConfigureAwait(false))
			{
				var json = _json.DeserializeFromStream<SearchResponse>(response);
				foreach (var artistEntry in json.results.artists)
				{
					var artist = await GetArtistById(artistEntry.Id, cancellationToken);
					var result = new RemoteSearchResult();
					result.ProviderIds = artist.ProviderIds;
					result.Name = artist.Name;
					result.ImageUrl = artist.PrimaryImagePath;

					results.Add(result);
				}
			}

			return results;
		}
	}
}