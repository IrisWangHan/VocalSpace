namespace VocalSpace.Models.SearchViewModel
{
    public class SearchViewModel
    {
        public List<SongInfoDTO>? Songs { get; set; }
        public List<ArtistDTO>? Artists { get; set; } 
        public List<PlaylistDTO>? Playlists { get; set; } 
    }
}
