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
        public int? SongCount { get; set; }

        //from model.SongRank
        public byte PreRank { get; set; }
        public byte CurrentRank { get; set; }

        //from model.LikeSongs
        public long LikeId { get; set; }
        public long UserId { get; set; }
        public int LikeSongCount { get; set; }

        //from model.PlayListSongs
        public long PlayListId { get; set; }

        public int PlayListSongCount { get; set; }

        //from model.PlayList
        public string PlayListName { get; set; } = null!;
        public string? PlayListCoverImagePath { get; set; }
        public DateTime PlayListCreateTime { get; set; }
        public string? PlaylistDescription { get; set; }
        public bool IsPublic { get; set; }
        // 歌曲資訊
        public List<SongDetail> Songs { get; set; } = new List<SongDetail>();

        public int PlayCount { get; set; }
    }

        public class SongDetail
        {
            public long SongId { get; set; }
            public string SongName { get; set; } = null!;
            public long ArtistId { get; set; }
            public string SongArtist { get; set; } = null!;
            public string? SongCoverPhotoPath { get; set; }
        }

}

