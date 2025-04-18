﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Selection;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static VocalSpace.Controllers.SelectionController;


namespace VocalSpace.Services
{
    public class SelectionService
    {
        private readonly VocalSpaceDbContext _context;
        private readonly IPaginationService _pagination;

        public SelectionService(VocalSpaceDbContext context, IPaginationService pagination)
        {
            _context = context;
            _pagination = pagination;
        }



        /// <summary>
        /// 取得活動資料 不需驗身分
        /// </summary>
        public async Task<List<SelectionListViewModel>?> GetSelectionsAsync()
        {
            try
            {
                return await _context.Selections
                .Where(s => s.Visible == true)  // 這裡過濾掉 Visible != 1 的資料
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

        /// <summary>
        /// 取得徵選活動的內容
        /// </summary>
        /// <param name="id">徵選活動ID</param>
        /// <returns></returns>
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

        /// <summary>
        /// 取得徵選活動的作品
        /// </summary>
        /// <param name="id">徵選活動ID</param>
        /// <param name="currentPage">點選的頁面</param>
        /// <returns></returns>
        public async Task<SelectionListViewModel?> GetWorks(int? id, long? UserID, int currentPage = 1)
        {
            try
            {
                //取得活動資料
                var selectionData = await _context.Selections
                    .FromSqlRaw(@" SELECT Title, SelectionCoverPath, StartDate, EndDate,Visible,SelectionID,VotingStartDate,VotingEndDate
                                    FROM Selection
                                   WHERE SelectionID = @SelectionID 
                                   AND Visible=1  ", new SqlParameter("@SelectionID", id))
                    .Select(u => new { u.Title, u.StartDate, u.EndDate, u.SelectionCoverPath, u.SelectionId, u.VotingStartDate, u.VotingEndDate })  // 使用匿名類型選擇需要的欄位
                    .FirstOrDefaultAsync();


                //每頁顯示筆數
                int pageSize = 10;
                var query = from C in _context.Selections  // Selections 是主資料表
                            join A in _context.SelectionDetails on C.SelectionId equals A.SelectionId into AC
                            from A in AC.DefaultIfEmpty()  // 這裡模擬 RIGHT JOIN
                            join B in _context.Songs on A.SongId equals B.SongId into AB
                            from B in AB.DefaultIfEmpty()  // LEFT JOIN Songs
                            join D in _context.LikeSongs on B.SongId equals D.SongId into BD
                            from D in BD.DefaultIfEmpty()  // LEFT JOIN Songs
                            join E in _context.UserVoteds on A.SelectionDetailId equals E.SelectionDetailId into AE
                            from E in AE.DefaultIfEmpty()  // LEFT JOIN Songs
                            where C.SelectionId == id && B.SongStatus == 1 && A.ReviewStatus == 1
                            select new
                            {
                                SelectionDetailID = A != null ? (long?)A.SelectionDetailId : 0,
                                VoteCount = A != null ? A.VoteCount : (int?)0,
                                CoverPhotoPath = B != null ? B.CoverPhotoPath : "",
                                SongDescription = B != null ? B.SongDescription : "",
                                SongID = B != null ? B.SongId : 0,
                                SongName = B != null ? B.SongName : "",
                                LikeCount = B != null ? B.LikeCount : (int?)0,
                                SongPath = B != null ? B.SongPath : "",
                                StartDate = C != null ? C.StartDate : (DateTime?)null,
                                EndDate = C != null ? C.EndDate : (DateTime?)null,
                                VotingStartDate = C != null ? C.VotingStartDate : (DateTime?)null,
                                VotingEndDate = C != null ? C.VotingEndDate : (DateTime?)null,
                                Title = C != null ? C.Title : "", // Handle null by providing fallbacks
                                LikeSongUserId = D != null ? D.UserId : 0,
                                VoteUserId = E != null ? E.UserId : 0
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
                    if (item.SelectionDetailID != 0)
                    {
                        if (UserID == 0)
                        {//'未登入
                            songsList.Add(new SelectionSongs
                            {
                                SongId = item.SongID,
                                SelectionDetailId = (long)item.SelectionDetailID,
                                VoteCount = item.VoteCount,
                                CoverPhotoPath = item.CoverPhotoPath,
                                SongDescription = item.SongDescription,
                                SongName = item.SongName,
                                LikeCount = item.LikeCount,
                                SongPath = item.SongPath,
                                IsLiked = false,
                                IsVoted = false
                            });
                        }
                        else
                        {
                            songsList.Add(new SelectionSongs
                            {
                                SongId = item.SongID,
                                SelectionDetailId = (long)item.SelectionDetailID,
                                VoteCount = item.VoteCount,
                                CoverPhotoPath = item.CoverPhotoPath,
                                SongDescription = item.SongDescription,
                                SongName = item.SongName,
                                LikeCount = item.LikeCount,
                                SongPath = item.SongPath,
                                IsLiked = item.LikeSongUserId == UserID ? true : false,
                                IsVoted = item.VoteUserId == UserID ? true : false
                            });
                        }

                    }
                }


                List<SelectionSongs> songsListDistinct = songsList
                   .GroupBy(x => x.SelectionDetailId)  // 根據 SelectionDetailId 分組
                   .Select(g => g.FirstOrDefault(x => (x.IsLiked.GetValueOrDefault(false)) || (x.IsVoted.GetValueOrDefault(false))) ?? g.First())
                   .ToList();


                // 配合前端分頁功能 傳入參數(List物件,當前的頁數,每頁顯示幾筆)
                // =>會回傳整包的物件 使用方式如下:
                //        取得該頁面的歌曲列表     PaginationSongs.DataList
                //        取得資料歌曲總筆數       PaginationSongs.TotalPage
                //        取得目前頁              PaginationSongs.CurrentPage
                //        取得總分頁數            PaginationSongs.PaginationCount
                //        第 [起始頁數]           PaginationSongs.StartPageNumber
                //        第 [結束頁數]           PaginationSongs.EndPageNumber
                var PaginationSongs = _pagination.GetPaginationToList(songsListDistinct, currentPage, pageSize);
                var SelectionList = new SelectionListViewModel();
                // 檢查是否找到活動資料
                if (selectionData != null)
                {

                    SelectionList = new SelectionListViewModel
                    {
                        Title = selectionData.Title,
                        StartDate = selectionData.StartDate,
                        EndDate = selectionData.EndDate,
                        JoinState = GetState(selectionData.VotingStartDate, selectionData.VotingEndDate),
                        VoteState = GetState(selectionData.StartDate, selectionData.EndDate),
                        SelectionId = id!.Value,
                        Songs = PaginationSongs.DataList,
                        PaginationCount = PaginationSongs.PaginationCount
                    };
                }


                return SelectionList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #region 活動報名表單頁
        /// <summary>
        /// 活動報名 需取得user資料
        /// </summary>
        /// <param name="id">徵選活動ID</param>
        /// <returns></returns>
        public async Task<SelectionFormViewModel> CheckUser(long? UserID)
        {
            try
            {
                SelectionFormViewModel data = new();
                //判斷已經登入帶入使用者資料
                if (UserID != null && UserID != 0)
                {
                    // 使用 await 查詢使用者資料
                    var userData = await (from u in _context.Users
                                          join ui in _context.UsersInfos on u.UserId equals ui.UserId
                                          where u.UserId == UserID
                                          select new
                                          {
                                              u.UserId,
                                              u.UserName,
                                              ui.Email,
                                              ui.AvatarPath
                                          }).FirstOrDefaultAsync();

                    // 檢查是否找到使用者資料
                    if (userData != null)
                    {
                        // 賦值給 ViewModel
                        data.UserName = userData.UserName;
                        data.Email = userData.Email;
                        data.AvatarPath = userData.AvatarPath;
                        data.UserId = userData.UserId;
                    }
                }
                return data!;
            }
            catch (Exception ex)
            {
                // 錯誤處理，這裡可以進行日誌記錄等
                return null!;
            }
        }


        /// <summary>
        /// 取得活動資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SelectionFormViewModel> CheckSelectionOnTime(int? id)
        {
            try
            {
                SelectionFormViewModel data = new();

                //判斷是否超過報名期限
                var selectionData = await _context.Selections
                    .FromSqlRaw(@" SELECT Title, SelectionCoverPath, StartDate, EndDate,Visible,SelectionID,VotingStartDate,VotingEndDate
                                    FROM Selection
                                   WHERE SelectionID = @SelectionID 
                                    AND Visible=1  
                                    AND StartDate <= GETDATE() 
                                    AND GETDATE() <= EndDate
                                ", new SqlParameter("@SelectionID", id))
                    .Select(u => new { u.Title, u.StartDate, u.EndDate, u.SelectionCoverPath, u.SelectionId, u.VotingStartDate, u.VotingEndDate })  // 使用匿名類型選擇需要的欄位
                    .FirstOrDefaultAsync();

                // 檢查是否找到活動資料
                if (selectionData != null)
                {
                    // 賦值給 ViewModel
                    data.Title = selectionData.Title;
                    data.StartDate = selectionData.StartDate;
                    data.EndDate = selectionData.EndDate;
                    data.SelectionCoverPath = selectionData.SelectionCoverPath;
                    data.SelectionId = selectionData.SelectionId;
                    data.VoteState = GetState(selectionData.VotingStartDate, selectionData.VotingEndDate);
                    data.JoinState = GetState(selectionData.StartDate, selectionData.EndDate);
                }
                return data!;
            }
            catch (Exception ex)
            {
                // 錯誤處理，這裡可以進行日誌記錄等
                return null!;
            }
        }

        /// <summary>
        /// 帶入所有資料 & 歌曲作品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SelectionFormViewModel?> GetFormData(int? id, SelectionFormViewModel userData, SelectionFormViewModel selectionData)
        {
            try
            {

                SelectionFormViewModel data = new();
                // 1️ 先判斷是否報名過
                bool hasJoined = await _context.SelectionDetails
                    .AnyAsync(sd => sd.Selection.SelectionId == id && sd.Song.Artist == userData.UserId);

                List<SelectionSongs> songs;
                List<SelectionSongs> applySongs = new List<SelectionSongs>();

                if (hasJoined)
                {
                    // 2️ 已報名，查詢對應的歌曲 左外聯結 (GroupJoin() + Select())
                    applySongs = await _context.Songs
                        .Where(s => s.Artist == userData.UserId) // 取得該用戶所有歌曲
                        .GroupJoin(
                            _context.SelectionDetails.Where(sd => sd.SelectionId == id), // 只選擇對應 SelectionID 的報名資料
                            song => song.SongId,
                            sd => sd.SongId,
                            (song, sdGroup) => new { song, SelectionDetailId = sdGroup.Select(sd => (int?)sd.SelectionDetailId).FirstOrDefault() } // 取第一筆或 NULL
                        )
                        .Select(x => new SelectionSongs
                        {
                            SongId = x.song.SongId,
                            SongName = x.song.SongName,
                            SongDescription = x.song.SongDescription,
                            CoverPhotoPath = x.song.CoverPhotoPath,
                            SongPath = x.song.SongPath,
                            LikeCount = x.song.LikeCount,
                            SelectionDetailId = x.SelectionDetailId ?? 0  // 修正：null 改為 0
                        })
                        .ToListAsync();

                    applySongs = applySongs
                   .Where(song => song.SelectionDetailId != 0)
                   .ToList();
                }

                // 3️ 無論有無報名，查詢該用戶所有歌曲
                songs = await _context.Songs
                    .Where(s => s.Artist == userData.UserId)
                     .Select(x => new SelectionSongs
                     {
                         SongId = x.SongId,
                         SongName = x.SongName,
                         SongDescription = x.SongDescription,
                         CoverPhotoPath = x.CoverPhotoPath,
                         SongPath = x.SongPath,
                         LikeCount = x.LikeCount,
                         SelectionDetailId = 0
                     })
                    .ToListAsync();


                data.Title = selectionData.Title;
                data.StartDate = selectionData.StartDate;
                data.EndDate = selectionData.EndDate;
                data.VoteState = selectionData.VoteState;
                data.JoinState = selectionData.JoinState;
                data.ApplyStatus = applySongs.Count > 0 ? (int?)SelectionApplyStatus.Applied : (int?)SelectionApplyStatus.NotApplied;
                data.SelectionCoverPath = selectionData.SelectionCoverPath;
                data.UserName = userData.UserName;
                data.Email = userData.Email;
                data.AvatarPath = userData.AvatarPath;
                data.SelectionId = selectionData.SelectionId;
                data.Songs = songs ?? new List<SelectionSongs>();
                data.ApplySongs = applySongs ?? new List<SelectionSongs>();
                switch (data.ApplyStatus)
                {
                    case (int?)SelectionApplyStatus.NotApplied:
                        data.ApplyStatusPath = "/image/selection/nothing.png";
                        data.ApplyStatusText = "尚未報名";
                        break;
                    case (int?)SelectionApplyStatus.Applied:
                        data.ApplyStatusPath = "/image/selection/success.png";
                        data.ApplyStatusText = "報名完成";
                        break;
                    case (int?)SelectionApplyStatus.Failed:
                        data.ApplyStatusPath = "/image/selection/errorRobot.png";
                        data.ApplyStatusText = "報名失敗";
                        break;
                }


                return data;
            }
            catch (Exception ex)
            {
                // 錯誤處理，可以記錄錯誤或回傳自定義錯誤訊息
                // Log.Error(ex, "Error fetching selection details");
                return null;
            }
        }
        #endregion

        public async Task<bool> InsertSelectionDetail(SubmitApplicationDTO data)
        {
            var selectionDetails = new List<SelectionDetail>();

            // 遍歷 ApplySongs，為每首歌創建一個 SelectionDetail
            foreach (var songsId in data.SongsId)
            {
                var sd = new SelectionDetail
                {
                    SelectionId = data.SelectionId,
                    SongId = songsId,
                    VoteCount = 0,
                    CreateTime = DateTime.Now,
                    ReviewStatus = 1
                };

                selectionDetails.Add(sd);
            }

            // **將資料批量新增到資料庫**
            await _context.SelectionDetails.AddRangeAsync(selectionDetails);

            // **異步儲存變更**
            await _context.SaveChangesAsync();

            return true;
        }


        /// <summary>
        /// 判斷歌曲有沒有按讚，並執行新增或刪除
        /// </summary>
        public async Task<SelectionSongs> AddToVoteSong(int? userid, long selectionDetailId)
        {
            SelectionSongs selectionSong = new();
            try
            {
               
                //找到該音樂作品的投票數
                var song = await _context.SelectionDetails
                   .Where(s => s.SelectionDetailId == selectionDetailId)
                   .Select(s => new { s.VoteCount,s.SelectionId })
                   .FirstOrDefaultAsync();

                if (song == null)
                {
                    return null;
                }
                int newVoteCount = song.VoteCount;
                int selectionId = (int)song.SelectionId;
                //找到使用者是否有對該作品按讚
                var songVote = await _context.UserVoteds
                    .FirstOrDefaultAsync(sd => sd.SelectionDetailId == selectionDetailId && sd.UserId == userid);


                if (songVote != null)
                {
                    // 已經按讚 → 取消讚
                    _context.UserVoteds.Remove(songVote);
                    selectionSong.IsVoted = false;
                    // 確保 VoteCount 不會小於 0
                    newVoteCount = Math.Max(0, song.VoteCount - 1);
                    await _context.Database.ExecuteSqlRawAsync("UPDATE SelectionDetail SET VoteCount = {0} WHERE SelectionDetailID = {1}", newVoteCount, selectionDetailId);
                }
                else
                {
                    //檢查是否有按讚該活動下其他作品
                    var songedVote = await _context.UserVoteds
                        .Join(_context.SelectionDetails, uv => uv.SelectionDetailId, sd => sd.SelectionDetailId, (uv, sd) => new { uv, sd })
                        .FirstOrDefaultAsync(joined => joined.sd.SelectionId == selectionId && joined.uv.UserId == userid);
                    if (songedVote!= null) { 
                        _context.UserVoteds.Remove(songedVote.uv);
                        var oldVoteCount = songedVote.sd.VoteCount - 1;
                        await _context.Database.ExecuteSqlRawAsync("UPDATE SelectionDetail SET VoteCount = {0} WHERE SelectionDetailID = {1}", oldVoteCount, songedVote.sd.SelectionDetailId);
                        selectionSong.OldSelectionDetailId = songedVote.sd.SelectionDetailId;
                        selectionSong.OldVoteCount = oldVoteCount;
                    }
                    
                    // 沒有按讚 → 新增讚
                    _context.UserVoteds.Add(new UserVoted
                    {
                        SelectionDetailId = selectionDetailId,
                        UserId = (long)userid!,
                    });
                    selectionSong.IsVoted = true;
                    // 增加 VoteCount
                    newVoteCount = song.VoteCount + 1;
                    await _context.Database.ExecuteSqlRawAsync("UPDATE SelectionDetail SET VoteCount = {0} WHERE SelectionDetailID = {1}", newVoteCount, selectionDetailId);
                }

                await _context.SaveChangesAsync();


                selectionSong.SelectionDetailId = selectionDetailId;
                selectionSong.VoteCount = newVoteCount;

                return selectionSong;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"處理歌單時發生錯誤: {ex.Message}");
                return null;
            }
        }
    }

    public enum SelectionApplyStatus
    {
        [Description("尚未報名")]
        NotApplied = 0,

        [Description("報名完成")]
        Applied = 1,

        [Description("報名失敗")]
        Failed = 2
    }

}
