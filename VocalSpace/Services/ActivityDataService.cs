using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Activity;
using VocalSpace.Models.ViewModel.Global;

namespace VocalSpace.Services
{
    public class ActivityDataService
    {
        // EF 的 DbContext
        private readonly VocalSpaceDbContext _context;
        private readonly FileService _fileService;

        // 建構函數 DbContext
        public ActivityDataService(VocalSpaceDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        /// <summary>
        /// 抓取所有資料
        /// </summary>
        public async Task<List<ActivityListViewModel>> GetActivityListData()
        {
            // 查詢活動資料
            var ActivityList = await _context.Activities
                .Include(c => c.Uploader)
                .OrderByDescending(c => c.EventTime)
                .Select(c => new ActivityListViewModel
                {
                    UploaderId = c.UploaderId,
                    UploaderName = c.Uploader.UserName ?? c.Uploader.Account,
                    UploaderAvatarPath = c.Uploader.UsersInfo!.AvatarPath,
                    ActivityId = c.ActivityId,
                    EventCoverPath = c.EventCoverPath,
                    EventTime = c.EventTime,
                    Title = c.Title,
                    Location = c.Location,
                    City = c.City,
                    ApprovalStatus = c.ApprovalStatus,
                    ActivityDescription = c.ActivityDescription
                }).ToListAsync();

            return ActivityList;
        }
    
        /// <summary>
        /// 抓取活動資料
        /// </summary>
        public async Task<List<ActivityListViewModel>> GetActivityListData(long? currentUserId, string keyword, string region, string startDate, string endDate)
        {
            // 查詢活動資料
            var ActivityList = await GetActivityListData();

            // 關鍵字搜尋(忽略大小寫)
            if (!string.IsNullOrEmpty(keyword))
            {
                ActivityList = ActivityList.Where(a => a.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) || a.ActivityDescription.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // 我的活動 => 只顯示自己上傳的活動
            if (currentUserId.HasValue)
            {
                ActivityList = ActivityList.Where(c => c.UploaderId == currentUserId).ToList();
            }

            // 根據地區過濾
            if (region != "all" && !string.IsNullOrEmpty(region))
            {
                // 根據區域來過濾活動列表
                var regionCities = RegionMapping.RegionDict
                    .Where(r => r.Value == region)
                    .Select(r => r.Key) 
                    .ToList();

                ActivityList = ActivityList.Where(a => regionCities.Contains(a.City)).ToList();
            }

            // 根據日期篩選
            if (DateTime.TryParse(startDate, out DateTime start))
            {
                ActivityList = ActivityList.Where(a => a.EventTime >= start).ToList();
            }

            if (DateTime.TryParse(endDate, out DateTime end))
            {
                ActivityList = ActivityList.Where(a => a.EventTime <= end).ToList();
            }

            return ActivityList;
        }

        /// <summary>
        /// 抓取所有資料
        /// </summary>
        public async Task<ActivityInfoViewModel> GetActivityInfoData(int ActivityId,long? CurrentUserid)
        {
            // 查詢當前使用者資訊（如果有登入）
            string? currentAvatar = null;
            string? currentUserAccount = null;
            bool isLogin = CurrentUserid.HasValue;

            if (isLogin)
            {
                var userInfo = await _context.Users
                    .Where(u => u.UserId == CurrentUserid)
                    .Select(u => new { u.Account, Avatar = u.UsersInfo!.AvatarPath })
                    .FirstOrDefaultAsync();

                if (userInfo != null)
                {
                    currentAvatar = userInfo.Avatar;
                    currentUserAccount = userInfo.Account;
                }
            }

            // 查詢活動資料
            var ActivityInfo = await _context.Activities
                .Where(c => c.ActivityId == ActivityId)
                .Select(c => new ActivityInfoViewModel
                {
                    UploaderId = c.UploaderId,
                    UploaderName = c.Uploader.UserName ?? c.Uploader.Account,
                    UploaderAvatarPath = c.Uploader.UsersInfo!.AvatarPath,
                    ActivityId = c.ActivityId,
                    EventCoverPath = c.EventCoverPath,
                    EventTime = c.EventTime,
                    Title = c.Title,
                    Location = c.Location,
                    City = c.City,
                    ApprovalStatus = c.ApprovalStatus,
                    ActivityDescription = c.ActivityDescription,
                    ShareUrl = $"https://w6r821w7-7145.asse.devtunnels.ms/Activity/Info/{c.ActivityId}",
                    Interesteds = new InterestedViewModel
                    {
                        IsInterested = isLogin ? c.Interesteds.Any(i => i.UserId == CurrentUserid) : false,
                        InterestedCount = c.Interesteds.Count,  //.Count為EF提供的屬性，用於計算集合中的資料行數
                    },
                    // 填充 CommentSection 資料
                    CommentSection = new CommentSectionViewModel
                    {
                        IsLogin = isLogin,
                        CurrentAvatar = currentAvatar ?? "",
                        CurrentUserAccount = currentUserAccount ?? "",
                        Comments = new List<CommentViewModel>() // 先空值，改用 AJAX 讀取
                    },
                }).FirstOrDefaultAsync();

            

            return ActivityInfo!;
        }

        /// <summary>
        /// 我也想去按鈕邏輯
        /// </summary>
        public async Task<(bool isInterested, int count)> ToggleInterested(long activityId, long userId)
        {
            var isInterest = await _context.Interesteds
                .FirstOrDefaultAsync(i => i.ActivityId == activityId && i.UserId == userId);

            if (isInterest == null)
            {
                // 如果還沒加入，則新增
                var newInterest = new Interested
                {
                    ActivityId = activityId,
                    UserId = userId,
                    InterestedDate = DateTime.UtcNow
                };
                _context.Interesteds.Add(newInterest);
            }
            else
            {
                // 如果已經加入，則刪除
                _context.Interesteds.Remove(isInterest);
            }

            await _context.SaveChangesAsync();

            // 計算並返回最新的興趣人數
            int interestedCount = await _context.Interesteds.CountAsync(i => i.ActivityId == activityId);

            // 回傳 isInterested 狀態和最新計數
            return (isInterest == null, interestedCount); 
        }

        /// <summary>
        /// 投稿活動表單上傳
        /// </summary>
        public async Task<bool> CreateActivityAsync(ActivityCreateModel model, long userId)
        {
            var activity = new VocalSpace.Models.Activity
            {
                Title = model.Title,
                EventTime = model.EventDate,
                Location = model.Location,
                ActivityDescription = model.ActivityDescription ?? "",
                UploaderId = userId,
            };

            // 如果有封面圖片，處理上傳
            if (model.CoverImage != null)
            {
                string filePath = await _fileService.UploadActivityCover(model.CoverImage);
                activity.EventCoverPath = filePath;
            }

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
