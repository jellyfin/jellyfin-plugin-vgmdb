using System.Collections.Generic;

namespace Jellyfin.Plugin.Vgmdb.Models
{
	public class SearchResponseResultsArtist
	{
		public List<string> aliases { get; set; }
		public string link { get; set; }
		public LocalizedString names { get; set; }

		public int Id => int.Parse(link.Replace("artist/", ""));
	}
}