using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VocalSpace.Models;

public partial class VocalSpaceDbContext : DbContext
{
    public VocalSpaceDbContext(DbContextOptions<VocalSpaceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<ActivityComment> ActivityComments { get; set; }

    public virtual DbSet<Authority> Authorities { get; set; }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<Ecpay> Ecpays { get; set; }

    public virtual DbSet<Favoriteplaylist> Favoriteplaylists { get; set; }

    public virtual DbSet<Interested> Interesteds { get; set; }

    public virtual DbSet<LikeSong> LikeSongs { get; set; }

    public virtual DbSet<PlayList> PlayLists { get; set; }

    public virtual DbSet<PlayListSong> PlayListSongs { get; set; }

    public virtual DbSet<Selection> Selections { get; set; }

    public virtual DbSet<SelectionDetail> SelectionDetails { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<SongCategory> SongCategories { get; set; }

    public virtual DbSet<SongComment> SongComments { get; set; }

    public virtual DbSet<SongRank> SongRanks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserFollow> UserFollows { get; set; }

    public virtual DbSet<UserVoted> UserVoteds { get; set; }

    public virtual DbSet<UsersInfo> UsersInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Activity__45F4A7F154A71235");

            entity.ToTable("Activity");

            entity.HasIndex(e => e.ActivityId, "UQ__Activity__45F4A7F09510B12B").IsUnique();

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ActivityDescription).HasMaxLength(255);
            entity.Property(e => e.ApprovalStatus).HasDefaultValue((byte)0);
            entity.Property(e => e.City).HasMaxLength(5);
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EventCoverPath).HasMaxLength(255);
            entity.Property(e => e.EventTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Location).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.UploaderId).HasColumnName("UploaderID");

            entity.HasOne(d => d.Uploader).WithMany(p => p.Activities)
                .HasForeignKey(d => d.UploaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Activity__Upload__00200768");
        });

        modelBuilder.Entity<ActivityComment>(entity =>
        {
            entity.HasKey(e => e.ActivityCommentId).HasName("PK__Activity__CDDE91BDA0B73A22");

            entity.HasIndex(e => e.ActivityCommentId, "UQ__Activity__CDDE91BC7B0CD57B").IsUnique();

            entity.Property(e => e.ActivityCommentId).HasColumnName("ActivityCommentID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.Comment).HasMaxLength(255);
            entity.Property(e => e.CommentTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Activity).WithMany(p => p.ActivityComments)
                .HasForeignKey(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ActivityC__Activ__02084FDA");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ActivityC__UserI__07C12930");
        });

        modelBuilder.Entity<Authority>(entity =>
        {
            entity.HasKey(e => e.AuthorityId).HasName("PK__Authorit__433B1E6DDE6918FC");

            entity.ToTable("Authority");

            entity.HasIndex(e => e.AuthorityId, "UQ__Authorit__433B1E6C0F417D64").IsUnique();

            entity.Property(e => e.AuthorityId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AuthorityID");
            entity.Property(e => e.Name).HasMaxLength(20);
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.HasKey(e => e.DonationId).HasName("PK_Payment");

            entity.ToTable(tb => tb.HasComment("紀錄贊助"));

            entity.Property(e => e.DonationId)
                .ValueGeneratedOnAdd()
                .HasColumnName("DonationID");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.PaymentGateway)
                .HasMaxLength(50)
                .HasComment("串接的廠商  [ecpay]綠界");
            entity.Property(e => e.ReceiverId).HasColumnName("ReceiverID");
            entity.Property(e => e.SponsorId).HasColumnName("SponsorID");

            entity.HasOne(d => d.DonationNavigation).WithOne(p => p.DonationDonationNavigation)
                .HasForeignKey<Donation>(d => d.DonationId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Receiver).WithMany(p => p.DonationReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Ecpay>(entity =>
        {
            entity.ToTable("Ecpay", tb => tb.HasComment("紀錄贊助詳細資訊"));

            entity.Property(e => e.EcpayId).HasColumnName("EcpayID");
            entity.Property(e => e.DonationId)
                .HasComment("對應綠界MerchantMemberID")
                .HasColumnName("DonationID");
            entity.Property(e => e.MerchantId)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasComment("回傳的收款用戶註冊綠界時的ID(綠界辨識用)")
                .HasColumnName("MerchantID");
            entity.Property(e => e.MerchantTradeDate)
                .HasMaxLength(50)
                .HasComment("交易成立的時間");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .HasComment("支付方式的類型");
            entity.Property(e => e.PaymentTypeChargeFee)
                .HasMaxLength(50)
                .HasComment("支付手續費。 0表示沒有手續費");
            entity.Property(e => e.RtnCode).HasComment("回傳的交易狀態碼。0 未付款,1成功");
            entity.Property(e => e.RtnMsg)
                .HasMaxLength(50)
                .HasComment("回傳的交易狀態訊息");
            entity.Property(e => e.TradeAmt).HasComment("回傳的交易金額");
            entity.Property(e => e.TradeNo)
                .HasMaxLength(50)
                .HasComment("回傳的交易編號,收款者可查詢交易狀態");

            entity.HasOne(d => d.Donation).WithMany(p => p.Ecpays)
                .HasForeignKey(d => d.DonationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ecpay_Donations");
        });

        modelBuilder.Entity<Favoriteplaylist>(entity =>
        {
            entity.HasKey(e => new { e.PlayListId, e.UserId }).HasName("PK__Favorite__E908137153E205DB");

            entity.ToTable("Favoriteplaylist");

            entity.Property(e => e.PlayListId).HasColumnName("PlayListID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.PlayList).WithMany(p => p.Favoriteplaylists)
                .HasForeignKey(d => d.PlayListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favoritep__PlayL__7C4F7684");

            entity.HasOne(d => d.User).WithMany(p => p.Favoriteplaylists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Favoritep__UserI__74AE54BC");
        });

        modelBuilder.Entity<Interested>(entity =>
        {
            entity.HasKey(e => e.InterestedId).HasName("PK__Interest__4F84193ECCCE3ACD");

            entity.ToTable("Interested");

            entity.HasIndex(e => e.ActivityId, "UQ__Interest__45F4A7F09CF90B37").IsUnique();

            entity.Property(e => e.InterestedId).HasColumnName("InterestedID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.InterestedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Activity).WithOne(p => p.Interested)
                .HasForeignKey<Interested>(d => d.ActivityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Intereste__Activ__06CD04F7");

            entity.HasOne(d => d.User).WithMany(p => p.Interesteds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Intereste__UserI__01142BA1");
        });

        modelBuilder.Entity<LikeSong>(entity =>
        {
            entity.HasKey(e => new { e.LikeId, e.UserId, e.SongId }).HasName("PK__LikeSong__85F843E85F24E7A1");

            entity.Property(e => e.LikeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("LikeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.SongId).HasColumnName("SongID");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Song).WithMany(p => p.LikeSongs)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LikeSongs__SongI__7E37BEF6");

            entity.HasOne(d => d.User).WithMany(p => p.LikeSongs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LikeSongs__UserI__7F2BE32F");
        });

        modelBuilder.Entity<PlayList>(entity =>
        {
            entity.HasKey(e => e.PlayListId).HasName("PK__PlayList__38709FBB7FFA7661");

            entity.ToTable("PlayList");

            entity.HasIndex(e => e.PlayListId, "UQ__PlayList__38709FBA3A337F35").IsUnique();

            entity.Property(e => e.PlayListId).HasColumnName("PlayListID");
            entity.Property(e => e.CoverImagePath).HasMaxLength(255);
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsPublic).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.PlaylistDescription).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.PlayLists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayList__UserID__72C60C4A");
        });

        modelBuilder.Entity<PlayListSong>(entity =>
        {
            entity.HasKey(e => new { e.PlayListId, e.SongId }).HasName("PK__PlayList__595EA2D4E5686B5E");

            entity.HasIndex(e => e.SongId, "UQ__PlayList__12E3D6F68D7A392C").IsUnique();

            entity.Property(e => e.PlayListId).HasColumnName("PlayListID");
            entity.Property(e => e.SongId).HasColumnName("SongID");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.PlayList).WithMany(p => p.PlayListSongs)
                .HasForeignKey(d => d.PlayListId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayListS__PlayL__73BA3083");

            entity.HasOne(d => d.Song).WithOne(p => p.PlayListSong)
                .HasForeignKey<PlayListSong>(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PlayListS__SongI__75A278F5");
        });

        modelBuilder.Entity<Selection>(entity =>
        {
            entity.HasKey(e => e.SelectionId).HasName("PK__Selectio__7F17912F0F589520");

            entity.ToTable("Selection");

            entity.Property(e => e.SelectionId).HasColumnName("SelectionID");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.SelectionCoverPath).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(20);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Visible).HasDefaultValue(true);
        });

        modelBuilder.Entity<SelectionDetail>(entity =>
        {
            entity.HasKey(e => e.SelectionDetailId).HasName("PK__Selectio__BFA38A20B31A69DA");

            entity.ToTable("SelectionDetail");

            entity.Property(e => e.SelectionDetailId).HasColumnName("SelectionDetailID");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SelectionId).HasColumnName("SelectionID");
            entity.Property(e => e.SongId).HasColumnName("SongID");
            entity.Property(e => e.VoteCount).HasDefaultValue(0);

            entity.HasOne(d => d.Selection).WithMany(p => p.SelectionDetails)
                .HasForeignKey(d => d.SelectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Selection__Selec__05D8E0BE");

            entity.HasOne(d => d.Song).WithMany(p => p.SelectionDetails)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Selection__SongI__787EE5A0");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.SongId).HasName("PK__Songs__12E3D6F7719E04F4");

            entity.HasIndex(e => e.SongId, "UQ__Songs__12E3D6F67B409495").IsUnique();

            entity.Property(e => e.SongId).HasColumnName("SongID");
            entity.Property(e => e.CoverPhotoPath)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsRemove).HasDefaultValue(false);
            entity.Property(e => e.LikeCount).HasDefaultValue(0);
            entity.Property(e => e.Lyrics).HasDefaultValue("");
            entity.Property(e => e.PlayCount).HasDefaultValue(0);
            entity.Property(e => e.SongCategoryId).HasColumnName("SongCategoryID");
            entity.Property(e => e.SongDescription)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.SongName).HasMaxLength(20);
            entity.Property(e => e.SongPath).HasMaxLength(255);

            entity.HasOne(d => d.ArtistNavigation).WithMany(p => p.Songs)
                .HasForeignKey(d => d.Artist)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Songs__Artist__7D439ABD");

            entity.HasOne(d => d.SongCategory).WithMany(p => p.Songs)
                .HasForeignKey(d => d.SongCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Songs__SongCateg__797309D9");
        });

        modelBuilder.Entity<SongCategory>(entity =>
        {
            entity.HasKey(e => e.SongCategoryId).HasName("PK__SongCate__BD6CF5D99FDD1B86");

            entity.ToTable("SongCategory");

            entity.HasIndex(e => e.SongCategoryId, "UQ__SongCate__BD6CF5D880EFE2AD").IsUnique();

            entity.Property(e => e.SongCategoryId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SongCategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(2);
        });

        modelBuilder.Entity<SongComment>(entity =>
        {
            entity.HasKey(e => e.SongCommentId).HasName("PK__SongComm__CDDE91BDF671567C");

            entity.HasIndex(e => e.SongCommentId, "UQ__SongComm__CDDE91BCC8084B24").IsUnique();

            entity.Property(e => e.SongCommentId).HasColumnName("SongCommentID");
            entity.Property(e => e.Comment).HasMaxLength(255);
            entity.Property(e => e.CommentTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SongId).HasColumnName("SongID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Song).WithMany(p => p.SongComments)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SongComme__SongI__02FC7413");

            entity.HasOne(d => d.User).WithMany(p => p.SongComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SongComme__UserI__08B54D69");
        });

        modelBuilder.Entity<SongRank>(entity =>
        {
            entity.HasKey(e => e.SongId).HasName("PK__SongRank__12E3D6F75D69C5FC");

            entity.ToTable("SongRank");

            entity.HasIndex(e => e.SongId, "UQ__SongRank__12E3D6F6353DFB7C").IsUnique();

            entity.Property(e => e.SongId)
                .ValueGeneratedNever()
                .HasColumnName("SongID");

            entity.HasOne(d => d.Song).WithOne(p => p.SongRank)
                .HasForeignKey<SongRank>(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SongRank__SongID__7A672E12");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACF244AD17");

            entity.HasIndex(e => e.Account, "IX_Users").IsUnique();

            entity.HasIndex(e => e.UserId, "UQ__Users__1788CCAD1150EEC1").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Account).HasMaxLength(50);
            entity.Property(e => e.AuthorityId).HasColumnName("AuthorityID");
            entity.Property(e => e.CreateTime).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Status).HasDefaultValue(false);
            entity.Property(e => e.TempPassword)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasDefaultValue("");

            entity.HasOne(d => d.Authority).WithMany(p => p.Users)
                .HasForeignKey(d => d.AuthorityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__Authority__71D1E811");
        });

        modelBuilder.Entity<UserFollow>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.FollowedUserId }).HasName("PK__UserFoll__597EE9E91933E0E3");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.FollowedUserId).HasColumnName("FollowedUserID");
            entity.Property(e => e.FollowTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.FollowedUser).WithMany(p => p.UserFollowFollowedUsers)
                .HasForeignKey(d => d.FollowedUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserFollo__Follo__778AC167");

            entity.HasOne(d => d.User).WithMany(p => p.UserFollowUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserFollo__UserI__76969D2E");
        });

        modelBuilder.Entity<UserVoted>(entity =>
        {
            entity.HasKey(e => new { e.SelectionDetailId, e.UserId }).HasName("PK__UserVote__6EDB06EA57146ADA");

            entity.ToTable("UserVoted");

            entity.Property(e => e.SelectionDetailId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SelectionDetailID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VoteTime).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.SelectionDetail).WithMany(p => p.UserVoteds)
                .HasForeignKey(d => d.SelectionDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserVoted__Selec__03F0984C");

            entity.HasOne(d => d.User).WithMany(p => p.UserVoteds)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserVoted__UserI__04E4BC85");
        });

        modelBuilder.Entity<UsersInfo>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UsersInf__1788CCAC81CC84F7");

            entity.ToTable("UsersInfo");

            entity.HasIndex(e => e.Email, "IX_UsersInfo").IsUnique();

            entity.HasIndex(e => e.UserId, "UQ__UsersInf__1788CCAD339DF13E").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.AvatarPath)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.BannerImagePath)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Birthday).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.Email).HasMaxLength(254);
            entity.Property(e => e.PersonalIntroduction).HasDefaultValue("");

            entity.HasOne(d => d.User).WithOne(p => p.UsersInfo)
                .HasForeignKey<UsersInfo>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsersInfo_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
