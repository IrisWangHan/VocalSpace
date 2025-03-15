using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Selection;


namespace VocalSpace.Services
{
    public class SelectionService
    {
        //第二步: 建立Service 處理業務邏輯
        private readonly VocalSpaceDbContext _context;
        private readonly IPaginationService _pagination;

        public SelectionService(VocalSpaceDbContext context, IPaginationService pagination )
        {
            _context = context;
            _pagination = pagination;
        }

        public async Task<List<SelectionListViewModel>?> GetSelectionsAsync()
        {
            try
            {
                return await _context.Selections
                .Select(s => new SelectionListViewModel
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
        public async Task<SelectionListViewModel?> GetEventDescriptionAsync(int? id)
        {
            try
            {
                return await _context.Selections
                    .Where(s => s.SelectionId == id)
                    .Select(s => new SelectionListViewModel
                    {
                        Title = s.Title,
                        SelectionCoverPath = s.SelectionCoverPath,
                        JoinState = GetState(s.StartDate, s.EndDate),
                        VoteState = GetState(s.VotingStartDate, s.VotingEndDate),
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        VotingStartDate = s.VotingStartDate,
                        VotingEndDate = s.VotingEndDate,
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


        public async Task<SelectionListViewModel?> GetWorks(int? id, int currentPage=1)
        {
            try
            {
                //每頁顯示筆數
                int pageSize = 10;
                var query = from A in _context.SelectionDetails
                            join B in _context.Songs on A.SongId equals B.SongId
                            join C in _context.Selections on A.SelectionId equals C.SelectionId
                            select new
                            {
                                SelectionDetailID = A.SelectionDetailId,
                                A.VoteCount,
                                B.CoverPhotoPath,
                                B.SongDescription,
                                B.SongName,
                                B.LikeCount,
                                B.SongPath,
                                C.StartDate,
                                C.EndDate,
                                C.VotingStartDate,
                                C.VotingEndDate,
                                C.Title
                            };
                // 排序
                var sortedQuery = query.OrderBy(q => q.SelectionDetailID)  // 主要排序 升冪
                                       .ThenBy(q => q.SongName);           // 次要排序 升冪

                //非同步查詢並轉成清單 (查詢所有活動徵選作品)
                var queryList = await sortedQuery.ToListAsync();           

                // 建立歌曲清單 
                List<SelectionSongs> songsList = new List<SelectionSongs>();
                foreach (var item in queryList)
                {
                    songsList.Add(new SelectionSongs
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
                // 配合前端分頁功能 傳入參數(List物件,當前的頁數,每頁顯示幾筆)
                // =>會回傳整包的物件 使用方式如下:
                //        取得該頁面的歌曲列表     PaginationSongs.DataList
                //        取得資料歌曲總筆數       PaginationSongs.TotalPage
                //        取得目前頁              PaginationSongs.CurrentPage
                //        取得總分頁數            PaginationSongs.PaginationCount
                //        第 [起始頁數]           PaginationSongs.StartPageNumber
                //        第 [結束頁數]           PaginationSongs.EndPageNumber
                var PaginationSongs = _pagination.GetPaginationToList(songsList, currentPage, pageSize);

                var SelectionList = new SelectionListViewModel
                {
                    Title = queryList.FirstOrDefault()?.Title,
                    StartDate = queryList.FirstOrDefault()?.StartDate,
                    EndDate = queryList.FirstOrDefault()?.EndDate,
                    JoinState = GetState(queryList.FirstOrDefault()?.StartDate, queryList.FirstOrDefault()?.EndDate),
                    VoteState = GetState(queryList.FirstOrDefault()?.VotingStartDate, queryList.FirstOrDefault()?.VotingEndDate),
                    SelectionId = id.Value,
                    Songs = PaginationSongs.DataList,
                    PaginationCount= PaginationSongs.PaginationCount
                };

                
                return SelectionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<SelectionListViewModel?> GetFormData(int? id)
        {
            try
            {
                return await _context.Selections
                    .Where(s => s.SelectionId == id)
                    .Select(s => new SelectionListViewModel
                    {
                        Title = s.Title,
                        SelectionCoverPath = s.SelectionCoverPath,
                        JoinState = GetState(s.StartDate, s.EndDate),
                        VoteState = GetState(s.VotingStartDate, s.VotingEndDate),
                        StartDate = s.StartDate,
                        EndDate = s.EndDate,
                        VotingStartDate = s.VotingStartDate,
                        VotingEndDate = s.VotingEndDate,
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
