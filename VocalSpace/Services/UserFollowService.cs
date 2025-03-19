using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;

namespace VocalSpace.Services
{
    //此Service負責處理使用者追蹤，然後所有Controller都可呼叫此Service
    public class UserFollowService
    {
        // EF 的 DbContext
        private readonly VocalSpaceDbContext _context;

        // 建構函數 DbContext
        public UserFollowService(VocalSpaceDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 先檢查是否已追蹤
        /// </summary>
        public async Task<bool> IsFollowingAsync(long userId, long followedUserId)
        {
            return await _context.UserFollows
                .AnyAsync(f => f.UserId == userId && f.FollowedUserId == followedUserId);
        }
        /// <summary>
        /// 追蹤 and 取消追蹤
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="followedUserId"></param>
        /// <returns></returns>
        public async Task<(bool isSuccess, bool isFollowing)> ToggleFollowAsync(long userId, long followedUserId)
        {
            //  不能追蹤自己
            if (userId == followedUserId)
                return (false, false);

            // 先查詢是否已追蹤
            var follow = await _context.UserFollows
                   .FirstOrDefaultAsync(f => f.UserId == userId && f.FollowedUserId == followedUserId);


            if (follow == null)
            {
                // 執行追蹤
                _context.UserFollows.Add(new UserFollow
                {
                    UserId = userId,
                    FollowedUserId = followedUserId,
                    FollowTime = DateTime.Now
                });
            }
            else
            {
                // 取消追蹤
                _context.UserFollows.Remove(follow);
            }
            }

            try //try catch 阻攔錯誤崩潰
            {
                await _context.SaveChangesAsync();
                return (true, follow == null); // 成功執行，回傳是否現在有追蹤
            }
            catch (Exception ex)
            {
                // 記錄錯誤（可以寫入 log）
                Console.WriteLine($"[Error] 追蹤操作失敗: {ex.Message}");
                return (false, false); // 失敗時回傳 false
            }
        }

    }
}
