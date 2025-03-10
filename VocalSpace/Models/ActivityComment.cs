using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class ActivityComment
{
    public int ActivityCommentId { get; set; }

    public long ActivityId { get; set; }

    public long UserId { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime? CommentTime { get; set; }

    public virtual Activity Activity { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
