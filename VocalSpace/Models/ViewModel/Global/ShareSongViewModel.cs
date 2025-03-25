namespace VocalSpace.Models.ViewModel.Global
{
    public class ShareSongViewModel
    {
        public long SongId { get; set; } // 歌曲 ID
        public string SongName { get; set; } = null!; // 歌曲名稱
        public string ArtistName { get; set; } = null!; // 歌手名稱
        public string CoverImage { get; set; } = null!; // 歌曲圖片
        public string ShareUrl { get; set; } = null!; // 分享網址
    }
}
