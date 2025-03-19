namespace VocalSpace.Models.ViewModel.Global
{
    public class UserBarViewModel
    {
        public string? pfp { get; set; }  // 使用者頭像
        public string? Name { get; set; } // 使用者名稱
        public string? Account { get; set; }  // 使用者帳號
        public bool isFollowing { get; set; } // 是否追蹤
        public long target_userId { get; set; } // 目標使用者ID
    }
}
