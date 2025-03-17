using VocalSpace.Models.ViewModel.Global;

namespace VocalSpace.Models.ViewModel.Song
{
    //把所有歌曲頁需要的資料都放在這個 ViewModel
    //這樣partial view就可以直接使用這個ViewModel
    //包含歌曲、歌手、類型

    public class SongDetailViewModel
    {

    }
    public class SongInfoViewModel
    {
        public required VocalSpace.Models.Song Song { get; set; }
        public required VocalSpace.Models.User User { get; set; }

        public required VocalSpace.Models.UsersInfo UsersInfo { get; set; }
        public required VocalSpace.Models.SongCategory SongCategory { get; set; }

        public List<PlayList> PlayLists { get; set; } = new List<PlayList>();

        //把留言區的 ViewModel 直接包進來
        public CommentSectionViewModel CommentSection { get; set; } = new CommentSectionViewModel();
    }

    public class SongLikeViewModel
    {
        public int LikeCount { get; set; }
        public bool IsLiked { get; set; }
    }
}
