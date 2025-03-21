namespace VocalSpace.Models.ViewModel.Global
{
    public class UserSessionViewModel
    {
        public long UserId { get; set; } // 使用者 ID
        public string Account { get; set; } = ""; // 使用者帳號
        public bool IsLogin { get; set; } // 是否登入
    }
}
