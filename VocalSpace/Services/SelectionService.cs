using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Selection;


namespace VocalSpace.Services
{
    public class SelectionService
    {
        //第二步: 建立Service 處理業務邏輯
        private readonly VocalSpaceDbContext _context;
        private readonly IPaginationService _pagination;

        public SelectionService(VocalSpaceDbContext context, IPaginationService pagination)
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


        public async Task<SelectionListViewModel?> GetWorks(int? id, int currentPage = 1)
        {
            try
            {
                //每頁顯示筆數
                int pageSize = 10;
                var query = from C in _context.Selections  // Selections 是主資料表
                            join A in _context.SelectionDetails on C.SelectionId equals A.SelectionId into AC
                            from A in AC.DefaultIfEmpty()  // 這裡模擬 RIGHT JOIN
                            join B in _context.Songs on A.SongId equals B.SongId into AB
                            from B in AB.DefaultIfEmpty()  // LEFT JOIN Songs
                            where C.SelectionId == id
                            select new
                            {
                                SelectionDetailID = A != null ? (long?)A.SelectionDetailId : 0,
                                VoteCount = A != null ? A.VoteCount : (int?)0,
                                CoverPhotoPath = B != null ? B.CoverPhotoPath : "",
                                SongDescription = B != null ? B.SongDescription : "",
                                SongName = B != null ? B.SongName : "",
                                LikeCount = B != null ? B.LikeCount : (int?)0,
                                SongPath = B != null ? B.SongPath : "",
                                StartDate = C != null ? C.StartDate : (DateTime?)null,
                                EndDate = C != null ? C.EndDate : (DateTime?)null,
                                VotingStartDate = C != null ? C.VotingStartDate : (DateTime?)null,
                                VotingEndDate = C != null ? C.VotingEndDate : (DateTime?)null,
                                Title = C != null ? C.Title : ""  // Handle null by providing fallbacks
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
                    if (item.SelectionDetailID != 0) { 
                    songsList.Add(new SelectionSongs
                    {
                        SelectionDetailId = (long)item.SelectionDetailID,
                        VoteCount = item.VoteCount,
                        CoverPhotoPath = item.CoverPhotoPath,
                        SongDescription = item.SongDescription,
                        SongName = item.SongName,
                        LikeCount = item.LikeCount,
                        SongPath = item.SongPath
                    });
                    }
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
                    PaginationCount = PaginationSongs.PaginationCount
                };


                return SelectionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<SelectionFormViewModel?> GetFormData(int? id)
        {
            try
            {
                //判斷是否超過報名期限




                //尚未有user物件 無法做
                long userId = 9; ;
                //如果我沒報名過=>
               var  checkApplyStatus = from A in _context.Songs
                            join B in _context.SelectionDetails on A.SongId equals B.SongId
                            join C in _context.Selections on B.SelectionId equals C.SelectionId
                            join D in _context.Users on A.Artist equals D.UserId
                            where D.UserId == userId && C.SelectionId == id
                            select new
                            {
                                A.SongId,
                                B.SelectionDetailId,
                                A.CoverPhotoPath,
                                A.SongName,
                                A.SongPath,
                                C.StartDate,
                                C.EndDate,
                                C.VotingStartDate,
                                C.VotingEndDate,
                                C.Title,
                                C.SelectionCoverPath,
                                C.SelectionId,
                                D.UserName
                            };

                bool hasData =await checkApplyStatus.AnyAsync();

                //取得資料

                    //沒有報名過
                    //取得活動資料 & 使用者資料 & 作品 (LEFT JOIN)
                    var notAppliedQuery = from A in _context.Songs
                                join B in _context.Users on A.Artist equals B.UserId
                                join C in _context.UsersInfos on B.UserId equals C.UserId
                                where B.UserId == userId
                                select new
                                {
                                    A.SongId,
                                    A.CoverPhotoPath,
                                    A.SongName,
                                    A.SongPath,
                                    B.UserName,
                                    C.Email
                                };
    
                    //已經報名過=>
                    //取得活動資料 & 使用者資料 & 作品 (LEFT JOIN)
                   var AppliedQuery = from A in _context.Songs
                                join B in _context.SelectionDetails on A.SongId equals B.SongId into AB
                                from B in AB.DefaultIfEmpty()
                                join C in _context.Selections on B.SelectionId equals C.SelectionId into BC
                                from C in BC.DefaultIfEmpty()
                                join D in _context.Users on A.Artist equals D.UserId
                                join E in _context.UsersInfos on D.UserId equals E.UserId
                                where D.UserId == userId
                                select new
                                {
                                    A.SongId,
                                    SelectionDetailId = B != null ? B.SelectionDetailId : 0,  // 如果 B 為 null，給預設值
                                    A.CoverPhotoPath,
                                    A.SongName,
                                    A.SongPath,
                                    StartDate = C != null ? C.StartDate : DateTime.MinValue,  // 如果 C 為 null，給預設值
                                    EndDate = C != null ? C.EndDate : DateTime.MinValue,
                                    VotingStartDate = C != null ? C.VotingStartDate : DateTime.MinValue,
                                    VotingEndDate = C != null ? C.VotingEndDate : DateTime.MinValue,
                                    Title = C != null ? C.Title : "無標題",  // 如果 C 為 null，給預設值
                                    SelectionCoverPath = C != null ? C.SelectionCoverPath : "無封面",
                                    SelectionId = C != null ? C.SelectionId : 0,  // 如果 C 為 null，給預設值
                                    UserName = D != null ? D.UserName : "未知",  // 如果 D 為 null，給預設值
                                    Email = E != null ? E.Email : "無電子郵件"  // 如果 E 為 null，給預設值
                                };


                IEnumerable<dynamic> queryList; 
                if (!hasData) {
                    
                    // 排序
                    var sortedQuery = notAppliedQuery.OrderBy(q => q.SongId)  // 主要排序 升冪
                                           .ThenBy(q => q.SongName);           // 次要排序 升冪

                    //非同步查詢並轉成清單 (查詢所有活動徵選作品)
                     queryList = await sortedQuery.ToListAsync();
                }
                else
                {
                    // 排序
                    var sortedQuery = AppliedQuery.OrderBy(q => q.SongId)  // 主要排序 升冪
                                           .ThenBy(q => q.SongName);           // 次要排序 升冪

                    //非同步查詢並轉成清單 (查詢所有活動徵選作品)
                     queryList = await sortedQuery.ToListAsync();
                }


                // 建立歌曲清單 
                List<SelectionSongs> songsList = new List<SelectionSongs>();

                foreach (var item in queryList)
                {
                    // 使用正確的型別進行強制轉換

                    songsList.Add(new SelectionSongs
                    {
                        SongId= item.SongId,
                        SelectionDetailId = item.SelectionDetailId, //long型態預設為0
                        CoverPhotoPath = item.CoverPhotoPath,
                        SongName = item.SongName,
                        SongPath = item.SongPath
                    });
                }
                var SelectionList = new SelectionFormViewModel
                {
                    Title = queryList.FirstOrDefault()?.Title,
                    StartDate = queryList.FirstOrDefault()?.StartDate,
                    EndDate = queryList.FirstOrDefault()?.EndDate,
                    JoinState = GetState(queryList.FirstOrDefault()?.StartDate, queryList.FirstOrDefault()?.EndDate),
                    VoteState = GetState(queryList.FirstOrDefault()?.VotingStartDate, queryList.FirstOrDefault()?.VotingEndDate),
                    SelectionId = queryList.FirstOrDefault()?.SelectionId ?? 0,
                    Songs = songsList,
                    SelectionCoverPath= queryList.FirstOrDefault()?.SelectionCoverPath,
                    ApplyStatus = songsList.Count() <= 0 ? 0 : 1,
                    UserName = queryList.FirstOrDefault()?.UserName,
                    Email = queryList.FirstOrDefault()?.Email
                };
                return SelectionList;
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
