using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class Song
{
    public long SongId { get; set; }

    public string SongName { get; set; } = null!;

    public long Artist { get; set; }

    public string SongPath { get; set; } = null!;

    public string CoverPhotoPath { get; set; } = null!;

    public byte SongCategoryId { get; set; }

    public int PlayCount { get; set; }

    public int LikeCount { get; set; }

    public string? SongDescription { get; set; }

    public string? Lyrics { get; set; }

    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 0審核中 1通過 2失敗
    /// </summary>
    public byte SongStatus { get; set; }

    /// <summary>
    /// 0未刪除 1已刪除
    /// </summary>
    public bool IsRemove { get; set; }

    public virtual User ArtistNavigation { get; set; } = null!;

    public virtual ICollection<LikeSong> LikeSongs { get; set; } = new List<LikeSong>();

    public virtual ICollection<PlayHistory> PlayHistories { get; set; } = new List<PlayHistory>();

    public virtual ICollection<PlayListSong> PlayListSongs { get; set; } = new List<PlayListSong>();

    public virtual ICollection<SelectionDetail> SelectionDetails { get; set; } = new List<SelectionDetail>();

    public virtual SongCategory SongCategory { get; set; } = null!;

    public virtual ICollection<SongComment> SongComments { get; set; } = new List<SongComment>();

    public virtual ICollection<SongRank> SongRanks { get; set; } = new List<SongRank>();
}
