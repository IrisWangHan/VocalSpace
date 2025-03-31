namespace VocalSpace.Models.ViewModel.Global
{
    public class AddToPlaylistViewModel
    {
        public long SongId { get; set; }
        public List<PlaylistViewModel> Playlists { get; set; } = new();
    }

    public class PlaylistViewModel
    {
        public long PlayListID { get; set; }
        public string? PlayListName { get; set; }
        public bool SonginPlaylist { get; set; }

        public string? CoverImagePath { get; set; }
    }
}
