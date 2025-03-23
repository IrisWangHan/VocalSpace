using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Activity;

namespace VocalSpace.Services
{
    public class ActivityDataService
    {
        // EF 的 DbContext
        private readonly VocalSpaceDbContext _context;

        // 建構函數 DbContext
        public ActivityDataService(VocalSpaceDbContext context)
        {
            _context = context;
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
        public async Task<ActivityInfoViewModel> GetActivityInfoData(int ActivityId)
        {
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
                }).FirstOrDefaultAsync();


            return ActivityInfo!;
        }
    }
}
