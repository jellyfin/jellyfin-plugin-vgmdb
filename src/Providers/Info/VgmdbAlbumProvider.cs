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
	public class VgmdbAlbumProvider : IRemoteMetadataProvider<MusicAlbum, AlbumInfo>
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly VgmdbApi _api;

		public VgmdbAlbumProvider(IHttpClientFactory httpClientFactory, IJsonSerializer json)
		{
			_httpClientFactory = httpClientFactory;
			_api = new VgmdbApi(httpClientFactory, json);
		}

		public string Name => "VGMdb";

		public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
		{
			return _httpClientFactory.CreateClient(NamedClient.Default).GetAsync(url);
		}

		public async Task<MusicAlbum> GetAlbumById(int id, CancellationToken cancellationToken)
		{
			var response = await _api.GetAlbumById(id, cancellationToken);

			if (response == null) return null;

			var album = new MusicAlbum
			{
				ProviderIds =
				{
					[VgmdbAlbumExternalId.ExternalId] = response.Id.ToString()
				},
				Name = response.names.GetPreferred()
			};

			//todo better date parsing
			int.TryParse(response.release_date.Split('-')[0], out var productionYear);
			if (productionYear > 0) album.ProductionYear = productionYear;

			var image = new ItemImageInfo
			{
				Path = response.picture_full,
				Type = ImageType.Primary
			};
			album.SetImage(image, 0);

			album.Overview = response.notes;

			if (response.categories != null)
			{
				foreach (var category in response.categories)
				{
					album.AddGenre(category);
				}
			}

			if (response.organizations != null)
			{
				foreach (var organisation in response.organizations)
				{
					album.AddStudio(organisation.names.GetPreferred());
				}
			}

			return album;
		}

		public async Task<int?> GetId(AlbumInfo info, CancellationToken cancellationToken)
		{
			var providedId = info.GetProviderId(VgmdbAlbumExternalId.ExternalId);
			if (providedId != null) return int.Parse(providedId);

			var searchResults = await GetSearchResults(info, cancellationToken);

			foreach (var result in searchResults)
			{
				var id = result.GetProviderId(VgmdbAlbumExternalId.ExternalId);

				if (id != null) return int.Parse(id);
			}

			return null;
		}

		public async Task<MetadataResult<MusicAlbum>> GetMetadata(AlbumInfo info, CancellationToken cancellationToken)
		{
			var id = await GetId(info, cancellationToken);

			if (id != null)
			{
				return new MetadataResult<MusicAlbum>
				{
					Item = await GetAlbumById((int) id, cancellationToken)
				};
			}

			return new MetadataResult<MusicAlbum>();
		}

		public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(AlbumInfo searchInfo, CancellationToken cancellationToken)
		{
			var response = await _api.GetSearchResults(searchInfo.Name, cancellationToken);

			var searchResults = new List<RemoteSearchResult>();
			if (response == null) return null;

			foreach (var albumEntry in response.results.albums)
			{
				var album = await GetAlbumById(albumEntry.Id, cancellationToken);
				var result = new RemoteSearchResult
				{
					ProviderIds = album.ProviderIds,
					Name = album.Name,
					ProductionYear = album.ProductionYear,
					ImageUrl = album.PrimaryImagePath
				};

				searchResults.Add(result);
			}

			return searchResults;
		}
	}
}
