//using System.Net.Http;
//using System.Threading.Tasks;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;
//using Mono.TextTemplating;
//using VocalSpace.Models;
//using VocalSpace.Models.ViewModel.MusicPlayer;
//using VocalSpace.Models.ViewModel.Selection;

//namespace VocalSpace.Services
//{
//    public interface IMusicPlayerService2
//    {
//        public MusicPlayModel GetCurrentMusic(double playedDuration);
//        public MusicPlayModel ChangeCurrentMusic(int songId);
//        public MusicPlayModel UpdateMusic(int userId);
//    }

//    public class MusicPlayerService2 : IMusicPlayerService2
//    {
//        private readonly VocalSpaceDbContext _context;
//        private static int SongId { get; set; }
//        private static string SongName { get; set; }
//        private static string ArtistName { get; set; }
//        private static string CoverPhotoPath { get; set; }
//        private static string SongPath { get; set; }
//        private static List<MusicModel> musicModels { get; set; } = new List<MusicModel>();
//        public static MusicPlayModel CurrentModel { get; private set; }
//        public MusicPlayerService2(VocalSpaceDbContext context)
//        {
//            _context = context;
//            //取得預設音樂資料 梅登入
//            //如果有登入 會自動帶 撥放清單所有音樂
//            //// 設定預設值
//            //SongId = 1;
//            //SongName = "晴天";
//            //ArtistName = "周杰倫";
//            //CoverPhotoPath = "/image/Song/default.jpg";
//            //SongPath = "/audio/default.mp3";

//            //// 預設加入這首歌到列表
//            //var defaultMusic = new MusicModel
//            //{
//            //    SongId = SongId,
//            //    SongName = SongName,
//            //    ArtistName = ArtistName,
//            //    CoverPhotoPath = CoverPhotoPath,
//            //    SongPath = SongPath,
//            //};
//            //musicModels.Add(defaultMusic);

//        }

//        /// <summary>
//        /// 更新音樂清單
//        /// </summary>
//        /// <param name="userId"></param>
//        public async MusicPlayModel UpdateMusic(int userId=0)
//        {
             
//            if (userId != 0)
//            {
//                // 2️ 已報名，查詢對應的歌曲 左外聯結 (GroupJoin() + Select())
//                musicModels = await _context.Songs
//                   .FromSqlRaw("SELECT A.UserID,UserName, Email,AvatarPath FROM Users A INNER JOIN UsersInfo B ON A.UserID=B.UserID WHERE A.UserID=@UserID", new SqlParameter("@UserID", UserID))
//                    .Select(x => new MusicModel
//                    {
//                        SongId = x.song.SongId,
//                        SongName = x.song.SongName,
//                        SongDescription = x.song.SongDescription,
//                        CoverPhotoPath = x.song.CoverPhotoPath,
//                        SongPath = x.song.SongPath,
//                        LikeCount = x.song.LikeCount,
//                        SelectionDetailId = x.SelectionDetailId ?? 0  // 修正：null 改為 0
//                    })
//                    .ToListAsync();
//                // 取得使用者的音樂清單
//                //var userMusicList = _musicPlayerRepository.GetMusicList(userId);
//                //if (userMusicList != null)
//                //{
//                //    musicModels = userMusicList;
//                //}
//            }
//            else
//            {
//                List<MusicModel> defaultMusics = new List<MusicModel>();
//                // 設定預設值
//                var defaultMusic1 = new MusicModel
//                {
//                    SongId = 1,
//                    SongName = "晴天",
//                    ArtistName = "周杰倫",
//                    CoverPhotoPath = "/image/Song/default.jpg",
//                    SongPath = "/audio/Morning.mp3",
//                };

//                var defaultMusic2 = new MusicModel
//                {
//                    SongId = 2,
//                    SongName = "夜曲",
//                    ArtistName = "周杰倫",
//                    CoverPhotoPath = "/image/Song/default.jpg",
//                    SongPath = "/audio/Paradise.mp3",
//                };

//                var defaultMusic3 = new MusicModel
//                {
//                    SongId = 3,
//                    SongName = "稻香",
//                    ArtistName = "周杰倫",
//                    CoverPhotoPath = "/image/Song/default.jpg",
//                    SongPath = "/audio/sample.mp3",
//                };

//                defaultMusics.Add(defaultMusic1);
//                defaultMusics.Add(defaultMusic2);
//                defaultMusics.Add(defaultMusic3);
//                // 初始化 MusicPlayModel
//                CurrentModel = new MusicPlayModel
//                {
//                    PlayListId = 0,
//                    PlayListName = "",
//                    CurrentArtistName = defaultMusic1.ArtistName,
//                    CurrentCoverPhotoPath = defaultMusic1.CoverPhotoPath,
//                    CurrentSongPath = defaultMusic1.SongPath,
//                    CurrentSongName = defaultMusic1.SongName,
//                    CurrentSongId = defaultMusic1.SongId,
//                    CurrentPlayedDuration = 0,
//                    musicModels = defaultMusics
//                };
//            }
//        }

//        public MusicPlayModel GetCurrentMusic(double playedDuration)
//        {
//            CurrentModel.CurrentPlayedDuration = (int)playedDuration;
//            return CurrentModel;
//        }
//        public MusicPlayModel ChangeCurrentMusic(int songId)
//        {
//            var changMusic = musicModels.FirstOrDefault(x => x.SongId == songId);
//            if (changMusic != null)
//            {
//                CurrentModel.CurrentArtistName = changMusic.ArtistName;
//                CurrentModel.CurrentPlayedDuration = 0;
//                CurrentModel.CurrentCoverPhotoPath = changMusic.CoverPhotoPath;
//                CurrentModel.CurrentSongPath = changMusic.SongPath;
//                CurrentModel.CurrentSongId = changMusic.SongId;
//                CurrentModel.CurrentSongName = changMusic.SongName;
//            }
//            return CurrentModel;
//        }
//    }
//}
