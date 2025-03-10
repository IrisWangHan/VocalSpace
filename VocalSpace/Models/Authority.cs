using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class Authority
{
    public byte AuthorityId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
