using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Search;
using VocalSpace.Models.ViewModel.Selection;

namespace VocalSpace.Services
{
    public class SearchExploreService
    {
        private readonly VocalSpaceDbContext _context;
        private readonly UserService _userService;
        //  ExploreResult : 歌曲資訊 (歌名，歌手名，圖，喜歡數)
        private static List<SongInfoDTO>? ExploreResult;
        public SearchExploreService(VocalSpaceDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        //  搜尋歌曲
        public async Task<List<SongInfoDTO>> LINQsong(string q)
        {
            //  將資料透過DTO物件傳遞到前端
            var Songs = _context.Songs.Join(
                    _context.Users,
                    song => song.Artist,
                    user => user.UserId,
                    (song, user) => new SongInfoDTO { SongId = song.SongId, SongName = song.SongName, UserName = user.UserName, CoverPhotoPath = song.CoverPhotoPath, LikeCount = song.LikeCount })
                .Where(data => data.SongName!.Contains(q!) || data.UserName!.Contains(q!))
                .OrderByDescending(data => data.SongName == q)                 //  1.先搜尋歌曲名稱完全符合關鍵字
                .ThenByDescending(data => data.UserName == q)                   //  2.歌手名稱完全符合關鍵字的歌曲
                .ThenByDescending(data => data.SongName!.Contains(q!))     //  3.有包含關鍵字的歌曲名稱
                .ThenByDescending(data => data.UserName!.Contains(q!));     //   4.有包含關鍵字的歌手名稱的歌曲
            return await Songs.ToListAsync();
        }

        public async Task<List<ArtistDTO>> LINQartist(string q, long? userid)
        {
            var Artists = from users in _context.Users
                          join infos in _context.UsersInfos on users.UserId equals infos.UserId
                          where users.UserName!.Contains(q!)
                          select new ArtistDTO { UserId = users.UserId, AvatarPath = infos.AvatarPath, UserName = users.UserName };
            //  1.先搜尋歌手名稱完全符合關鍵字
            //  2.有包含關鍵字的歌手
            Artists = Artists.OrderByDescending(data => data.UserName == q).ThenByDescending(data => data.UserName!.Contains(q!));

            var result = await Artists.ToListAsync();

            //  使用者未登入
            if (userid == null)
            {
                return result;
            }

            //  使用者已登入，檢查是否追蹤 artist
            foreach (var artist in result)
            {
                //  判斷是否追蹤，true:已追蹤，false:未追蹤
                artist.isFollowing = await _userService.IsFollowingAsync(userid.Value, artist.UserId);
            }
            return result;
        }

        public async Task<List<PlaylistDTO>> LINQplaylist(string q)
        {
            var Playlists = from user in _context.Users
                            join playlist in _context.PlayLists on user.UserId equals playlist.UserId
                            join playlistsong in _context.PlayListSongs on playlist.PlayListId equals playlistsong.PlayListId into playlistGroup
                            where playlist.Name.Contains(q!) || user.UserName!.Contains(q!)
                            group new { user, playlist, playlistGroup } by new
                            {
                                user.UserName,
                                playlist.PlayListId,
                                playlist.Name,
                                playlist.CoverImagePath
                            } into g
                            select new PlaylistDTO
                            {
                                PlayListId = g.Key.PlayListId,
                                Name = g.Key.Name,
                                UserName = g.Key.UserName,
                                CoverImagePath = g.Key.CoverImagePath,
                                SongCount = g.SelectMany(x => x.playlistGroup).Count() // 統計播放清單中的歌曲數量
                            };

            // 排序播放清單
            Playlists = Playlists.OrderByDescending(data => data.Name == q)
                                 .ThenByDescending(data => data.UserName == q)
                                 .ThenByDescending(data => data.Name!.Contains(q!));

            // 執行查詢並返回結果
            return await Playlists.ToListAsync();
        }

        //  發現音樂
        private IQueryable<SongInfoDTO> ExploreSong()
        {
            var a = from song in _context.Songs
                                        join user in _context.Users
                                        on song.Artist equals user.UserId
                                        join category in _context.SongCategories
                                        on song.SongCategoryId equals category.SongCategoryId
                                        select new SongInfoDTO
                                        //  LikeCount = song.LikeCount(預設值) + 後續增加的喜歡數
                                        { SongId = song.SongId, CoverPhotoPath = song.CoverPhotoPath, SongName = song.SongName, UserName = user.UserName, LikeCount = song.LikeCount + _context.LikeSongs.Count(ls => ls.SongId == song.SongId) , CreateTime = song.CreateTime, PlayCount = song.PlayCount, SongCategoryId = category.SongCategoryId };
            return a;
        }
        //  對LINQ結果進行篩選(Where)，排序(OrderByDescending)
        public List<SongInfoDTO> ExploreSongWhereOrder(byte type, string rank)
        {
            //  type == 0 代表全部歌曲
            IQueryable<SongInfoDTO> result = (type == 0) ? ExploreSong() : ExploreSong().Where(song => song.SongCategoryId == type);
            switch (rank)
            {    //  song.LikeCount+_context.LikeSongs.Count(ls => ls.SongId == songId)
                case "mostlike":
                    ExploreResult = result.OrderByDescending(songinfo => songinfo.LikeCount).ToList();
                    return ExploreResult;
                case "new":
                    ExploreResult = result.OrderByDescending(songinfo => songinfo.CreateTime).ToList();
                    return ExploreResult;
                case "mostplay":
                    ExploreResult = result.OrderByDescending(songinfo => songinfo.PlayCount).ToList();
                    return ExploreResult;
                default:
                    goto case "new";
            }
        }
    }
}
