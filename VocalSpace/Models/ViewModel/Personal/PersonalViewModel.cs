namespace VocalSpace.Models.ViewModel.Personal
{
    public class PersonalViewModel
    {
        public long CurrentUserId { get; set; }
        // from user
        public long UserId { get; set; }                           

        public string? UserName { get; set; }

        public string Account { get; set; } = null!;

        public DateTime CreateTime { get; set; }

        // from UserFollow
        public long FollowedUserId { get; set; }
        public int FollowCount { get; set; }
        public bool isFollowing { get; set; } 
        // from UsersInfo
        public string? BannerImagePath { get; set; }

        public string AvatarPath { get; set; } = null!;
        public string? PersonalIntroduction { get; set; }



    }
}
