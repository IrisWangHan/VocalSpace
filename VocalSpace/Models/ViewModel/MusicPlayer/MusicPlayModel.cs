using System.ComponentModel;

namespace VocalSpace.Models.ViewModel.MusicPlayer
{

    public class MusicPlayModel
    {

        [Description("演出者")]
        public string? CurrentArtistName { get; set; }

        [Description("當前專輯封面圖片路徑")]
        public string? CurrentCoverPhotoPath { get; set; }

        [Description("當前音樂檔案路徑")]
        public string? CurrentSongPath { get; set; }
        [Description("當前歌曲名稱")]
        public string? CurrentSongName { get; set; }

        [Description("當前歌曲 ID")]
        public int CurrentSongId { get; set; }

        [Description("當前以撥放歌曲時長 (秒)")]
        public int CurrentPlayedDuration { get; set; }

        public List<MusicModel> musicModels { get; set; } = new List<MusicModel>();
    }
    public class MusicModel
    {
        [Description("撥放清單ID")]
        public int PlayListId { get; set; }

        [Description("撥放清單")]
        public string PlayListName { get; set; }
        [Description("歌曲 ID")]
        public int SongId { get; set; }

        [Description("歌曲名稱")]
        public string? SongName { get; set; }

        [Description("演出者")]
        public string? ArtistName { get; set; }

        [Description("專輯封面圖片路徑")]
        public string? CoverPhotoPath { get; set; }

        [Description("音樂檔案路徑")]
        public string? SongPath { get; set; }



    }
}
