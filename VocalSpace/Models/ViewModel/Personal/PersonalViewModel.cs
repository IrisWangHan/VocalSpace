namespace VocalSpace.Models.ViewModel.Personal
{
    public class PersonalViewModel
    {
        // from user
        public long UserId { get; set; }

        public string? UserName { get; set; }

        public string Account { get; set; } = null!;

        public DateTime CreateTime { get; set; }
        // from UserFollow
        public long FollowedUserId { get; set; }
        // from UsersInfo
        public string? BannerImagePath { get; set; }

        public string AvatarPath { get; set; } = null!;

    }
}
