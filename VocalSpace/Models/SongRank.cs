using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class SongRank
{
    public long SongId { get; set; }

    public byte PreRank { get; set; }

    public byte CurrentRank { get; set; }

    public virtual Song Song { get; set; } = null!;
}
