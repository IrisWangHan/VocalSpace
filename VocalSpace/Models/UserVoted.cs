using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class UserVoted
{
    public long SelectionDetailId { get; set; }

    public long UserId { get; set; }

    public DateTime VoteTime { get; set; }

    public virtual SelectionDetail SelectionDetail { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
