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
            if (userId == followedUserId)
                return (false, false); //  不能追蹤自己

            // **先查詢是否已追蹤**
            bool isFollowing = await IsFollowingAsync(userId, followedUserId);

            if (!isFollowing)
            {
                // **執行追蹤**
                _context.UserFollows.Add(new UserFollow
                {
                    UserId = userId,
                    FollowedUserId = followedUserId,
                    FollowTime = DateTime.UtcNow
                });
            }
            else
            {
                // **執行取消追蹤**
                var follow = await _context.UserFollows
                    .FirstOrDefaultAsync(f => f.UserId == userId && f.FollowedUserId == followedUserId);

                if (follow != null)
                {
                    _context.UserFollows.Remove(follow);
                }
            }

            await _context.SaveChangesAsync();
            return (true, !isFollowing); // **成功執行，回傳是否現在有追蹤**
        }

    }
}
