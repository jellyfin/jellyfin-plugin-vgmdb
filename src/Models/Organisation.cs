namespace Jellyfin.Plugin.Vgmdb.Models
{
	public class Organisation
	{
		public string link { get; set; }
		public LocalizedString names { get; set; }
		public string role { get; set; }

		public int Id => int.Parse(link.Replace("org/", ""));
	}
}