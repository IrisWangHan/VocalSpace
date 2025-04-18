﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Filters;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Selection;
using VocalSpace.Models.ViewModel.Song;
using VocalSpace.Services;


namespace VocalSpace.Controllers
{
    public class CollectionController : Controller
    {
        private readonly VocalSpaceDbContext _context;

        private readonly FileService _fileService;

        public CollectionController(VocalSpaceDbContext context, FileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        [SessionToLogin]
        public async Task<IActionResult> like(long id)
        {
            long? currentUserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.LoginID = currentUserId;
            if (id == currentUserId)
            {
                var songdata = await _context.Songs
                    .Include(s => s.ArtistNavigation)
                    .Include(s => s.LikeSongs)
                    .Where(s => s.LikeSongs.Any(like => like.UserId == id))
                    .Select(s => new SongViewModel
                    {
                        SongId = s.SongId,
                        SongCoverPhotoPath = s.CoverPhotoPath,
                        SongName = s.SongName,
                        UserName = s.ArtistNavigation.UserName!,
                        UserId = s.ArtistNavigation!.UserId,
                        LikeId = s.LikeSongs.FirstOrDefault()!.LikeId,

                    }).ToListAsync();
                return View(songdata.Any() ? songdata : null);

            }
            else
            {
                return Content("<script>alert('無權查看'); window.history.back();</script>", "text/html; charset=utf-8");

            }

        }
        [SessionToLogin]
        [HttpGet("/Collection/ list/{id}")]
        public async Task<IActionResult> mylist(long id)
        {
            long? currentUserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.LoginID = currentUserId;
            if (currentUserId == id)
            {
                var songdata = await _context.PlayLists
                 .Where(p => p.UserId == id) // 篩選該使用者的歌單
                .Select(p => new SongViewModel
                {
                    UserId = p.UserId,
                    UserName = p.User!.UserName!,
                    PlayListId = p.PlayListId,
                    PlayListName = p.Name,
                    PlayListCoverImagePath = p.CoverImagePath,
                    PlayListSongCount = p.PlayListSongs.Count() // 計算歌單內歌曲數量
                }).ToListAsync();

                // 顯示成功訊息
                ViewBag.SuccessMessage = TempData["SuccessMessage"];

                return View(songdata.Any() ? songdata : null);
            }
            return Content("<script>alert('無權查看'); window.history.back();</script>", "text/html; charset=utf-8");

        }
        [SessionToLogin]
        public async Task<IActionResult> playrecord(long id)
        {
            long? currentUserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.LoginID = currentUserId;
            if (id == currentUserId)
            {
                var songdata = await _context.Songs
                    .Include(s => s.ArtistNavigation)
                    .Include(s => s.PlayHistories)
                    .Where(s => s.PlayHistories.Any(play => play.UserId == id))
                    .Select(s => new SongViewModel
                    {
                        SongId = s.SongId,
                        SongCoverPhotoPath = s.CoverPhotoPath,
                        SongName = s.SongName,
                        UserName = s.ArtistNavigation.UserName!,
                        UserId = s.ArtistNavigation!.UserId,
                    }).ToListAsync();
                return View(songdata.Any() ? songdata : null);

            }
            else
            {
                return Content("<script>alert('無權查看'); window.history.back();</script>", "text/html; charset=utf-8");

            }
        }
        [SessionToLogin]
        public async Task<IActionResult> booking(long id)
        {
            long? currentUserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.LoginID = currentUserId;

            if (id == currentUserId)
            {
                var songdata = await _context.Favoriteplaylists
                   .Where(f => f.UserId == id)
                    .Select(f => new SongViewModel
                    {
                        UserId = f.PlayList.User!.UserId,
                        UserName = f.PlayList.User.UserName!,
                        PlayListId = f.PlayList.PlayListId, // 直接透過關聯屬性取值
                        PlayListName = f.PlayList.Name,
                        PlayListCoverImagePath = f.PlayList.CoverImagePath,
                        PlayListSongCount = _context.PlayListSongs.Count(ps => ps.PlayListId == f.PlayList.PlayListId)

                    }).ToListAsync();
                return View(songdata.Any() ? songdata : null);

            }
            else
            {
                return Content("<script>alert('無權查看'); window.history.back();</script>", "text/html; charset=utf-8");

            }
        }
        [SessionToLogin]
        public IActionResult uploadMusic()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadMusic([FromForm] SongUploadModel model)
        {
            if (model.coverImage == null || model.audioFile == null)
            {
                return BadRequest("請提供封面圖片與音樂檔案");
            }

            // 確保格式正確
            if (!model.coverImage.ContentType.StartsWith("image/") || model.audioFile.ContentType != "audio/mpeg")
            {
                return BadRequest("檔案格式錯誤");
            }

            long? currentUserId = HttpContext.Session.GetInt32("UserId");
            // 抓創作人id
            if (currentUserId == null)
            {
                return BadRequest("身份有誤");
            }
            else
            {
                currentUserId = (long)HttpContext.Session.GetInt32("UserId")!;
            }
            try
            {
                // 儲存資料庫
                var newSong = new VocalSpace.Models.Song
                {
                    SongName = model.songName,
                    Artist = (long)currentUserId,
                    SongCategoryId = model.categoryID,
                    Lyrics = model.lyrics,
                    SongPath = "/audio/sample.mp3",
                    CoverPhotoPath = "/image/Song/default.jpg",
                    CreateTime = DateTime.Now
                };

                if(model.description!=null)
                {
                    newSong.SongDescription = model.description;
                }
                if(model.lyrics != null)
                {
                    newSong.Lyrics = model.lyrics;
                }

                _context.Songs.Add(newSong);
                await _context.SaveChangesAsync();

                // **Step 2: 嘗試上傳檔案**
                string coverImagePath = await _fileService.UploadSongCoverAsync(model.coverImage);
                string audioPath = await _fileService.UploadAudioFileAsync(model.audioFile);
                if (audioPath == null || coverImagePath == null)
                {
                    // 檔案上傳失敗，刪除剛剛的資料庫紀錄，避免資料庫和 wwwroot 不同步
                    _context.Songs.Remove(newSong);
                    await _context.SaveChangesAsync();
                    return StatusCode(500, "檔案上傳失敗，請重新嘗試");
                }

                // **Step 3: 更新資料庫，改成真實檔案路徑**
                newSong.SongPath = audioPath;
                newSong.CoverPhotoPath = coverImagePath;

                await _context.SaveChangesAsync(); // 第二次存，更新檔案路徑
                
                // 新增 SongRank 記錄
                var newSongRank = new SongRank
                {
                    SongId = newSong.SongId, // 使用剛剛儲存的歌曲 ID
                    CurrentRank = 0,
                    PreRank = 0,
                    RankPeriod = DateTime.Now
                };

                _context.SongRanks.Add(newSongRank);
                await _context.SaveChangesAsync(); // 儲存排行資料
                Console.WriteLine("SongRank寫入成功...");

                return Ok(new { message = "歌曲上傳成功", songId = newSong.SongId });
            }
            catch(Exception ex)
            {
                // 確保在發生異常時也能刪除資料庫中的記錄
                var songToDelete = await _context.Songs.FirstOrDefaultAsync(s => s.SongName == model.songName && s.Artist == currentUserId);
                if (songToDelete != null)
                {
                    _context.Songs.Remove(songToDelete);
                    await _context.SaveChangesAsync();
                }
                return StatusCode(500, "發生錯誤：" + ex.Message);   
            }
        }

        [SessionToLogin]
        public IActionResult createlist()
        {
            long? currentUserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.LoginID = currentUserId;
            return View();
        }
        [SessionToLogin]
        [HttpPost]
        public async Task<IActionResult> createlist(PlayList model, IFormFile? CoverImage, bool IsPublic)
        {
            TempData["SuccessMessage"] = null;
            long? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null) return RedirectToAction("Login", "Account");
            ViewBag.LoginID = currentUserId;

            if (ModelState.IsValid)
            {
                model.CreateTime = DateTime.Now; // 設定建立時間

                model.UserId = currentUserId.Value;

                if (IsPublic == true) { model.IsPublic = true; } else { model.IsPublic = false; }

                // 處理圖片上傳
                if (CoverImage != null && CoverImage.Length > 0)
                {

                    var filePath = Path.Combine("wwwroot/image/playlist", CoverImage.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CoverImage.CopyToAsync(stream);
                    }

                    model.CoverImagePath = "/image/playlist/" + CoverImage.FileName; // 儲存相對路徑
                }
                else { model.CoverImagePath = "/image/playlist/default.jpg"; }


                _context.PlayLists.Add(model);
                await _context.SaveChangesAsync();

                // 設定 TempData 用於顯示成功訊息
                TempData["SuccessMessage"] = "播放清單新增成功！";

                // 跳轉到 CollectionController 的 MyList

                return RedirectToAction("mylist", "Collection", new { id = currentUserId.Value });
            }
            return View(model);

        }
        [HttpGet("Collection/editlist/{id}")]
        public async Task<IActionResult> editlist(long? id)
        {
            long? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null) return RedirectToAction("Login", "Account");
            ViewBag.LoginID = currentUserId;

            if (id == null)
            {
                return NotFound();
            }

            var list = await _context.PlayLists.FindAsync(id);
            if (currentUserId == list!.UserId)
            {
                if (list == null)
                {
                    return NotFound();
                }
                return View(list);
            }
            else
            {
                return Forbid("<script>alert('無權查看'); window.history.back();</script>", "text/html; charset=utf-8");
            }
        }
        [SessionToLogin]
        [HttpPost]
        public async Task<IActionResult> editlist(long id, PlayList model, IFormFile? CoverImage, bool IsPublic, string? PlaylistDescription)
        {
            TempData["SuccessMessage"] = null;
            long? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId == null) return RedirectToAction("Login", "Account");
            ViewBag.LoginID = currentUserId;

            var playlist = await _context.PlayLists.FindAsync(id);
            if (playlist == null || playlist.UserId != currentUserId.Value)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                playlist.Name = model.Name;
                playlist.PlaylistDescription = PlaylistDescription;
                playlist.IsPublic = IsPublic; // 確保布林值不為 null

                // 處理圖片上傳
                if (CoverImage != null && CoverImage.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/image/playlist", CoverImage.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await CoverImage.CopyToAsync(stream);
                    }
                    playlist.CoverImagePath = "/image/playlist/" + CoverImage.FileName;
                }

                try
                {
                    _context.PlayLists.Update(playlist);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "播放清單更新成功！";
                }
                catch (Exception ex)
                {
                    // 處理例外狀況
                    ModelState.AddModelError(string.Empty, "更新播放清單時發生錯誤：" + ex.Message);
                    return View(model);
                }

                return RedirectToAction("mylist", "Collection", new { id = currentUserId.Value });
            }

            return View(model);
        }

        public async Task<IActionResult> listdetail(long id)
        {
            var songViewModels = await _context.PlayLists
                .Where(p => p.PlayListId == id)
             .Select(p => new SongViewModel
             {
                 UserId = p.UserId,
                 UserName = p.User!.UserName!, // 歌單作者名稱
                 PlayListId = p.PlayListId,  // 歌單 ID
                 PlayListName = p.Name,      // 歌單名稱
                 PlayListCoverImagePath = p.CoverImagePath, // 歌單封面
                 PlayListSongCount = p.PlayListSongs.Count(), // 歌單內歌曲數量
                 PlayListCreateTime = p.CreateTime,
                 LikeCount = p.Favoriteplaylists.Count(), // 歌單的喜歡次數
                 Songs = p.PlayListSongs.Select(ps => new SongDetail
                 {
                     SongId = ps.SongId,
                     SongName = ps.Song.SongName,
                     ArtistId = ps.Song.ArtistNavigation.UserId,
                     SongArtist = ps.Song.ArtistNavigation.UserName!, // 歌曲作者
                     SongCoverPhotoPath = ps.Song.CoverPhotoPath // 歌曲封面
                 }).ToList() // 歌單內每首歌曲的詳細資訊
             }).ToListAsync();
            return View(songViewModels.Any() ? songViewModels : null);
        }



        [HttpPost("Collection/Delete/{id}")]
        public IActionResult Delete(long id)
        {
            var playlist = _context.PlayLists.Find(id);
            if (playlist == null)
            {
                return NotFound();
            }

            _context.PlayLists.Remove(playlist);
            _context.SaveChanges();

            return Ok(new { message = "刪除成功" });
        }

        /// <summary>
        /// 刪除歌單內的歌曲
        /// </summary>

        [HttpPost("Collection/DeleteSong/{playlistId}/{songId}")]
        public async Task<IActionResult> DeleteSong(long playlistId, long songId)
        {
            var playlistSong = await _context.PlayListSongs
                .FirstOrDefaultAsync(ps => ps.PlayListId == playlistId && ps.SongId == songId);
            if (playlistSong == null)
            {
                return NotFound();
            }

            _context.PlayListSongs.Remove(playlistSong);
            await _context.SaveChangesAsync();

            return Ok(new { message = "刪除成功" });
        }

        [HttpPost("Collection/DeleteSong/{songId}")]
        public async Task<IActionResult> DeleteSong(long songId)
        {
            var Song = await _context.Songs
                .FirstOrDefaultAsync(s => s.SongId == songId);
            if (Song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(Song);
            await _context.SaveChangesAsync();

            return Ok(new { message = "刪除成功" });
        }

    }
}


