namespace VocalSpace.Models.ViewModel.Search
{
    public class ArtistDTO
    {
        public long UserId { get; set; }
        public string? AvatarPath { get; set; }
        public string? UserName { get; set; }
        public bool isFollowing { get; set; } // 是否追蹤
    }
}
