using System;
using System.Collections.Generic;

namespace VocalSpace.newModels;

public partial class Selection
{
    public long SelectionId { get; set; }

    public string Title { get; set; } = null!;

    public string SelectionCoverPath { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime VotingStartDate { get; set; }

    public DateTime VotingEndDate { get; set; }

    public bool Visible { get; set; }

    public string Description { get; set; } = null!;

    public long UserId { get; set; }

    public virtual ICollection<SelectionDetail> SelectionDetails { get; set; } = new List<SelectionDetail>();
}
