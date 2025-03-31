namespace VocalSpace.Models.ViewModel.Search
{
    public class PlaylistDTO
    {
        public long PlayListId { get; set; }
        public string? Name { get; set; }

        public string? CoverImagePath { get; set; }
        public string? UserName { get; set; }
        public int SongCount { get; set; }
    }
}
