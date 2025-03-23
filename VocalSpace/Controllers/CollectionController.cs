using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Filters;
using VocalSpace.Models;
using VocalSpace.Models.Test.Selection;
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

                return View(songdata);
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
                        UserId = f.PlayList.User.UserId,
                        UserName= f.PlayList.User.UserName!,
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
        [SessionToLogin]
        public IActionResult createlist()
        {
            return View();
        }

        public async Task<IActionResult> listdetail(long id)
        {
            var songViewModels = await _context.PlayLists     
             .Select(p => new SongViewModel
             {
                 UserId = p.UserId,
                 UserName = p.User.UserName!, // 歌單作者名稱
                 PlayListId = p.PlayListId,  // 歌單 ID
                 PlayListName = p.Name,      // 歌單名稱
                 PlayListCoverImagePath = p.CoverImagePath, // 歌單封面
                 PlayListSongCount = p.PlayListSongs.Count(), // 歌單內歌曲數量
                 PlayListCreateTime = p.CreateTime,
                 PlayCount = p.PlayListSongs.Sum(ps => ps.Song.PlayHistories.Count), // 所有歌曲的播放次數總和
                 LikeCount = p.Favoriteplaylists.Count(), // 歌單的喜歡次數
                 Songs = p.PlayListSongs.Select(ps => new SongDetail
                 {
                     SongId = ps.SongId,
                     SongName = ps.Song.SongName,
                     ArtistId= ps.Song.ArtistNavigation.UserId,
                     SongArtist = ps.Song.ArtistNavigation.UserName!, // 歌曲作者
                     SongCoverPhotoPath = ps.Song.CoverPhotoPath // 歌曲封面
                 }).ToList() // 歌單內每首歌曲的詳細資訊
             }).ToListAsync();
            return View(songViewModels.Any() ? songViewModels : null);
        }

    }
}
