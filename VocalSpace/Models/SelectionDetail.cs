using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class SelectionDetail
{
    public long SelectionDetailId { get; set; }

    public long SelectionId { get; set; }

    public long SongId { get; set; }

    public int? VoteCount { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual Selection Selection { get; set; } = null!;

    public virtual Song Song { get; set; } = null!;

    public virtual ICollection<UserVoted> UserVoteds { get; set; } = new List<UserVoted>();
}
