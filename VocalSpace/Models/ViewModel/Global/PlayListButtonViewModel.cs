namespace VocalSpace.Models.ViewModel.Global
{
    public class PlayListButtonViewModel
    {
        public long PlayListId { get; set; }
        public long SongId { get; set; }
        public string Name { get; set; }
        public bool isliked { get; set; }
        //  public string PlayListName { get; set; } = null!;
        //public bool SonginPlaylist { get; set; }
        public int SongCount { get; set; }
    }
}
