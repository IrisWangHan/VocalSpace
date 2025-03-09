using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class Song
{
    public string SongPath { get; set; } = null!;

    public long SongId { get; set; }

    public long Artist { get; set; }

    public string? CoverPhotoPath { get; set; }

    public int? LikeCount { get; set; }

    public DateTime? CreateTime { get; set; }

    public string? SongDescription { get; set; }

    public string? Lyrics { get; set; }

    public byte SongStatus { get; set; }

    public bool? IsRemove { get; set; }

    public byte SongCategoryId { get; set; }

    public string SongName { get; set; } = null!;

    public int? PlayCount { get; set; }

    public virtual User ArtistNavigation { get; set; } = null!;

    public virtual ICollection<LikeSong> LikeSongs { get; set; } = new List<LikeSong>();

    public virtual PlayListSong? PlayListSong { get; set; }

    public virtual ICollection<SelectionDetail> SelectionDetails { get; set; } = new List<SelectionDetail>();

    public virtual SongCategory SongCategory { get; set; } = null!;

    public virtual ICollection<SongComment> SongComments { get; set; } = new List<SongComment>();

    public virtual SongRank? SongRank { get; set; }
}
