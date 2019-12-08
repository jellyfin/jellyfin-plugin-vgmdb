namespace Jellyfin.Plugin.Vgmdb.Models
{
	public class ArtistResponse
	{
		public string name { get; set; }
		public string picture_full { get; set; }
		public string picture_small { get; set; }
		public string link { get; set; }
		public string notes { get; set; }

		public int Id => int.Parse(link.Replace("artist/", ""));
	}
}