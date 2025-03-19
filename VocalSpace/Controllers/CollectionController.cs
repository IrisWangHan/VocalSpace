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
        public async Task<IActionResult> like()
        {
            string? account = HttpContext.Session.GetString("UserAccount");
            var songdata = await _context.Songs
                .Include(s => s.ArtistNavigation)
                .Include(s => s.LikeSongs)
                .Where(s => s.ArtistNavigation.Account == account 
                        & s.ArtistNavigation.UserId == s.LikeSongs.FirstOrDefault()!.UserId 
                        & s.SongId == s.LikeSongs.FirstOrDefault()!.SongId)
                .Select(s => new SongViewModel
                {
                    SongId = s.SongId,
                    SongCoverPhotoPath = s.CoverPhotoPath,
                    SongName = s.SongName,
                    UserName = s.ArtistNavigation.UserName!,
                    LikeId = s.LikeSongs.FirstOrDefault()!.LikeId
                }).FirstOrDefaultAsync();

            return View(songdata);
        }
        [SessionToLogin]
        public IActionResult mylist()
        {
            return View();
        }
        [SessionToLogin]
        public IActionResult playrecord()
        {
            return View();
        }
        [SessionToLogin]
        public IActionResult booking()
        {
            return View();
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


    }
}
