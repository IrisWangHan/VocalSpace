namespace VocalSpace.Models.ViewModel.Account
{
    public class UserSettingViewModel
    {
        public long UserId { get; set; }
        public string? UserName { get; set; }
        public string? Birthday { get; set; }

        public string? PersonalIntroduction { get; set; }
        public string? BannerImagePath { get; set; }

        public string? AvatarPath { get; set; }
        public string? Password { get; set; }

        public string? Email { get; set; }
        public int totalSpent { get; set; }
        public int totalIncome { get; set; }
    }
}
