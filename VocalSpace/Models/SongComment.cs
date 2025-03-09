using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class SongComment
{
    public int SongCommentId { get; set; }

    public long SongId { get; set; }

    public long UserId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime? CommentTime { get; set; }

    public virtual Song Song { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
