using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Filters;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Song;

namespace VocalSpace.Controllers
{
    public class CollectionController : Controller
    {
        private readonly VocalSpaceDbContext _context;
        
        public CollectionController(VocalSpaceDbContext context)
        {
            _context = context;
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
                    UserName = p.User.UserName!,
                    PlayListId = p.PlayListId,
                    PlayListName = p.Name,
                    PlayListCoverImagePath = p.CoverImagePath,
                    PlayListSongCount = p.PlayListSongs.Count() // 計算歌單內歌曲數量
                }).ToListAsync();

                //var songdata = await _context.Songs
                //    .Include(s => s.ArtistNavigation)                    
                //    .Include(s => s.PlayListSongs.Any())
                //    .Where(s => s.LikeSongs.Any(like => like.UserId == id))
                //    .Select(s => new SongViewModel
                //    {
                //        SongId = s.SongId,
                //        SongCoverPhotoPath = s.CoverPhotoPath,
                //        SongName = s.SongName,
                //        UserName = s.ArtistNavigation.UserName!,
                //        UserId= id,
                //        LikeId = s.LikeSongs.FirstOrDefault()!.LikeId
                //    }).ToListAsync();

                return View(songdata);
            }
            return Content("<script>alert('無權查看'); window.history.back();</script>", "text/html; charset=utf-8");

        }
        [SessionToLogin]
        public IActionResult playrecord()
        {
            return View();
        }
        [SessionToLogin]
        public async Task<IActionResult> booking(long id)
        {
            // 未完成
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
        public IActionResult uploadMusic()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") == "true")
            {
                return View();
            }
            return RedirectToAction("Login", "Accounts");
        }
        [SessionToLogin]
        public IActionResult createlist()
        {
            return View();
        }
        
        public IActionResult listdetail()
        {
            return View();
        }

    }
}
