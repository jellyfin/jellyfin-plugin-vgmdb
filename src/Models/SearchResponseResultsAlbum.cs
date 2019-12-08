namespace Jellyfin.Plugin.Vgmdb.Models
{
	public class SearchResponseResultsAlbum
	{
		public string catalog { get; set; }
		public string link { get; set; }
		public string release_date { get; set; }
		public LocalizedString titles { get; set; }

		public int Id => int.Parse(link.Replace("album/", ""));
	}
}