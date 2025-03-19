namespace VocalSpace.Models.ViewModel.Song
{
    public class SongViewModel
    {
        public string UserName { get; set; } = null!;
        //from model.Song
        public string SongPath { get; set; } = null!;

        public long SongId { get; set; }

        public long SongArtist { get; set; }

        public string? SongCoverPhotoPath { get; set; }

        public int? LikeCount { get; set; }

        public byte SongStatus { get; set; }

        public bool? IsRemove { get; set; }

        public byte SongCategoryId { get; set; }

        public string SongName { get; set; } = null!;

        //from model.SongRank
        public byte PreRank { get; set; }
        public byte CurrentRank { get; set; }

        //from model.LikeSongs
        public long LikeId { get; set; }
        public long UserId { get; set; }

        //from model.PlayListSongs
        public long PlayListId { get; set; }

        //from model.PlayList
        public string PlayListName { get; set; } = null!;
        public string? PlayListCoverImagePath { get; set; }


    }
}
