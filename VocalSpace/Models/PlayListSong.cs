using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class PlayListSong
{
    public long PlayListId { get; set; }

    public long SongId { get; set; }

    public DateTime CreateTime { get; set; }

    public virtual PlayList PlayList { get; set; } = null!;

    public virtual Song Song { get; set; } = null!;
}
