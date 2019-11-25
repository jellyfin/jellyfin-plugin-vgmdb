using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
	public class VgmdbAlbumImageProvider : IRemoteImageProvider
	{
		private readonly IHttpClient _httpClient;
		private readonly IJsonSerializer _json;
		private readonly ILogger _logger;
		private readonly VgmdbApi _api;

		public VgmdbAlbumImageProvider(IHttpClient httpClient, IJsonSerializer json, ILogger logger)
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

		public async Task<IEnumerable<RemoteImageInfo>> GetImages(BaseItem item, CancellationToken cancellationToken)
		{
			var images = new List<RemoteImageInfo>();

			var id = item.GetProviderId(VgmdbAlbumExternalId.KEY);
			if (id != null) //todo use a search to find id
			{
				var album = await _api.GetAlbumById(int.Parse(id), cancellationToken);

				images.Add(new RemoteImageInfo
				{
					Url = album.picture_full,
					ThumbnailUrl = album.picture_small
				});
			}

			return images;
		}

		public IEnumerable<ImageType> GetSupportedImages(BaseItem item)
		{
			return new List<ImageType>
			{
				ImageType.Primary
			};
		}

		public bool Supports(BaseItem item) => item is MusicAlbum;
	}
}