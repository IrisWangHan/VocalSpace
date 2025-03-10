using System;
using System.Collections.Generic;

namespace VocalSpace.Models;

public partial class User
{
    public long UserId { get; set; }

    public string? UserName { get; set; }

    public byte AuthorityId { get; set; }

    public string Account { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? TempPassword { get; set; }

    public DateTime? CreateTime { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public virtual ICollection<ActivityComment> ActivityComments { get; set; } = new List<ActivityComment>();

    public virtual Authority Authority { get; set; } = null!;

    public virtual Donation? DonationDonationNavigation { get; set; }

    public virtual ICollection<Donation> DonationReceivers { get; set; } = new List<Donation>();

    public virtual ICollection<Favoriteplaylist> Favoriteplaylists { get; set; } = new List<Favoriteplaylist>();

    public virtual ICollection<Interested> Interesteds { get; set; } = new List<Interested>();

    public virtual ICollection<LikeSong> LikeSongs { get; set; } = new List<LikeSong>();

    public virtual ICollection<PlayList> PlayLists { get; set; } = new List<PlayList>();

    public virtual ICollection<SongComment> SongComments { get; set; } = new List<SongComment>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

    public virtual ICollection<UserFollow> UserFollowFollowedUsers { get; set; } = new List<UserFollow>();

    public virtual ICollection<UserFollow> UserFollowUsers { get; set; } = new List<UserFollow>();

    public virtual ICollection<UserVoted> UserVoteds { get; set; } = new List<UserVoted>();

    public virtual UsersInfo? UsersInfo { get; set; }
}
