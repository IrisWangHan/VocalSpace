using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Filters;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Global;
using VocalSpace.Models.ViewModel.Personal;
using VocalSpace.Models.ViewModel.Song;
using VocalSpace.Services;


namespace VocalSpace.Controllers
{
    //設定路由
    public class PersonalController : Controller
    {
        // 建構函式，初始化資料庫context和service
        private readonly VocalSpaceDbContext _context;
        private readonly UserService _UserService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ModalDataService _ModalDataService;


        public PersonalController(VocalSpaceDbContext context, UserService UserService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _UserService = UserService;
            _webHostEnvironment = webHostEnvironment;
            _ModalDataService = modalDataService;
        }

        private IQueryable<PersonalViewModel> personal(long id) 
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");

            int FollowCount =  _context.UserFollows
                                    .Where(uf => uf.FollowedUserId == id)
                                    .Count();

            IQueryable<PersonalViewModel> personals = from user in _context.Users
                                                      join UsersInfo in _context.UsersInfos on user.UserId equals UsersInfo.UserId
                                                      where user.UserId == id
                                                      select new PersonalViewModel
                                                      {
                                                          CurrentUserId = currentUserId ?? 0,
                                                          UserId = user.UserId,
                                                          UserName = user.UserName,
                                                          PersonalIntroduction = UsersInfo.PersonalIntroduction,
                                                          Account = user.Account,
                                                          CreateTime = user.CreateTime,
                                                          BannerImagePath = UsersInfo.BannerImagePath,
                                                          AvatarPath = UsersInfo.AvatarPath,
                                                          FollowCount = FollowCount,
                                                          isFollowing = currentUserId.HasValue && currentUserId != 0  //如果使用者未登入，預設為false
                                                            ? _context.UserFollows.Any(f => f.UserId == currentUserId && f.FollowedUserId == id) : false
                                                      };
            return personals;
        }
        [HttpGet("Personal/mymusic/{id}")]
        public async Task<IActionResult> mymusic(long id)
        {

            var SongCount = await _context.Songs
            .Where(song => song.Artist == id)
            .CountAsync();

            var songdata = await _context.Songs
                .Include(s => s.ArtistNavigation)
                .Where(s => s.Artist == id)
                .Select(s => new SongViewModel
                {
                    SongId = s.SongId,
                    SongCoverPhotoPath = s.CoverPhotoPath,
                    SongName = s.SongName,
                    UserId = s.Artist,
                    UserName = s.ArtistNavigation.UserName!,
                    SongCount = SongCount
                }).ToListAsync();
            ViewData["song"] = songdata.Any() ? songdata : null;
            return View(personal(id).ToList());
        }
        [HttpGet("Personal/myabout/{id}")]
        public IActionResult myabout(long id)
        {
            return View(personal(id).ToList());
        }
        [HttpGet("Personal/mylist/{id}")]
        public async Task<IActionResult> mylist(long id)
        {
            long? currentUserId = HttpContext.Session.GetInt32("UserId");
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
                ViewData["mylist"] = songdata.Any() ? songdata : null;
                return View(personal(id).ToList());
            }
            return Content("<script>alert('無權查看'); window.history.back();</script>", "text/html; charset=utf-8");

           
        }
        [HttpGet("Personal/mylike/{id}")]
        public async Task<IActionResult> mylike(long id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");


            if (id == currentUserId) {

                var likeSongCount = await _context.LikeSongs
                .Where(like => like.UserId == id)
                .CountAsync();

                var songdata = await _context.Songs
                   .Include(s => s.ArtistNavigation)
                   .Include(s => s.LikeSongs)
                   .Where(s => s.LikeSongs.Any(like => like.UserId == id))
                   .Select(s => new SongViewModel
                   {
                       SongId = s.SongId,
                       SongCoverPhotoPath = s.CoverPhotoPath,
                       SongName = s.SongName,
                       UserId = id,
                       UserName = s.ArtistNavigation.UserName!,
                       LikeId = s.LikeSongs.FirstOrDefault(like => like.UserId == id)!.LikeId,
                       LikeSongCount = likeSongCount
                   }).ToListAsync();
            ViewData["likesong"]= songdata.Any()?songdata:null;

            return View(personal(id).ToList());
            }
            return Content("<script>alert('無權查看'); window.history.back();</script>", "text/html; charset=utf-8");

        }


        [HttpPost("Personal/UploadAvatar")]
        public async Task<IActionResult> UploadAvatar(IFormFile avatarFile)
        {
            if (avatarFile != null && avatarFile.Length > 0)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "Avatar", avatarFile.FileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await avatarFile.CopyToAsync(stream);
                    }

                    // 更新用戶的頭貼路徑
                    var userInfo = await _context.UsersInfos.FirstOrDefaultAsync(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
                    if (userInfo != null)
                    {
                        userInfo.AvatarPath = "/image/Avatar/" + avatarFile.FileName;
                        await _context.SaveChangesAsync();
                    }

                    return Json(new { filePath = "/image/Avatar/" + avatarFile.FileName });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "上傳失敗", error = ex.Message });
                }
            }

            return BadRequest(new { message = "無效的文件" });
        }

        [HttpPost("Personal/UploadBanner")]
        public async Task<IActionResult> UploadBanner(IFormFile bannerFile)
        {
            if (bannerFile != null && bannerFile.Length > 0)
            {
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "image", "banners", bannerFile.FileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await bannerFile.CopyToAsync(stream);
                    }

                    // 更新用戶的封面路徑
                    var userInfo = await _context.UsersInfos.FirstOrDefaultAsync(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
                    if (userInfo != null)
                    {
                        userInfo.BannerImagePath = "/image/banners/" + bannerFile.FileName;
                        await _context.SaveChangesAsync();
                    }

                    return Json(new { filePath = "/image/banners/" + bannerFile.FileName });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "上傳失敗", error = ex.Message });
                }
            }

            return BadRequest(new { message = "無效的文件" });
        }


        
        /// <summary>
        /// 追蹤功能API方法邏輯
        /// </summary>
        [HttpPost("Personal/ToggleFollow/{targetUserId}")]
        public async Task<IActionResult> ToggleFollow(long targetUserId)
        {
            long? currentUserId = HttpContext.Session.GetInt32("UserId");

            if (currentUserId == null)
            {
                return Unauthorized(new { message = "請先登入" });
            }

            var (isSuccess, isFollowing) = await _UserService.ToggleFollowAsync(currentUserId.Value, targetUserId);

            if(currentUserId == targetUserId)
            {
                return BadRequest(new { message = "操作失敗，無法追蹤自己。" });
            }

            if (!isSuccess)
            {
                return StatusCode(500, new { message = "操作失敗，請稍後再試。" });
            }

            // 重新取得 UserBarData，讓前端能夠更新按鈕狀態
            var userBarData = await _UserService.GetUserBarData(currentUserId, targetUserId);

            if (userBarData == null)
            {
                return NotFound(new { message = "使用者不存在" });
            }

            return Ok(new {isFollowing});
        }


        /// <summary>
        ///UserBar 的 Partial View
        /// </summary>
        [HttpGet("Personal/GetUserBar/{targetUserId}")]
        public async Task<IActionResult> GetUserBar(long targetUserId)
        {
           long? currentUserId = HttpContext.Session.GetInt32("UserId");
            // 取得 UserBarViewModel
            var userBarData = await _UserService.GetUserBarData(currentUserId ?? 0 , targetUserId);
            if(userBarData==null)
            {
                return NotFound(); // 找不到使用者
            }
            return PartialView("_Userbar_partial", userBarData);
        }

        /// <summary>
        /// AJAX 喜歡歌單邏輯
        /// </summary>
        [HttpPost("/Personal/AddLikePlaylist")]
        public async Task<IActionResult> AddLikePlaylist([FromBody] Favoriteplaylist model)
        {
            // 取得使用者ID
            long? userId = HttpContext.Session.GetInt32("UserId");

            // 確保使用者已登入
            if (userId == null || userId == 0)
            {
                return Unauthorized(new { success = false, message = "請先登入！" });
            }
            //  判斷歌單是否已喜歡 -> 新增或刪除 Favoriteplaylist 資料
            var (isSuccess, isliked) = await _ModalDataService.AddLikePlaylistAsync(userId.Value, model.PlayListId);

            if (!isSuccess)
            {
                return StatusCode(500, new { message = "操作失敗，請稍後再試。" });
            }

            return Ok(new { isliked, message = isliked ? "歌單已加入收藏" : "歌單已移除收藏" });

        }
    }
}

