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
        /// 使用者留言列表邏輯，整合不同類型留言的查詢方式，並將查詢結果丟到CommentViewModel。
        /// </summary>
        public async Task<List<CommentViewModel>> GetCommentListData(string account, long targetId, string commentype)
        {
            // 先宣告一個空的查詢，讓後續的查詢結果可以丟進去
            var query = Enumerable.Empty<CommentViewModel>().AsQueryable();

            // 針對留言類型不同，查詢不同的留言
            switch (commentype)
            {
                case "Song":
                    // 針對歌曲的留言查詢
                    query = _context.SongComments
                        .Where(c => c.SongId == targetId) // 抓取指定SongId的留言
                        .OrderByDescending(c => c.CommentTime)
                        .Include(c => c.User)
                        .ThenInclude(u => u.UsersInfo)
                        .Select(c => new CommentViewModel
                        {
                            TargetType = "Song", // 留言類型
                            CommentId = c.SongCommentId,
                            TargetId = c.SongId,
                            Account = c.User.Account,
                            CurrentUserAccount = account ?? string.Empty,
                            UserName = c.User.UserName ?? "",
                            Avatar = c.User.UsersInfo!.AvatarPath,
                            Comment = c.Comment,
                            CommentTime = c.CommentTime
                        });
                    break;

                case "Activity":
                    // 針對活動的留言查詢
                    query = _context.ActivityComments
                        .Where(c => c.ActivityId == targetId) // 抓取指定ActivityId的留言
                        .OrderByDescending(c => c.CommentTime)
                        .Include(c => c.User)
                        .ThenInclude(u => u.UsersInfo)
                        .Select(c => new CommentViewModel
                        {
                            TargetType = "Activity", // 留言類型
                            CommentId = c.ActivityCommentId,
                            TargetId = c.ActivityId,
                            Account = c.User.Account,
                            CurrentUserAccount = account ?? string.Empty,
                            UserName = c.User.UserName ?? "",
                            Avatar = c.User.UsersInfo!.AvatarPath,
                            Comment = c.Comment,
                            CommentTime = c.CommentTime
                        });
                    break;
                //要是commentype是未宣告的，直接回傳錯誤
                default:
                    //Service層 無法直接Return HTTP錯誤碼 因此我直接丟ArgumentException
                    throw new ArgumentException("不支援的留言類型！");
            }
            return await query.ToListAsync();
        }

        /// <summary>
        /// 上傳留言邏輯，統計完資料直接新增到資料庫，並整合不同類型留言的上傳方式。
        /// </summary>
        public async Task<bool> PostCommentAsync(string account, long targetId, string targetType, string comment)
        {
            // 在Service統一處理錯誤訊息，防止Controller端代碼重複

            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new ArgumentException("留言內容不能為空！");
            }

            if (string.IsNullOrWhiteSpace(account))
            {
                throw new UnauthorizedAccessException("請先登入！");
            }

            var user = await _context.Users
                .Include(u => u.UsersInfo)
                .FirstOrDefaultAsync(u => u.Account == account);

            if (user == null)
            {
                throw new KeyNotFoundException("使用者不存在！");
            }

            //針對留言類型跑不同的邏輯
            switch (targetType)
            {
                case "Song":
                    var songComment = new SongComment
                    {
                        SongId = targetId,
                        UserId = user.UserId,
                        Comment = comment,
                        CommentTime = DateTime.Now
                    };

                    _context.SongComments.Add(songComment);
                    break;

                case "Activity":
                    var activityComment = new ActivityComment
                    {
                        ActivityId = targetId,
                        UserId = user.UserId,
                        Comment = comment,
                        CommentTime = DateTime.Now
                    };

                    _context.ActivityComments.Add(activityComment);
                    break;

                default:
                    throw new ArgumentException("不支援的留言類型！");
            }

            // 儲存更改
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 刪除留言邏輯，指定留言ID並判斷留言類型是否正確，再做刪除。
        /// </summary>
        public async Task<bool> DeleteCommentAsync(string account, int commentId, string targetType)
        {
            //判斷登入
            if (string.IsNullOrEmpty(account))
            {
                throw new UnauthorizedAccessException("請先登入！");
            }
            //依照留言類型套用不同查詢
            switch (targetType)
            {
                //歌曲
                case "Song":
                    var songComment = await _context.SongComments
                        .Include(c => c.User)
                        .FirstOrDefaultAsync(c => c.SongCommentId == commentId);

                    if (songComment == null)
                    {
                        throw new KeyNotFoundException("留言不存在！");
                    }

                    if (songComment.User.Account != account)
                    {
                        throw new UnauthorizedAccessException("您無權刪除此留言！");
                    }
                    _context.SongComments.Remove(songComment);
                    break;
                //活動
                case "Activity":
                    var activityComment = await _context.ActivityComments
                        .Include(c => c.User)
                        .FirstOrDefaultAsync(c => c.ActivityCommentId == commentId);

                    if (activityComment == null)
                    {
                        throw new KeyNotFoundException("留言不存在！");
                    }

                    if (activityComment.User.Account != account)
                    {
                        throw new UnauthorizedAccessException("您無權刪除此留言！");
                    }
                    _context.ActivityComments.Remove(activityComment);
                    break;
                //未宣告
                default:
                    throw new ArgumentException("不支援的留言類型！");
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
