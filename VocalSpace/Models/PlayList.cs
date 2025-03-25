using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VocalSpace.Models;

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
    [NotMapped] // 避免 EF 嘗試將這個欄位存入資料庫
    public virtual User? User { get; set; }
}
