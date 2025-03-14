using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class Interested
{
    public long InterestedId { get; set; }

    public long UserId { get; set; }

    public long ActivityId { get; set; }

    public DateTime InterestedDate { get; set; }

    public virtual Activity Activity { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
