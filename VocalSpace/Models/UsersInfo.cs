using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class UsersInfo
{
    public long UserId { get; set; }

    public string? BannerImagePath { get; set; }

    public string? AvatarPath { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? PersonalIntroduction { get; set; }

    public string Email { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
