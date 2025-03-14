using System;
using System.Collections.Generic;

namespace VocalSpace.newModels;

public partial class Activity
{
    public long ActivityId { get; set; }

    public long UploaderId { get; set; }

    public string? EventCoverPath { get; set; }

    public DateTime EventTime { get; set; }

    public string Title { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string City { get; set; } = null!;

    public byte ApprovalStatus { get; set; }

    public DateTime CreateTime { get; set; }

    public string ActivityDescription { get; set; } = null!;

    public virtual ICollection<ActivityComment> ActivityComments { get; set; } = new List<ActivityComment>();

    public virtual ICollection<Interested> Interesteds { get; set; } = new List<Interested>();

    public virtual User Uploader { get; set; } = null!;
}
