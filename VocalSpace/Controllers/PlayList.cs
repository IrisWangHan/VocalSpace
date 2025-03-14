using System;
using System.Collections.Generic;

namespace VocalSpace.newModels;

public partial class PlayList
{
    public long PlayListId { get; set; }

    public long UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? CoverImagePath { get; set; }

    public DateTime CreateTime { get; set; }

    public string? PlaylistDescription { get; set; }

    public bool IsPublic { get; set; }

    public virtual ICollection<Favoriteplaylist> Favoriteplaylists { get; set; } = new List<Favoriteplaylist>();

    public virtual ICollection<PlayListSong> PlayListSongs { get; set; } = new List<PlayListSong>();

    public virtual User User { get; set; } = null!;
}
