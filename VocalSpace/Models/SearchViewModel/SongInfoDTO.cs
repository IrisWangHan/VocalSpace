namespace VocalSpace.Models.SearchViewModel
{
    public class SongInfoDTO
    {
        public string? SongPath { get; set; }

        public string? CoverPhotoPath { get; set; }

        public string? SongName { get; set; }
        public string? UserName { get; set; }

        public int? LikeCount { get; set; }
    }
}
