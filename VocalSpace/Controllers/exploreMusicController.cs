using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Search;

namespace VocalSpace.Controllers
{
    public class exploreMusicController : Controller
    {
        private readonly VocalSpaceDbContext _context;
        //  type 代表歌曲類別
        private static int type;
        //  ExploreResult : 歌曲資訊 (歌名，歌手名，圖，喜歡數)
        private static List<SongInfoDTO>? ExploreResult;
        public exploreMusicController(VocalSpaceDbContext context)
        {
            _context = context;
        }
        //   歌曲資訊 LINQ 結果
        private IQueryable<SongInfoDTO> ExploreSong( )
        {
            IQueryable<SongInfoDTO> a = from song in _context.Songs
                                                         join user in _context.Users
                                                         on song.Artist equals user.UserId
                                                         join category in _context.SongCategories
                                                         on song.SongCategoryId equals category.SongCategoryId
                                                         select new SongInfoDTO { CoverPhotoPath = song.CoverPhotoPath, SongName = song.SongName, UserName = user.UserName, LikeCount = song.LikeCount, SongCategoryId = category.SongCategoryId };
            return a;
        }
        public IActionResult ExploreMusicAll()
        {
            type = 0;
   
            ExploreResult = ExploreSong().ToList();
            return View(ExploreResult);
        }
        public IActionResult ExploreMusicRock()
        {
            type = 1;
            ExploreResult = ExploreSong().Where(song => song.SongCategoryId == type).ToList();
            return View( ExploreResult );
        }
        public IActionResult ExploreMusicFolk()
        {
            type = 2;   //  Folk type == 2

            ExploreResult = ExploreSong().Where(song => song.SongCategoryId == type).ToList();
            return View(ExploreResult);
        }
        public IActionResult ExploreMusicHiphop()
        {
            type = 3;   //  Hiphop type == 3

            ExploreResult = ExploreSong().Where(song => song.SongCategoryId == type).ToList();
            return View(ExploreResult);
        }
        public IActionResult ExploreMusicCitypop()
        {
            type = 4;  //  Citypop type == 4

            ExploreResult = ExploreSong().Where(song => song.SongCategoryId == type).ToList();
            return View(ExploreResult);
        }
        public IActionResult ExploreMusicEDM()
        {
            type = 5;  //  Edm type == 5

            ExploreResult = ExploreSong().Where(song => song.SongCategoryId == type).ToList();
            return View(ExploreResult);
        }
        public IActionResult ExploreMusicExplore()
        {
            type = 6;   //  Edm type == 6

            ExploreResult = ExploreSong().Where(song => song.SongCategoryId == type).ToList();
            return View(ExploreResult);
        }
        [HttpGet]
        public IActionResult RankType()
        {

            switch (Request.Query["rt"])
            {
                case "mostlike":
                    //  1. type == 0 ，代表全部歌曲，2. type != 0，篩選同類別歌曲(SongCategoryId == type)，2種情況之後依喜歡數排序(由大到小)
                    ExploreResult = ( type == 0 ) ? ExploreSong().OrderByDescending(song => song.LikeCount).ToList() : ExploreSong().Where(song => song.SongCategoryId == type).OrderByDescending(song => song.LikeCount).ToList();
                    break;
                default:
                    break;
            }
            return PartialView("_partialViewSong", ExploreResult);

        }
        [HttpGet]
        public IActionResult loadmore()
        {
            switch (Request.Query["type"])
            {
                case "song":
                    return PartialView("_partialViewSong");
                case "songlist":
                    return PartialView("_partialViewSonglist");
                case "artist":
                    return PartialView("_partialViewArtist");
                default:
                    return PartialView("_partialViewSong");
            }

        }

    }
}
