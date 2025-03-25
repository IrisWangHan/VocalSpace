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
        /// 取得 SongButtonViewModel的資料，以便Controller使用
        /// </summary>
        public async Task<SongButtonViewModel> GetSongButtonViewModel(long songId, long? currentUserId)
        {
            var song = await _context.Songs
                .Where(s => s.SongId == songId)
                .Select(s => new
                {
                    s.SongName,
                    s.LikeCount
                })
                .FirstOrDefaultAsync();

            //isliked 引用service中的IsLikedAsync方法，確認是否有對歌曲按讚
            return new SongButtonViewModel
            {
                SongName = song!.SongName,
                LikeCount = song.LikeCount+_context.LikeSongs.Count(ls => ls.SongId == songId),
                SongId = songId,
                isliked = currentUserId.HasValue
        ? await IsLikedAsync(currentUserId.Value, songId)
        : false
            };
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
        /// <summary>
        /// 判斷歌曲有沒有按讚，並執行新增或刪除
        /// </summary>
        public async Task<(bool isSuccess, bool isliked, int likeCount)> AddToLikesong(long userid, long songId)
        {
            try
            {
                var song = await _context.Songs
               .Where(s => s.SongId == songId)
               .Select(s => new { s.LikeCount })
               .FirstOrDefaultAsync();

                int defaultLikeCount = song?.LikeCount ?? 0;

                var songLike = await _context.LikeSongs
                    .FirstOrDefaultAsync(ls => ls.UserId == userid && ls.SongId == songId);

                if (songLike != null)
                {
                    // 已經按讚 → 取消讚
                    _context.LikeSongs.Remove(songLike);
                }
                else
                {
                    // 沒有按讚 → 新增讚
                    _context.LikeSongs.Add(new LikeSong
                    {
                        UserId = userid,
                        SongId = songId,
                    });
                }

                await _context.SaveChangesAsync();

                // 重新計算讚數
                int addLikeCount = await _context.LikeSongs.CountAsync(ls => ls.SongId == songId);
                int likeCount = defaultLikeCount + addLikeCount;
                return (true, songLike == null, likeCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"處理歌單時發生錯誤: {ex.Message}");
                return (false, false, 0);
            }
        }


        /// <summary>
        /// 判斷歌曲是否已按讚
        /// </summary>
        public async Task<bool> IsLikedAsync(long userId, long songId)
        {
            return await _context.LikeSongs
                .AnyAsync(ls => ls.UserId == userId && ls.SongId == songId);
        }

        /// <summary>
        /// 取得 PlayListButtonViewModel 的資料，以便Controller使用
        /// </summary>
        public async Task<PlayListButtonViewModel> GetPlayListButtonViewModel(long playListId, long? UserId)
        {
           var playlist = await _context.PlayLists
                .Where(p => p.PlayListId == playListId)
                .Select(p => new
                {
                    p.PlayListId,
                    p.Name,
                })
                .FirstOrDefaultAsync();
            return new PlayListButtonViewModel
            {    
                PlayListId = playListId,
                Name = playlist!.Name,
                //  isliked = true 代表已喜歡
                isliked = (UserId.HasValue) ? await IsLikedPlayListAsync(UserId.Value, playListId) : false
            };        
        }

        /// <summary>
        /// 判斷歌單是否已喜歡
        /// </summary>
        public async Task<bool> IsLikedPlayListAsync(long userId, long playlistid)
        {
            //  true 代表存在(已喜歡)
            return await _context.Favoriteplaylists
                .AnyAsync(p => p.UserId == userId && p.PlayListId == playlistid);
        }

        /// <summary>
        /// 判斷喜歡歌單是否已喜歡
        /// </summary>
        public async Task<(bool isSuccess, bool isliked)> AddLikePlaylistAsync(long userId, long playlistid)
        {
            try
            {
                var playlist = await _context.Favoriteplaylists
                    .FirstOrDefaultAsync(p => p.UserId == userId && p.PlayListId == playlistid);
                if (playlist != null)
                {
                    // 已經喜歡 → 取消喜歡
                    _context.Favoriteplaylists.Remove(playlist);
                }
                else
                {
                    // 沒有喜歡 → 新增喜歡
                    _context.Favoriteplaylists.Add(new Favoriteplaylist
                    {
                        UserId = userId,
                        PlayListId = playlistid
                    });
                }
                await _context.SaveChangesAsync();
                //  isliked = true 代表已喜歡
                return (true, playlist == null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"處理喜歡歌單時發生錯誤: {ex.Message}");
                return (false, false);
            }
        }
    }
}
