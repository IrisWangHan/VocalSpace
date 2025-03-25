using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class UserFollow
{
    public long UserId { get; set; }

    public long FollowedUserId { get; set; }

    public DateTime FollowTime { get; set; }

    public virtual User FollowedUser { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
