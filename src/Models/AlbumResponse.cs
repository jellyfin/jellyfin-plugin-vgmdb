using System.Collections.Generic;

namespace Jellyfin.Plugin.Vgmdb.Models
{
	public class AlbumResponse
	{
		public LocalizedString names { get; set; }
		public string picture_full { get; set; }
		public string picture_small { get; set; }
		public string picture_thumb { get; set; }
		public string link { get; set; }
		public string notes { get; set; }

		public List<string> categories { get; set; }
		public List<Organisation> organizations { get; set; }

		public int Id => int.Parse(link.Replace("album/", ""));
	}
}