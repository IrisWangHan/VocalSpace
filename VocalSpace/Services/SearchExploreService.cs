using Microsoft.EntityFrameworkCore;
using VocalSpace.Models;
using VocalSpace.Models.ViewModel.Search;
using VocalSpace.Models.ViewModel.Selection;

namespace VocalSpace.Services
{
    public class SearchExploreService
    {
        private readonly VocalSpaceDbContext _context;
        //  ExploreResult : 歌曲資訊 (歌名，歌手名，圖，喜歡數)
        private static List<SongInfoDTO>? ExploreResult;
        public SearchExploreService(VocalSpaceDbContext context)
        {
            _context = context;
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

        public async Task<List<ArtistDTO>> LINQartist(string q)
        {
            var Artists = from users in _context.Users
                          join infos in _context.UsersInfos on users.UserId equals infos.UserId
                          where users.UserName!.Contains(q!)
                          select new ArtistDTO { AvatarPath = infos.AvatarPath, UserName = users.UserName };
            //  1.先搜尋歌手名稱完全符合關鍵字
            //  2.有包含關鍵字的歌手
            Artists = Artists.OrderByDescending(data => data.UserName == q).ThenByDescending(data => data.UserName!.Contains(q!));
            return await Artists.ToListAsync();
        }

        public async Task<List<PlaylistDTO>> LINQplaylist(string q)
        {
            var Playlists = from user in _context.Users
                            join playlist in _context.PlayLists on user.UserId equals playlist.UserId
                            join playlistsong in _context.PlayListSongs on playlist.PlayListId equals playlistsong.PlayListId
                            where playlist.Name.Contains(q!) || user.UserName!.Contains(q!)
                            group new { user, playlist, playlistsong } by new
                            {
                                user.UserName,
                                playlist.PlayListId,
                                playlist.Name,
                                playlist.CoverImagePath
                            } into g
                            select new PlaylistDTO { PlayListId = g.Key.PlayListId, Name = g.Key.Name, UserName = g.Key.UserName, CoverImagePath = g.Key.CoverImagePath };
            Playlists = Playlists.OrderByDescending(data => data.Name == q)
                 .ThenByDescending(data => data.UserName == q)
                 .ThenByDescending(data => data.Name!.Contains(q!));
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
