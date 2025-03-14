namespace VocalSpace.Models.ViewModel.Search
{
    public class SongInfoDTO
    {
        public string? SongPath { get; set; }

        public string? CoverPhotoPath { get; set; }

        public string? SongName { get; set; }
        public string? UserName { get; set; }
        public int? LikeCount { get; set; }
        public byte SongCategoryId { get; set; }

    }
}
