using System;
using System.Collections.Generic;

namespace VocalSpace.newModels;

public partial class SongCategory
{
    public byte SongCategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
