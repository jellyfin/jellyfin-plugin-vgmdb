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
		private readonly VgmdbApi _api;

		public VgmdbArtistProvider(IHttpClient httpClient, IJsonSerializer json, ILogger logger)
		{
			_httpClient = httpClient;
			_json = json;
			_logger = logger;
			_api = new VgmdbApi(httpClient, json, logger);
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
			var response = await _api.GetArtistById(id, cancellationToken);

			if (response != null)
			{
				var artist = new MusicArtist();
				artist.ProviderIds[VgmdbArtistExternalId.KEY] = response.Id.ToString();
				artist.Name = response.name;

				var image = new ItemImageInfo();
				image.Path = response.picture_full;
				image.Type = ImageType.Primary;
				artist.SetImage(image, 0);

				artist.Overview = response.notes;

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
			var response = await _api.GetSearchResults(searchInfo.Name, cancellationToken);

			var searchResults = new List<RemoteSearchResult>();
			if (response != null)
			{
				foreach (var artistEntry in response.results.artists)
				{
					var artist = await GetArtistById(artistEntry.Id, cancellationToken);
					var result = new RemoteSearchResult();
					result.ProviderIds = artist.ProviderIds;
					result.Name = artist.Name;
					result.ImageUrl = artist.PrimaryImagePath;

					searchResults.Add(result);
				}
			}

			return searchResults;
		}
	}
}