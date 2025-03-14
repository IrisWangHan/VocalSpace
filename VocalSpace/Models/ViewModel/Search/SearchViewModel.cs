namespace VocalSpace.Models.ViewModel.Search
{
    public class SearchViewModel
    {
        
        public List<SongInfoDTO>? Songs { get; set; }
        public List<ArtistDTO>? Artists { get; set; }
        public List<PlaylistDTO>? Playlists { get; set; }
        public bool IsEmpty { get; set; }    //  IsEmpty = true，代表搜尋沒資料

    }
}
