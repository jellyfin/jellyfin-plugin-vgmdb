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
	public class VgmdbAlbumProvider : IRemoteMetadataProvider<MusicAlbum, AlbumInfo>
	{
		private readonly IHttpClient _httpClient;
		private readonly IJsonSerializer _json;
		private readonly ILogger _logger;
		private readonly VgmdbApi _api;

		public VgmdbAlbumProvider(IHttpClient httpClient, IJsonSerializer json, ILogger logger)
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

		public async Task<MusicAlbum> GetAlbumById(int id, CancellationToken cancellationToken)
		{
			var response = await _api.GetAlbumById(id, cancellationToken);

			if (response != null)
			{
				var album = new MusicAlbum();
				album.ProviderIds[VgmdbAlbumExternalId.KEY] = response.Id.ToString();
				album.Name = response.names.GetPreferred();

				var image = new ItemImageInfo();
				image.Path = response.picture_full;
				image.Type = ImageType.Primary;
				album.SetImage(image, 0);

				album.Overview = response.notes;

				foreach (var category in response.categories)
				{
					album.AddGenre(category);
				}

				foreach (var organisation in response.organisations)
				{
					album.AddStudio(organisation.names.GetPreferred());
				}

				return album;
			}

			return null;
		}

		public async Task<int?> GetId(AlbumInfo info, CancellationToken cancellationToken)
		{
			var providedId = info.GetProviderId(VgmdbAlbumExternalId.KEY);
			if (providedId != null) return int.Parse(providedId);

			var searchResults = await GetSearchResults(info, cancellationToken);

			foreach (var result in searchResults)
			{
				var id = result.GetProviderId(VgmdbAlbumExternalId.KEY);

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
					Item = await GetAlbumById((int)id, cancellationToken)
				};
			}

			return new MetadataResult<MusicAlbum>();
		}

		public async Task<IEnumerable<RemoteSearchResult>> GetSearchResults(AlbumInfo searchInfo, CancellationToken cancellationToken)
		{
			var response = await _api.GetSearchResults(searchInfo.Name, cancellationToken);

			var searchResults = new List<RemoteSearchResult>();
			if (response != null)
			{
				foreach (var albumEntry in response.results.albums)
				{
					var album = await GetAlbumById(albumEntry.Id, cancellationToken);
					var result = new RemoteSearchResult();
					result.ProviderIds = album.ProviderIds;
					result.Name = album.Name;
					result.ImageUrl = album.PrimaryImagePath;

					searchResults.Add(result);
				}
			}

			return searchResults;
		}
	}
}