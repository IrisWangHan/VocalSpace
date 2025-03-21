using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Global;

namespace VocalSpace.Services
{
    public class ModalDataService
    {
        private readonly VocalSpaceDbContext _context;

        public ModalDataService(VocalSpaceDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得"加入歌單Modal"需要的歌單列表，以及歌曲ID，回傳AddToPlaylistViewModel，若無歌單則回傳 null
        /// </summary>
        public async Task<AddToPlaylistViewModel?> GetPlaylistModalData(long userId, long songId)
        {
            var playlists = await _context.PlayLists
                .Where(p => p.UserId == userId)
                .Select(p => new PlaylistViewModel
                {
                    PlayListID = p.PlayListId,
                    PlayListName = p.Name,
                    SonginPlaylist = p.PlayListSongs.Any(ps => ps.SongId == songId)
                })
                .ToListAsync();


            return new AddToPlaylistViewModel
            {
                SongId = songId,
                Playlists = playlists
            };
        }
        /// <summary>
        /// 判斷歌曲是否在歌單中，並執行新增或刪除
        /// </summary>
        public async Task<(bool isSuccess, bool isAdded)> AddToPlaylist(long playlistId, long songId)
        {
            try
            {
                // 檢查這首歌是否已在該歌單中
                bool SongInPlaylist = await _context.PlayListSongs
                    .AnyAsync(ps => ps.PlayListId == playlistId && ps.SongId == songId);

                if (SongInPlaylist)
                {
                    // 歌曲已存在 → 執行刪除
                    await _context.PlayListSongs
                        .Where(ps => ps.PlayListId == playlistId && ps.SongId == songId)
                        .ExecuteDeleteAsync();
                    return (true, false);
                }
                else
                {
                    // 歌曲不存在 → 執行新增
                    _context.PlayListSongs.Add(new PlayListSong
                    {
                        PlayListId = playlistId,
                        SongId = songId,
                    });
                    await _context.SaveChangesAsync();
                    return (true, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"處理歌單時發生錯誤: {ex.Message}");
                return (false, false);
            }
        }

        /// <summary>
        /// 取得"分享歌曲Modal"需要的歌曲資訊，回傳ShareSongViewModel
        /// </summary>
        public async Task<ShareSongViewModel?> GetShareSongModalData(long songId)
        {
            var song = await _context.Songs
                .Include(s => s.ArtistNavigation) 
                .FirstOrDefaultAsync(s => s.SongId == songId);
            if (song == null)
            {
                return null;
            }
            return new ShareSongViewModel
            {
                SongId = song.SongId,
                SongName = song.SongName,
                ArtistName = song.ArtistNavigation.UserName ?? song.ArtistNavigation.Account,
                CoverImage = song.CoverPhotoPath,
                ShareUrl = $"https://vocalspace.com/song/{song.SongId}"
            };

        }
    }
}
