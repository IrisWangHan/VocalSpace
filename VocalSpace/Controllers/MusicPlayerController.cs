using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VocalSpace.Models.ViewModel.MusicPlayer;
using VocalSpace.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;
namespace VocalSpace.Controllers
{
    public class MusicPlayerController : Controller
    {
        private readonly IMusicPlayerService _musicPlayerService;

        public MusicPlayerController(IMusicPlayerService musicPlayerService)
        {
            _musicPlayerService = musicPlayerService;
        }

        /// <summary>
        /// 取得預設音樂 首次進入網站時使用
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetDefaultMusic(double playedDuration)
        {
            var musicModel = _musicPlayerService.GetDefaultMusic(playedDuration);
            if (musicModel != null)
            {
                HttpContext.Session.SetString("MusicPlayerModel", JsonConvert.SerializeObject(musicModel));
                return PartialView("_MusicPlayerPartial", musicModel);
            }
            return NoContent();
        }

        /// <summary>
        /// 取得音樂 撥放單一歌曲時使用
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSong(int songId)
        {
            var musicModel = await _musicPlayerService.GetSong(songId);
            if (musicModel != null)
            {
                HttpContext.Session.SetString("MusicPlayerModel", JsonConvert.SerializeObject(musicModel));
                return PartialView("_MusicPlayerPartial", musicModel);
            }
            return NoContent();
        }

        /// <summary>
        /// 取得音樂 撥放清單歌曲時使用
        /// </summary>
        /// <param name="playListId">撥放清單</param>
        /// <param name="songId">可選 指定撥放歌曲</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPlayListSong(int playListId, int songId = 0)
        {

            var musicModel =await _musicPlayerService.GetPlayListSong(playListId, songId);
            if (musicModel != null)
            {
                HttpContext.Session.SetString("MusicPlayerModel", JsonConvert.SerializeObject(musicModel));
                return PartialView("_MusicPlayerPartial", musicModel);
            }
            return NoContent();
        }

        [HttpGet]
        public IActionResult PlayNextSong()
        {
            MusicPlayModel musicObject = new MusicPlayModel();
            var musicPlayerModel = HttpContext.Session.GetString("MusicPlayerModel");

            if (string.IsNullOrEmpty(musicPlayerModel))
            {
                return NoContent();
            }
            try
            {
                musicObject = JsonConvert.DeserializeObject<MusicPlayModel>(musicPlayerModel) ?? new MusicPlayModel();
            }
            catch (Exception ex)
            {
                return NoContent();
            }

            var musicModel = _musicPlayerService.PlayNextSong(musicObject);
            if (musicModel != null)
            {
                HttpContext.Session.SetString("MusicPlayerModel", JsonConvert.SerializeObject(musicModel));
                return PartialView("_MusicPlayerPartial", musicModel);
            }
            return NoContent();
        }

        [HttpGet]
        public IActionResult PlayPreSong()
        {
            MusicPlayModel musicObject = new MusicPlayModel();
            var musicPlayerModel = HttpContext.Session.GetString("MusicPlayerModel");

            if (string.IsNullOrEmpty(musicPlayerModel))
            {
                return NoContent();
            }
            try
            {
                musicObject = JsonConvert.DeserializeObject<MusicPlayModel>(musicPlayerModel) ?? new MusicPlayModel();
            }
            catch (Exception ex)
            {
                return NoContent();
            }
            var musicModel = _musicPlayerService.PlayPreSong(musicObject);
            if (musicModel != null)
            {
                HttpContext.Session.SetString("MusicPlayerModel", JsonConvert.SerializeObject(musicModel));
                return PartialView("_MusicPlayerPartial", musicModel);
            }
            return NoContent();

        }

        public class UpdateTimeRequestDTO
        {
            public double playedDuration { get; set; }
        }
    }
}
