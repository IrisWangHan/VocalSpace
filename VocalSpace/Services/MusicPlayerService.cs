using System.Linq;
using System.Collections.Generic;
using VocalSpace.Models.ViewModel.MusicPlayer;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models.ViewModel.Selection;
using VocalSpace.Models;
using Org.BouncyCastle.Utilities;

namespace VocalSpace.Services
{
    public interface IMusicPlayerService
    {
        MusicPlayModel GetDefaultMusic(double playedDuration);
        Task<MusicPlayModel> GetSong(int songId);
        Task<MusicPlayModel> GetPlayListSong(int playListId, int songId = 0);
        MusicPlayModel PlayNextSong(MusicPlayModel musicObject);
        MusicPlayModel PlayPreSong(MusicPlayModel musicObject);
    }

    public class MusicPlayerService : IMusicPlayerService
    {
        private readonly VocalSpaceDbContext _context;
        public static MusicPlayModel CurrentModel { get; private set; }

        public MusicPlayerService(VocalSpaceDbContext context)
        {
            _context = context;
            InitializeDefaultMusic();
        }

        private void InitializeDefaultMusic()
        {
            List<MusicModel> defaultMusics = new List<MusicModel>
            {
                new MusicModel
                {
                    PlayListId = 0,
                    PlayListName = "",
                    SongId = 0,
                    SongName = "目前尚未撥放",
                    ArtistName = "",
                    CoverPhotoPath = "/image/Song/default.jpg",
                    SongPath = "",
                },
            };

            CurrentModel = new MusicPlayModel
            {
                CurrentArtistName = defaultMusics[0].ArtistName,
                CurrentCoverPhotoPath = defaultMusics[0].CoverPhotoPath,
                CurrentSongPath = defaultMusics[0].SongPath,
                CurrentSongName = defaultMusics[0].SongName,
                CurrentSongId = defaultMusics[0].SongId,
                CurrentPlayedDuration = 0,
                musicModels = defaultMusics
            };
        }

        public MusicPlayModel GetDefaultMusic(double playedDuration)
        {
            CurrentModel.CurrentPlayedDuration = (int)playedDuration;
            return CurrentModel;
        }

        public async Task<MusicPlayModel> GetSong(int songId)
        {
            var song = await _context.Songs
                .Join(_context.Users, s => s.Artist, a => a.UserId, (s, a) => new { s, a })
                .Where(sa => sa.s.SongId == songId)
                .Select(sa => new MusicModel
                {
                    PlayListId = 0,
                    PlayListName = "",
                    SongId = (int)sa.s.SongId,
                    SongName = sa.s.SongName,
                    CoverPhotoPath = sa.s.CoverPhotoPath,
                    ArtistName = sa.a.UserName,
                    SongPath = sa.s.SongPath,
                })
                .FirstOrDefaultAsync();

            if (song != null)
            {
                CurrentModel.CurrentArtistName = song.ArtistName;
                CurrentModel.CurrentCoverPhotoPath = song.CoverPhotoPath;
                CurrentModel.CurrentSongPath = song.SongPath;
                CurrentModel.CurrentSongName = song.SongName;
                CurrentModel.CurrentSongId = song.SongId;
                CurrentModel.CurrentPlayedDuration = 0;
                CurrentModel.musicModels = new List<MusicModel> { song };
            }
            return CurrentModel;
        }


        public async Task<MusicPlayModel> GetPlayListSong(int playListId, int songId = 0)
        {
            MusicModel currentSong = new MusicModel();
            try
            {
                var songs = await _context.Songs
                 .Join(_context.PlayListSongs, s => s.SongId, ps => ps.SongId, (s, ps) => new { s, ps })
                 .Join(_context.Users, sps => sps.s.Artist, u => u.UserId, (sps, u) => new { sps.s, sps.ps, u })
                 .Join(_context.PlayLists, spsu => spsu.ps.PlayListId, pl => pl.PlayListId, (spsu, pl) => new { spsu.s, spsu.ps, spsu.u, pl })
                 .Where(spsupl => spsupl.pl.PlayListId == playListId)
                 .Select(spsupl => new MusicModel
                 {
                     SongId = (int)spsupl.s.SongId,
                     PlayListId = (int)spsupl.pl.PlayListId,
                     PlayListName = spsupl.pl.Name,
                     SongName = spsupl.s.SongName,
                     CoverPhotoPath = spsupl.s.CoverPhotoPath,
                     ArtistName = spsupl.u.UserName,
                     SongPath = spsupl.s.SongPath,
                 })
                 .ToListAsync();
                //如果播放清單中沒有歌曲，則返回當前預設播放清單
                if (songs.Count == 0)
                {
                    return CurrentModel;
                }


                if (songId != 0)
                {
                    //撥放 指定清單歌曲
                    //判斷歌曲是否存在於播放清單中
                    var isExists = songs.Any(s => s.SongId == songId);
                    if (isExists) { currentSong = songs.First(s => s.SongId == songId); }
                }
                else
                {
                    //播放 清單中的第一首歌
                    currentSong = songs.First();
                }
                CurrentModel.CurrentArtistName = currentSong.ArtistName;
                CurrentModel.CurrentCoverPhotoPath = currentSong.CoverPhotoPath;
                CurrentModel.CurrentSongPath = currentSong.SongPath;
                CurrentModel.CurrentSongName = currentSong.SongName;
                CurrentModel.CurrentSongId = currentSong.SongId;
                CurrentModel.CurrentPlayedDuration = 0;
                CurrentModel.musicModels = songs;

                return CurrentModel;
            }
            catch (Exception ex)
            {
                // 記錄異常信息
                Console.WriteLine($"查詢播放清單歌曲時發生錯誤: {ex.Message}");
                return CurrentModel;
            }
        }

        public MusicPlayModel PlayNextSong(MusicPlayModel musicObject)
        {
            var musicModels = musicObject.musicModels;

            //找到當前歌曲的索引
            var currentIndex = musicModels.FindIndex(x => x.SongId == musicObject.CurrentSongId);
            if (currentIndex < musicModels.Count - 1)
            {
                musicObject.CurrentArtistName = musicModels[currentIndex + 1].ArtistName;
                musicObject.CurrentCoverPhotoPath = musicModels[currentIndex + 1].CoverPhotoPath;
                musicObject.CurrentSongPath = musicModels[currentIndex + 1].SongPath;
                musicObject.CurrentSongName = musicModels[currentIndex + 1].SongName;
                musicObject.CurrentSongId = musicModels[currentIndex + 1].SongId;
                musicObject.CurrentPlayedDuration = 0;

                return musicObject;
            }
            return musicObject;


        }

        public MusicPlayModel PlayPreSong(MusicPlayModel musicObject)
        {
            var musicModels = musicObject.musicModels;
            //找到當前歌曲的索引
            var currentIndex = musicModels.FindIndex(x => x.SongId == musicObject.CurrentSongId);
            if (currentIndex > 0 )
            {
                musicObject.CurrentArtistName = musicModels[currentIndex - 1].ArtistName;
                musicObject.CurrentCoverPhotoPath = musicModels[currentIndex - 1].CoverPhotoPath;
                musicObject.CurrentSongPath = musicModels[currentIndex - 1].SongPath;
                musicObject.CurrentSongName = musicModels[currentIndex - 1].SongName;
                musicObject.CurrentSongId = musicModels[currentIndex - 1].SongId;
                musicObject.CurrentPlayedDuration = 0;
            }
                return musicObject;
        }
    }
}