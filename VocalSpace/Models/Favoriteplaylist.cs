using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class Favoriteplaylist
{
    public long PlayListId { get; set; }

    public long UserId { get; set; }

    public DateTime? CreateTime { get; set; }

    public virtual PlayList PlayList { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
