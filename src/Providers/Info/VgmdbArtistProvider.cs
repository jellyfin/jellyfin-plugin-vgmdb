using System.Collections.Generic;
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
using MediaBrowser.Model.Serialization;

namespace Jellyfin.Plugin.Vgmdb.Providers.Info
{
	// todo: implement IHasOrder, but find out what it does first
	public class VgmdbArtistProvider : IRemoteMetadataProvider<MusicArtist, ArtistInfo>
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly VgmdbApi _api;

		public VgmdbArtistProvider(IHttpClientFactory httpClientFactory, IJsonSerializer json)
		{
			_httpClientFactory = httpClientFactory;
			_api = new VgmdbApi(httpClientFactory, json);
		}

		public string Name => "VGMdb";

		public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
		{
			return _httpClientFactory.CreateClient().GetAsync(url);
		}

		public async Task<MusicArtist> GetArtistById(int id, CancellationToken cancellationToken)
		{
			var response = await _api.GetArtistById(id, cancellationToken);

			if (response == null) return null;

			var artist = new MusicArtist
			{
				ProviderIds =
				{
					[VgmdbArtistExternalId.ExternalId] = response.Id.ToString()
				},
				Name = response.name
			};

			var image = new ItemImageInfo
			{
				Path = response.picture_full,
				Type = ImageType.Primary
			};
			artist.SetImage(image, 0);

			artist.Overview = response.notes;

			return artist;
		}

		public async Task<int?> GetId(ArtistInfo info, CancellationToken cancellationToken)
		{
			var providedId = info.GetProviderId(VgmdbArtistExternalId.ExternalId);
			if (providedId != null) return int.Parse(providedId);

			var searchResults = await GetSearchResults(info, cancellationToken);

			foreach (var result in searchResults)
			{
				var id = result.GetProviderId(VgmdbArtistExternalId.ExternalId);

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
					Item = await GetArtistById((int) id, cancellationToken)
				};
			}

			return new MetadataResult<MusicArtist>();
		}

		public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(ArtistInfo searchInfo, CancellationToken cancellationToken)
		{
			var response = await _api.GetSearchResults(searchInfo.Name, cancellationToken);

			var searchResults = new List<RemoteSearchResult>();
			if (response == null) return null;

			foreach (var artistEntry in response.results.artists)
			{
				var artist = await GetArtistById(artistEntry.Id, cancellationToken);
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
}
