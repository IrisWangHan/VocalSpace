namespace VocalSpace.Models.ViewModel.Search
{
    public class SongInfoDTO
    {
        public long SongId { get; set; }
        public string? SongPath { get; set; }

        public string? CoverPhotoPath { get; set; }

        public string? SongName { get; set; }
        public string? UserName { get; set; }

        public long UserId { get; set; }
        public int? LikeCount { get; set; }
        public byte SongCategoryId { get; set; }

        public DateTime CreateTime { get; set; }

        public int PlayCount { get; set; }
    }
}
