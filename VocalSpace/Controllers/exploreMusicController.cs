using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Search;
using VocalSpace.Services;

namespace VocalSpace.Controllers
{
    public class exploreMusicController : Controller
    {
        private readonly SearchExploreService _SearchExploreService;
        //  type 代表歌曲類別
        private static byte type;
        //  ExploreResult : 歌曲資訊 (歌名，歌手名，圖，喜歡數)
        private static List<SongInfoDTO>? ExploreResult;
        public exploreMusicController(SearchExploreService SearchExploreService)
        {
            _SearchExploreService = SearchExploreService;
        }
        
        public IActionResult ExploreMusicAll(string id)
        {

            type = Convert.ToByte(id);
            ViewData["type"] = type;
            //  透過Service取得歌曲資訊，篩選(Where)，排序(OrderByDescending)
            ExploreResult = _SearchExploreService.ExploreSongWhereOrder(type, "new");
            return View(ExploreResult);
        }
        //  Rock / Pop / Jazz / HipHop / Classical...
        [HttpGet]
        public IActionResult MusicType(string id)
        {
            //  id = 最新 / 最多播放 / 最多喜歡
            type = Convert.ToByte(id);
            ExploreResult = _SearchExploreService.ExploreSongWhereOrder(type, "new");
            return PartialView("_partialViewSong", ExploreResult);
        }

        [HttpGet]
        public IActionResult RankType(string id)
        {
            //  id = 最新 / 最多播放 / 最多喜歡         
            ExploreResult = _SearchExploreService.ExploreSongWhereOrder(type, id);
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
