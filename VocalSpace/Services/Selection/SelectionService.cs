using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.Test.Selection;


namespace VocalSpace.Services.Selection
{
    public class SelectionService
    {
        //第二步: 建立Service 處理業務邏輯
        private readonly VocalSpaceDbContext _context;

        public SelectionService(VocalSpaceDbContext context)
        {
            _context = context;
        }

        public async Task<List<SelectionListDTO>?> GetSelectionsAsync()
        {
            try
            {
                return await _context.Selections
                .Select(s => new SelectionListDTO
                {
                    Title = s.Title,
                    SelectionCoverPath = s.SelectionCoverPath,
                    JoinState = GetState(s.StartDate, s.EndDate),
                    VoteState = GetState(s.VotingStartDate, s.VotingEndDate),
                    SelectionId = s.SelectionId,
                })
                .ToListAsync();
            }
            catch (Exception ex)
            {
                // 錯誤處理，可以記錄錯誤或回傳自定義錯誤訊息
                // Log.Error(ex, "Error fetching selection details");
                return null;
            }
        }
        public async Task<SelectionListDTO?> GetEventDescriptionAsync(int? id)
        {
            try
            {
                return await _context.Selections
                    .Where(s => s.SelectionId == id)
                    .Select(s => new SelectionListDTO
                    {
                        Title = s.Title,
                        SelectionCoverPath = s.SelectionCoverPath,
                        JoinState = GetState(s.StartDate, s.EndDate),
                        VoteState = GetState(s.VotingStartDate, s.VotingEndDate),
                        StartDate = s.StartDate ?? DateTime.MinValue,
                        EndDate = s.EndDate ?? DateTime.MinValue,
                        VotingStartDate = s.VotingStartDate ?? DateTime.MinValue,
                        VotingEndDate = s.VotingEndDate ?? DateTime.MinValue,
                        SelectionId = s.SelectionId,
                        Description = s.Description

                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // 錯誤處理，可以記錄錯誤或回傳自定義錯誤訊息
                // Log.Error(ex, "Error fetching selection details");
                return null;
            }
        }
        private static string GetState(DateTime? startTime, DateTime? endTime)
        {
            DateTime now = DateTime.Now;
            if (now < startTime)
            {
                return "未開放";
            }
            else if (now >= startTime && now <= endTime)
            {
                return "開放中";
            }
            else
            {
                return "已結束";
            }
        }


        //public async Task<SelectionListDTO?> GetWorks(int? id)
        //{
        //    try
        //    {
        //        //var user = "johndoe";
        //        //var condition = "some condition";  // 假設你有某個查詢條件

        //        //var blogs = await _context.Blogs
        //        //    .FromSql($"SELECT A.Id, A.Name, B.Description " +
        //        //             "FROM A " +
        //        //             "INNER JOIN B ON A.UserId = B.UserId " +
        //        //             "WHERE A.UserName = @user AND A.SomeCondition = @condition",
        //        //             new SqlParameter("@user", user),
        //        //             new SqlParameter("@condition", condition))
        //        //    .ToListAsync();
        //        //return new SelectionListDTO;
        //    }
        //    catch (Exception ex)
        //    {
        //        // 錯誤處理，可以記錄錯誤或回傳自定義錯誤訊息
        //        // Log.Error(ex, "Error fetching selection details");
        //        return null;
        //    }
        //}
    }
}
