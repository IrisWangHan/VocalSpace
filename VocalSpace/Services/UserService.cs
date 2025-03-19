using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Global;

namespace VocalSpace.Services
{
    //此Service負責處理使用者相關邏輯，然後所有Controller都可呼叫此Service
    public class UserService
    {
        // EF 的 DbContext
        private readonly VocalSpaceDbContext _context;

        // 建構函數 DbContext
        public UserService(VocalSpaceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 檢查當前使用者是否有追蹤目標使用者
        /// </summary>
        public async Task<bool> IsFollowingAsync(long userId, long followedUserId)
        {
            //此程式將會回傳是否有追蹤目標使用者，若有則回傳true，若無則回傳false
            return await _context.UserFollows
                .AnyAsync(f => f.UserId == userId && f.FollowedUserId == followedUserId);
        }

        /// <summary>
        /// 追蹤 and 取消追蹤，回傳兩個bool值，是否，是否有追蹤目標使用者
        /// </summary>
        public async Task<(bool isSuccess, bool isFollowing)> ToggleFollowAsync(long userId, long followedUserId)
        {
            // 防止自己追蹤自己
            if (userId == followedUserId)
                return (false, false);

            try
            {

                //檢查是否有追蹤目標使用者，以及是否追蹤自己，調用上面的方法
                bool isFollowing = await IsFollowingAsync(userId, followedUserId);

                //如果回傳true(已追蹤)
                if (isFollowing)
                {
                    //表示已追蹤 -> 取消追蹤(刪除userfollows中的該資料)
                    await _context.UserFollows
                        .Where(f => f.UserId == userId && f.FollowedUserId == followedUserId)
                        .ExecuteDeleteAsync(); //ExecuteDeleteAsync 批次刪除資料 如果不使用此方法，則需使用Remove方法
                    return (true, false);
                }
                else
                {
                    //表示未追蹤 -> 追蹤(新增userfollows中的該資料)
                    _context.UserFollows.Add(new UserFollow
                    {
                        UserId = userId,
                        FollowedUserId = followedUserId
                    });
                    await _context.SaveChangesAsync();
                    return (true, true);
                }
            }
            //如果有例外發生，回傳false
            catch (Exception ex)
            {
                Console.WriteLine($"ToggleFollowAsync 執行時發生錯誤: {ex.Message}");
                return (false, false);
            }
        }

        /// <summary>
        /// 取得 UserBarViewModel，long? currentUserId 確保訪客也能看到追蹤按鈕
        /// </summary>
        public async Task<UserBarViewModel?> GetUserBarData(long? currentUserId, long targetUserId)
        {
            try { 
            var user = await _context.Users
                .Include(u => u.UsersInfo)
                .Where(u => u.UserId == targetUserId)
                .Select(u => new UserBarViewModel
                {
                    target_userId = u.UserId,
                    Name = u.UserName,
                    Account = u.Account,
                    pfp = u.UsersInfo!.AvatarPath,
                    isFollowing = currentUserId.HasValue && currentUserId!= 0  //如果使用者未登入，預設為false
                        ? _context.UserFollows.Any(f => f.UserId == currentUserId && f.FollowedUserId == targetUserId)
                        : false
                })
                .FirstOrDefaultAsync();

            return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUserBarData 執行時發生錯誤: {ex.Message}");
                return null;
            }
        }
    }
}

