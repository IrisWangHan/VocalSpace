using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Global;
namespace VocalSpace.Services
{
    public class CommentDataService
    {
        private readonly VocalSpaceDbContext _context;

        // 建構函數 DbContext
        public CommentDataService(VocalSpaceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 使用者留言輸入區邏輯
        /// </summary>


        /// <summary>
        /// 使用者留言列表邏輯，整合不同類型留言的查詢方式。
        /// </summary>
        public async Task<List<CommentViewModel>> GetCommentListData(string account, long targetId, string commentype)
        {
            // 先宣告一個空的查詢，讓後續的查詢結果可以丟進去
            var query = Enumerable.Empty<CommentViewModel>().AsQueryable();

            // 針對留言類型不同，查詢不同的留言

            if (commentype == "Song")
            {
                // 針對歌曲的留言查詢
                query = _context.SongComments
                    .Where(c => c.SongId == targetId) // 抓取指定SongId的留言
                    .OrderByDescending(c => c.CommentTime) // 依照留言時間排序
                    .Include(c => c.User) // 確保 User 被載入
                    .ThenInclude(u => u.UsersInfo) // 確保 UsersInfo 被載入
                    .Select(c => new CommentViewModel
                    {
                        TargetType = "Song", // 留言類型
                        CommentId = c.SongCommentId, // 留言 ID
                        TargetId = c.SongId, // 歌曲 ID
                        Account = c.User.Account, // 使用者帳號
                        CurrentUserAccount = account ?? string.Empty, // 目前登入使用者帳號
                        UserName = c.User.UserName ?? "", // 使用者名稱，若為 null，設為空字串
                        Avatar = c.User.UsersInfo!.AvatarPath, // 使用者頭像，若為 null，設為空字串
                        Comment = c.Comment, // 留言內容
                        CommentTime = c.CommentTime // 留言時間
                    });
            }
            else if (commentype == "Activity")
            {
                // 針對活動的留言查詢
                query = _context.ActivityComments
                    .Where(c => c.ActivityId == targetId) // 抓取指定ActivityId的留言
                    .OrderByDescending(c => c.CommentTime) // 依照留言時間排序
                    .Include(c => c.User) // 確保 User 被載入
                    .ThenInclude(u => u.UsersInfo) // 確保 UsersInfo 被載入
                    .Select(c => new CommentViewModel
                    {
                        TargetType = "Activity", // 留言類型
                        CommentId = c.ActivityCommentId, // 留言 ID
                        TargetId = c.ActivityId, // 活動 ID
                        Account = c.User.Account, // 使用者帳號
                        CurrentUserAccount = account ?? string.Empty, // 目前登入使用者帳號
                        UserName = c.User.UserName ?? "", // 使用者名稱，若為 null，設為空字串
                        Avatar = c.User.UsersInfo!.AvatarPath, // 使用者頭像，若為 null，設為空字串
                        Comment = c.Comment, // 留言內容
                        CommentTime = c.CommentTime // 留言時間
                    });
            }
            else
            {
                // 如果commentype無效，可以選擇拋出異常或回傳空
                throw new ArgumentException("Invalid comment type", nameof(commentype));
            }

            return await query.ToListAsync();
        }
    }
}
