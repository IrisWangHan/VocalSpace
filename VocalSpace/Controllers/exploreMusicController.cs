using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Search;

namespace VocalSpace.Controllers
{
    public class exploreMusicController : Controller
    {
        private readonly VocalSpaceDbContext _context;
        //  type 代表歌曲類別
        private static byte type;
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
                                                         select new SongInfoDTO 
                                                         { CoverPhotoPath = song.CoverPhotoPath, SongName = song.SongName, UserName = user.UserName, LikeCount = song.LikeCount, CreateTime = song.CreateTime, PlayCount = song.PlayCount, SongCategoryId = category.SongCategoryId };
            return a;
        }

        //  對LINQ結果進行篩選(Where)，排序(OrderByDescending)
        private void SongData(string rank)
        {
            //  type == 0 代表全部歌曲
            IQueryable<SongInfoDTO> result = (type == 0) ? ExploreSong() : ExploreSong().Where(song => song.SongCategoryId == type);
            switch (rank) 
            {
                case "mostlike":
                    ExploreResult = result.OrderByDescending(song => song.LikeCount).ToList();
                    break;
                case "new":
                    ExploreResult = result.OrderByDescending(song => song.CreateTime).ToList();
                    break;
                case "mostplay":
                    ExploreResult = result.OrderByDescending(song => song.PlayCount).ToList();
                    break;
            }
        }
        public IActionResult ExploreMusicAll(string id)
        {
            type = Convert.ToByte(id);
            ViewData["type"] = type;
            SongData("new");
            return View(ExploreResult);
        }
        
        [HttpGet]
        public IActionResult RankType(string id)
        {
            SongData(id);
            
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
