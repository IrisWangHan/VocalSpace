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


        public async Task<SelectionListDTO?> GetWorks(int? id)
        {
            try
            {
                var query = from A in _context.SelectionDetails
                            join B in _context.Songs on A.SongId equals B.SongId
                            join C in _context.Selections on A.SelectionId equals C.SelectionId
                            select new { 
                                SelectionDetailID =A.SelectionDetailId, 
                                VoteCount=A.VoteCount, 
                                CoverPhotoPath=B.CoverPhotoPath,
                                SongDescription= B.SongDescription,
                                SongName = B.SongName,
                                LikeCount = B.LikeCount,
                                SongPath = B.SongPath,
                                StartDate=C.StartDate,
                                EndDate=C.EndDate,
                                VotingStartDate=C.VotingStartDate,
                                VotingEndDate=C.VotingEndDate,
                                Title=C.Title
                            };
                var queryList = await query.ToListAsync(); // 將查詢結果轉換為列表
                List<SongDTO> songsList = new List<SongDTO>();
                foreach (var item in queryList)
                {
                    songsList.Add(new SongDTO
                    {
                        SelectionDetailId = item.SelectionDetailID,
                        VoteCount = item.VoteCount,
                        CoverPhotoPath = item.CoverPhotoPath,
                        SongDescription = item.SongDescription,
                        SongName = item.SongName,
                        LikeCount = item.LikeCount,
                        SongPath = item.SongPath
                    });
                }

                var SelectionList = new SelectionListDTO
                {
                    Title  = queryList.FirstOrDefault()?.Title,
                    StartDate = queryList.FirstOrDefault()?.StartDate,
                    EndDate = queryList.FirstOrDefault()?.EndDate,
                    JoinState = GetState(queryList.FirstOrDefault()?.StartDate, queryList.FirstOrDefault()?.EndDate),
                    VoteState = GetState(queryList.FirstOrDefault()?.VotingStartDate, queryList.FirstOrDefault()?.VotingEndDate),
                    SelectionId = id.Value, 
                    Songs = songsList 
                };

                return SelectionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<SelectionListDTO?> GetFormData(int? id)
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

    }
}
