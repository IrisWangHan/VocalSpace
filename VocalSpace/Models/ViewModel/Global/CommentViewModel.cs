namespace VocalSpace.Models.ViewModel.Global
{
    //單個留言
    public class CommentViewModel
    {
        public string TargetType { get; set; } = null!;   // "Song" 或 "Activity"
        public long CommentId { get; set; }                     //可能為SongCommentId,ActivityCommentId
        public long TargetId { get; set; }                           //可能為SongID,ActivityID

        public string Account { get; set; } = null!;
        public string CurrentUserAccount { get; set; } = null!;
        public string UserName { get; set; } = null!;

        public long UserId { get; set; }

        public string Avatar { get; set; } = null!;

        public string Comment { get; set; } = null!;

        public DateTime CommentTime { get; set; }
    }
    //控制整個留言區的顯示，包括留言框是否顯示
    public class CommentSectionViewModel
    {
        public bool IsLogin { get; set; }
        public string CurrentAvatar { get; set; } = null!;

        public string CurrentUserAccount { get; set; } = null!;

        public List<CommentViewModel> Comments { get; set; } = null!;
    }
    //接收AJAX留言
    public class CommentRequestViewModel
    {
        public long TargetId { get; set; } // 目標 ID (SongId 或 ActivityId)
        public string TargetType { get; set; } = null!; // "Song" 或 "Activity"
        public string Comment { get; set; } = null!;
    }
}
