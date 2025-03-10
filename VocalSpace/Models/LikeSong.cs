using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class LikeSong
{
    public long LikeId { get; set; }

    public long UserId { get; set; }

    public long SongId { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual Song Song { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
