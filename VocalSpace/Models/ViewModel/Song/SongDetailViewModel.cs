namespace VocalSpace.Models.ViewModel.Song
{
    //把所有歌曲頁需要的資料都放在這個 ViewModel
    //這樣partial view就可以直接使用這個ViewModel
    //包含歌曲、歌手、類型

    public class SongDetailViewModel
    {
        public SongInfoViewModel? SongInfo { get; set; }
        public SongLikeViewModel? SongLike { get; set; }
        public SongLyricsViewModel? SongLyrics { get; set; }
        public SongCommentViewModel? SongComment { get; set; }
    }
    public class SongInfoViewModel
    {
        public required VocalSpace.Models.Song Song { get; set; }
        public required VocalSpace.Models.User User { get; set; }

        public required VocalSpace.Models.UsersInfo UsersInfo { get; set; }
        public required VocalSpace.Models.SongCategory SongCategory { get; set; }

        public List<PlayList> PlayLists { get; set; } = new List<PlayList>();
    }

    public class SongLikeViewModel
    {
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
    }

    public class SongLyricsViewModel
    {
        public string? Lyrics { get; set; }
    }

    public class SongCommentViewModel
    {
        public List<SongComment>? Comments { get; set; }
    }
}
