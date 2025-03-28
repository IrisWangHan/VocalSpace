/*    ==指令碼參數==

    來源伺服器版本 : SQL Server 2022 (16.0.1000)
    來源資料庫引擎版本 : Microsoft SQL Server Express Edition
    來源資料庫引擎類型 : 獨立 SQL Server

    目標伺服器版本 : SQL Server 2022
    目標資料庫引擎版本 : Microsoft SQL Server Express Edition
    目標資料庫引擎類型 : 獨立 SQL Server
*/
USE [master]
GO
/****** Object:  Database [VocalSpaceDB]    Script Date: 2025/3/23 上午 02:12:50 ******/
CREATE DATABASE [VocalSpaceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'VocalSpaceDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\VocalSpaceDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'VocalSpaceDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\VocalSpaceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [VocalSpaceDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VocalSpaceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VocalSpaceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [VocalSpaceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [VocalSpaceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [VocalSpaceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [VocalSpaceDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [VocalSpaceDB] SET  MULTI_USER 
GO
ALTER DATABASE [VocalSpaceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [VocalSpaceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [VocalSpaceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [VocalSpaceDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [VocalSpaceDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [VocalSpaceDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [VocalSpaceDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [VocalSpaceDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [VocalSpaceDB]
GO
/****** Object:  Table [dbo].[Activity]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activity](
	[ActivityID] [bigint] IDENTITY(1,1) NOT NULL,
	[UploaderID] [bigint] NOT NULL,
	[EventCoverPath] [nvarchar](255) NOT NULL,
	[EventTime] [datetime2](7) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[City] [nvarchar](5) NOT NULL,
	[ApprovalStatus] [tinyint] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[ActivityDescription] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK__Activity__45F4A7F197DB201F] PRIMARY KEY CLUSTERED 
(
	[ActivityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityComments]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityComments](
	[ActivityCommentID] [int] IDENTITY(1,1) NOT NULL,
	[ActivityID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Comment] [nvarchar](255) NOT NULL,
	[CommentTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__Activity__F569732DA198E90B] PRIMARY KEY CLUSTERED 
(
	[ActivityCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Authority]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authority](
	[AuthorityID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AuthorityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Donations]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donations](
	[DonationID] [bigint] IDENTITY(1,1) NOT NULL,
	[SponsorID] [bigint] NOT NULL,
	[ReceiverID] [bigint] NOT NULL,
	[PaymentGateway] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[DonationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ecpay]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ecpay](
	[EcpayID] [bigint] IDENTITY(1,1) NOT NULL,
	[DonationID] [bigint] NOT NULL,
	[MerchantID] [nvarchar](50) NOT NULL,
	[TradeNo] [nvarchar](50) NOT NULL,
	[RtnCode] [int] NOT NULL,
	[RtnMsg] [nvarchar](50) NULL,
	[TradeAmt] [int] NULL,
	[PaymentType] [nvarchar](50) NULL,
	[PaymentTypeChargeFee] [nvarchar](50) NULL,
	[MerchantTradeDate] [nvarchar](50) NULL,
 CONSTRAINT [PK_Ecpay] PRIMARY KEY CLUSTERED 
(
	[EcpayID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Favoriteplaylist]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Favoriteplaylist](
	[PlayListID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__Favorite__E9081371355A6DAE] PRIMARY KEY CLUSTERED 
(
	[PlayListID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Interested]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Interested](
	[InterestedID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[ActivityID] [bigint] NOT NULL,
	[InterestedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Interested] PRIMARY KEY CLUSTERED 
(
	[InterestedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LikeSongs]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LikeSongs](
	[LikeID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[SongID] [bigint] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_LikeSongs] PRIMARY KEY CLUSTERED 
(
	[LikeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayHistory]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayHistory](
	[HistoryID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[SongID] [bigint] NOT NULL,
	[PlayTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__PlayHist__4D7B4ADD679AA6EF] PRIMARY KEY CLUSTERED 
(
	[HistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayList]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayList](
	[PlayListID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[CoverImagePath] [nvarchar](255) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[PlaylistDescription] [nvarchar](255) NULL,
	[IsPublic] [bit] NOT NULL,
 CONSTRAINT [PK__PlayList__38709FBB8F33B645] PRIMARY KEY CLUSTERED 
(
	[PlayListID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayListSongs]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayListSongs](
	[PlayListID] [bigint] NOT NULL,
	[SongID] [bigint] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__PlayList__595EA2D4E5686B5E] PRIMARY KEY CLUSTERED 
(
	[PlayListID] ASC,
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Selection]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Selection](
	[SelectionID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](20) NOT NULL,
	[SelectionCoverPath] [nvarchar](255) NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[VotingStartDate] [datetime2](7) NOT NULL,
	[VotingEndDate] [datetime2](7) NOT NULL,
	[Visible] [bit] NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[UserID] [bigint] NOT NULL,
 CONSTRAINT [PK__Selectio__7F17912F0F589520] PRIMARY KEY CLUSTERED 
(
	[SelectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SelectionDetail]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SelectionDetail](
	[SelectionDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[SelectionID] [bigint] NOT NULL,
	[SongID] [bigint] NOT NULL,
	[VoteCount] [int] NOT NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[ReviewStatus] [tinyint] NOT NULL,
 CONSTRAINT [PK__Selectio__BFA38A20B31A69DA] PRIMARY KEY CLUSTERED 
(
	[SelectionDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SongCategory]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SongCategory](
	[SongCategoryID] [tinyint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SongCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SongComments]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SongComments](
	[SongCommentID] [int] IDENTITY(1,1) NOT NULL,
	[SongID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Comment] [nvarchar](255) NOT NULL,
	[CommentTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__SongComm__CDDE91BDF671567C] PRIMARY KEY CLUSTERED 
(
	[SongCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SongRank]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SongRank](
	[SongID] [bigint] NOT NULL,
	[PreRank] [tinyint] NULL,
	[CurrentRank] [tinyint] NOT NULL,
	[RankPeriod] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__SongRank__12E3D6F75D69C5FC] PRIMARY KEY CLUSTERED 
(
	[SongID] ASC,
	[RankPeriod] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Songs]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Songs](
	[SongID] [bigint] IDENTITY(1,1) NOT NULL,
	[SongName] [nvarchar](20) NOT NULL,
	[Artist] [bigint] NOT NULL,
	[SongPath] [nvarchar](255) NOT NULL,
	[CoverPhotoPath] [nvarchar](255) NOT NULL,
	[SongCategoryID] [tinyint] NOT NULL,
	[PlayCount] [int] NOT NULL,
	[LikeCount] [int] NOT NULL,
	[SongDescription] [nvarchar](255) NULL,
	[Lyrics] [nvarchar](max) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[SongStatus] [tinyint] NOT NULL,
	[IsRemove] [bit] NOT NULL,
 CONSTRAINT [PK__Songs__12E3D6F7A5969905] PRIMARY KEY CLUSTERED 
(
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserFollows]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFollows](
	[UserID] [bigint] NOT NULL,
	[FollowedUserID] [bigint] NOT NULL,
	[FollowTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__UserFoll__597EE9E9CFBAEE1F] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[FollowedUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[AuthorityID] [tinyint] NOT NULL,
	[Account] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[TempPassword] [nvarchar](255) NULL,
	[CreateTime] [datetime2](7) NOT NULL,
	[Status] [tinyint] NULL,
 CONSTRAINT [PK__Users__1788CCACF244AD17] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersInfo]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersInfo](
	[UserID] [bigint] NOT NULL,
	[BannerImagePath] [nvarchar](255) NULL,
	[AvatarPath] [nvarchar](255) NOT NULL,
	[Birthday] [date] NOT NULL,
	[PersonalIntroduction] [nvarchar](max) NULL,
	[Email] [nvarchar](254) NOT NULL,
 CONSTRAINT [PK__UsersInf__1788CCAC81CC84F7] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserVoted]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserVoted](
	[SelectionDetailID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[VoteTime] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_UserVoted] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[SelectionDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Activity] ON 

INSERT [dbo].[Activity] ([ActivityID], [UploaderID], [EventCoverPath], [EventTime], [Title], [Location], [City], [ApprovalStatus], [CreateTime], [ActivityDescription]) VALUES (1, 2, N'/image/Activity/cs.jpg', CAST(N'2025-06-01T00:00:00.0000000' AS DateTime2), N'Alice 的生日演唱會', N'台北市民廣場', N'台北市', 0, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2), N'Alice將舉辦演唱會，一起用音樂點燃熱情！')
INSERT [dbo].[Activity] ([ActivityID], [UploaderID], [EventCoverPath], [EventTime], [Title], [Location], [City], [ApprovalStatus], [CreateTime], [ActivityDescription]) VALUES (2, 3, N'/image/Activity/cs.jpg', CAST(N'2025-05-10T00:00:00.0000000' AS DateTime2), N'Bob爵士', N'勤美綠園道', N'台中市', 0, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2), N'擅長薩克斯風的Bob，將會展現前所未見的獨特風格！')
INSERT [dbo].[Activity] ([ActivityID], [UploaderID], [EventCoverPath], [EventTime], [Title], [Location], [City], [ApprovalStatus], [CreateTime], [ActivityDescription]) VALUES (3, 4, N'/image/Activity/cs.jpg', CAST(N'2025-05-01T00:00:00.0000000' AS DateTime2), N'Chainsmoker電音狂歡節', N'台北小巨蛋', N'台北市', 0, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2), N'雙人組合 The Chainsmokers，帶來一場震撼音樂盛會必定讓您嗨翻全場！')
SET IDENTITY_INSERT [dbo].[Activity] OFF
GO
SET IDENTITY_INSERT [dbo].[ActivityComments] ON 

INSERT [dbo].[ActivityComments] ([ActivityCommentID], [ActivityID], [UserID], [Comment], [CommentTime]) VALUES (1, 1, 3, N'Alice 生日快樂！我一定到場支援!', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[ActivityComments] ([ActivityCommentID], [ActivityID], [UserID], [Comment], [CommentTime]) VALUES (2, 1, 2, N'謝謝各位<3。', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[ActivityComments] ([ActivityCommentID], [ActivityID], [UserID], [Comment], [CommentTime]) VALUES (3, 2, 4, N'爵士大好!!!!。', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[ActivityComments] ([ActivityCommentID], [ActivityID], [UserID], [Comment], [CommentTime]) VALUES (4, 2, 3, N'大好~!!!!!', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[ActivityComments] ([ActivityCommentID], [ActivityID], [UserID], [Comment], [CommentTime]) VALUES (5, 3, 1, N'這一次竟然在小巨蛋，好擠的感覺。', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[ActivityComments] ([ActivityCommentID], [ActivityID], [UserID], [Comment], [CommentTime]) VALUES (6, 3, 4, N'忠實粉絲簽到!', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[ActivityComments] ([ActivityCommentID], [ActivityID], [UserID], [Comment], [CommentTime]) VALUES (7, 3, 3, N'現場一定超嗨！', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
SET IDENTITY_INSERT [dbo].[ActivityComments] OFF
GO
SET IDENTITY_INSERT [dbo].[Authority] ON 

INSERT [dbo].[Authority] ([AuthorityID], [Name]) VALUES (3, N'admin')
INSERT [dbo].[Authority] ([AuthorityID], [Name]) VALUES (2, N'manager')
INSERT [dbo].[Authority] ([AuthorityID], [Name]) VALUES (1, N'user')
SET IDENTITY_INSERT [dbo].[Authority] OFF
GO
INSERT [dbo].[Favoriteplaylist] ([PlayListID], [UserID], [CreateTime]) VALUES (1, 2, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[Favoriteplaylist] ([PlayListID], [UserID], [CreateTime]) VALUES (1, 3, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[Favoriteplaylist] ([PlayListID], [UserID], [CreateTime]) VALUES (2, 2, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[Favoriteplaylist] ([PlayListID], [UserID], [CreateTime]) VALUES (2, 3, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[Favoriteplaylist] ([PlayListID], [UserID], [CreateTime]) VALUES (4, 4, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Interested] ON 

INSERT [dbo].[Interested] ([InterestedID], [UserID], [ActivityID], [InterestedDate]) VALUES (85, 2, 1, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[Interested] ([InterestedID], [UserID], [ActivityID], [InterestedDate]) VALUES (86, 3, 1, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[Interested] ([InterestedID], [UserID], [ActivityID], [InterestedDate]) VALUES (87, 3, 2, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[Interested] ([InterestedID], [UserID], [ActivityID], [InterestedDate]) VALUES (88, 2, 2, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[Interested] ([InterestedID], [UserID], [ActivityID], [InterestedDate]) VALUES (89, 4, 3, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[Interested] ([InterestedID], [UserID], [ActivityID], [InterestedDate]) VALUES (90, 2, 3, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[Interested] ([InterestedID], [UserID], [ActivityID], [InterestedDate]) VALUES (91, 3, 3, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Interested] OFF
GO
SET IDENTITY_INSERT [dbo].[LikeSongs] ON 

INSERT [dbo].[LikeSongs] ([LikeID], [UserID], [SongID], [CreateTime]) VALUES (2, 3, 1, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[LikeSongs] ([LikeID], [UserID], [SongID], [CreateTime]) VALUES (3, 3, 2, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[LikeSongs] ([LikeID], [UserID], [SongID], [CreateTime]) VALUES (4, 4, 2, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[LikeSongs] ([LikeID], [UserID], [SongID], [CreateTime]) VALUES (8, 2, 4, CAST(N'2025-03-22T03:00:39.2166667' AS DateTime2))
INSERT [dbo].[LikeSongs] ([LikeID], [UserID], [SongID], [CreateTime]) VALUES (9, 2, 3, CAST(N'2025-03-22T03:00:39.8400000' AS DateTime2))
INSERT [dbo].[LikeSongs] ([LikeID], [UserID], [SongID], [CreateTime]) VALUES (10, 2, 1, CAST(N'2025-03-22T03:00:40.3400000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[LikeSongs] OFF
GO
SET IDENTITY_INSERT [dbo].[PlayHistory] ON 

INSERT [dbo].[PlayHistory] ([HistoryID], [UserID], [SongID], [PlayTime]) VALUES (1, 2, 1, CAST(N'2025-03-10T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[PlayHistory] ([HistoryID], [UserID], [SongID], [PlayTime]) VALUES (2, 2, 2, CAST(N'2025-03-11T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[PlayHistory] ([HistoryID], [UserID], [SongID], [PlayTime]) VALUES (3, 3, 2, CAST(N'2025-03-12T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[PlayHistory] ([HistoryID], [UserID], [SongID], [PlayTime]) VALUES (4, 4, 1, CAST(N'2025-03-13T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[PlayHistory] ([HistoryID], [UserID], [SongID], [PlayTime]) VALUES (5, 2, 1, CAST(N'2025-03-14T00:00:00.0000000' AS DateTime2))
SET IDENTITY_INSERT [dbo].[PlayHistory] OFF
GO
SET IDENTITY_INSERT [dbo].[PlayList] ON 

INSERT [dbo].[PlayList] ([PlayListID], [UserID], [Name], [CoverImagePath], [CreateTime], [PlaylistDescription], [IsPublic]) VALUES (1, 2, N'Alice的歌單', NULL, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2), NULL, 1)
INSERT [dbo].[PlayList] ([PlayListID], [UserID], [Name], [CoverImagePath], [CreateTime], [PlaylistDescription], [IsPublic]) VALUES (2, 2, N'Alice的歌單2', NULL, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2), NULL, 1)
INSERT [dbo].[PlayList] ([PlayListID], [UserID], [Name], [CoverImagePath], [CreateTime], [PlaylistDescription], [IsPublic]) VALUES (3, 3, N'bob的歌曲精選', NULL, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2), NULL, 1)
INSERT [dbo].[PlayList] ([PlayListID], [UserID], [Name], [CoverImagePath], [CreateTime], [PlaylistDescription], [IsPublic]) VALUES (4, 4, N'查理歌單', NULL, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2), NULL, 1)
SET IDENTITY_INSERT [dbo].[PlayList] OFF
GO
INSERT [dbo].[PlayListSongs] ([PlayListID], [SongID], [CreateTime]) VALUES (1, 1, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[PlayListSongs] ([PlayListID], [SongID], [CreateTime]) VALUES (1, 2, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[PlayListSongs] ([PlayListID], [SongID], [CreateTime]) VALUES (3, 1, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[PlayListSongs] ([PlayListID], [SongID], [CreateTime]) VALUES (4, 1, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Selection] ON 

INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (1, N'2025 夏日音樂大賽', N'/image/selection/cover1.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), 1, N'夏日來臨，一起用音樂點燃熱情！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (2, N'新聲代音樂徵選', N'/image/selection/cover2.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-04-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-01T00:00:00.0000000' AS DateTime2), 1, N'發掘新生代音樂人才，讓你的聲音被世界聽見！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (3, N'原創音樂大賞', N'/image/selection/cover3.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-05-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-20T00:00:00.0000000' AS DateTime2), 1, N'原創音樂盛典，展現你的創作才華！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (4, N'校園音樂徵選賽', N'/image/selection/cover4.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-03-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-05T00:00:00.0000000' AS DateTime2), 1, N'專為學生打造的音樂徵選比賽，勇敢追夢！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (5, N'電音製作人大賽', N'/image/selection/cover5.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-07-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-20T00:00:00.0000000' AS DateTime2), 1, N'專為電子音樂製作人而生，展現你的DJ與混音技巧！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (6, N'爵士新星徵選', N'/image/selection/cover6.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-06-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-30T00:00:00.0000000' AS DateTime2), 1, N'尋找未來的爵士新星，展現你的獨特風格！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (7, N'嘻哈饒舌大戰', N'/image/selection/cover7.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-08-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-20T00:00:00.0000000' AS DateTime2), 1, N'你有炸裂的Flow嗎？來這裡挑戰最強饒舌歌手！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (8, N'搖滾樂團徵選', N'/image/selection/cover8.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-09-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-20T00:00:00.0000000' AS DateTime2), 1, N'搖滾魂不滅，帶著你的樂團來征服舞台！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (9, N'民謠創作比賽', N'/image/selection/cover9.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-05-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-25T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-10T00:00:00.0000000' AS DateTime2), 1, N'用民謠講故事，讓你的音樂感動人心！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (10, N'流行金曲挑戰', N'/image/selection/cover10.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-10-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-20T00:00:00.0000000' AS DateTime2), 1, N'挑戰流行金曲，看看誰能成為下一位音樂明星！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (11, N'全球音樂新秀賽', N'/image/selection/cover1.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-04-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-25T00:00:00.0000000' AS DateTime2), 1, N'來自世界各地的音樂新星，誰能脫穎而出？', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (12, N'嘻哈對決', N'/image/selection/cover2.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-07-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-30T00:00:00.0000000' AS DateTime2), 1, N'誰才是最具實力的地下饒舌王？', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (13, N'經典翻唱大賽', N'/image/selection/cover3.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-05-25T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-25T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), 1, N'翻唱經典歌曲，展現你獨特的音樂風格！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (14, N'重金屬樂團決戰', N'/image/selection/cover4.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-20T00:00:00.0000000' AS DateTime2), 1, N'重金屬狂熱，燃爆全場！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (15, N'偶像歌手選拔', N'/image/selection/cover5.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-09-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-25T00:00:00.0000000' AS DateTime2), 1, N'展現你的舞台魅力，成為下一位音樂偶像！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (16, N'聲林之王選秀', N'/image/selection/cover6.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-03-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-30T00:00:00.0000000' AS DateTime2), 1, N'發掘最具潛力的音樂之星！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (17, N'電音Remix大賽', N'/image/selection/cover7.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-08-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-30T00:00:00.0000000' AS DateTime2), 1, N'改編熱門曲目，秀出你的混音才華！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (18, N'世界音樂之夜', N'/image/selection/cover8.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-05T00:00:00.0000000' AS DateTime2), 1, N'融合不同文化的音樂風格，一場聽覺盛宴！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (19, N'饒舌王之爭', N'/image/selection/cover9.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-06-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-05T00:00:00.0000000' AS DateTime2), 1, N'快嘴對決，誰是最強饒舌高手？', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (20, N'亞洲偶像歌手賽', N'/image/selection/cover10.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-10-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-25T00:00:00.0000000' AS DateTime2), 1, N'挑戰亞洲流行音樂，邁向偶像之路！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (21, N'樂隊之巔', N'/image/selection/cover1.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-05-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-30T00:00:00.0000000' AS DateTime2), 1, N'全國最強樂隊競技，一較高下！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (22, N'R&B靈魂歌手選拔', N'/image/selection/cover2.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-09-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-05T00:00:00.0000000' AS DateTime2), 1, N'展現你的靈魂樂嗓音，打動評審！', 1)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (23, N'電影配樂創作大賽', N'/image/selection/cover3.jpg', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), CAST(N'2025-08-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-20T00:00:00.0000000' AS DateTime2), 1, N'創作最動人的電影配樂，讓旋律訴說故事！', 1)
SET IDENTITY_INSERT [dbo].[Selection] OFF
GO
SET IDENTITY_INSERT [dbo].[SelectionDetail] ON 

INSERT [dbo].[SelectionDetail] ([SelectionDetailID], [SelectionID], [SongID], [VoteCount], [CreateTime], [ReviewStatus]) VALUES (1, 1, 1, 0, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), 1)
INSERT [dbo].[SelectionDetail] ([SelectionDetailID], [SelectionID], [SongID], [VoteCount], [CreateTime], [ReviewStatus]) VALUES (2, 1, 2, 0, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), 1)
INSERT [dbo].[SelectionDetail] ([SelectionDetailID], [SelectionID], [SongID], [VoteCount], [CreateTime], [ReviewStatus]) VALUES (3, 2, 3, 0, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), 1)
INSERT [dbo].[SelectionDetail] ([SelectionDetailID], [SelectionID], [SongID], [VoteCount], [CreateTime], [ReviewStatus]) VALUES (4, 2, 4, 0, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[SelectionDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[SongCategory] ON 

INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (2, N'民謠')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (6, N'探索')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (4, N'都會')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (1, N'搖滾')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (5, N'電子')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (3, N'嘻哈')
SET IDENTITY_INSERT [dbo].[SongCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[SongComments] ON 

INSERT [dbo].[SongComments] ([SongCommentID], [SongID], [UserID], [Comment], [CommentTime]) VALUES (1, 3, 3, N'這首歌真的有夠炸！', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[SongComments] ([SongCommentID], [SongID], [UserID], [Comment], [CommentTime]) VALUES (2, 4, 3, N'副歌太洗腦了，已經單曲循環一整天了。', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[SongComments] ([SongCommentID], [SongID], [UserID], [Comment], [CommentTime]) VALUES (3, 1, 2, N'太甜了吧。', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[SongComments] ([SongCommentID], [SongID], [UserID], [Comment], [CommentTime]) VALUES (4, 2, 3, N'鼓點節奏感超讚，bassline 好沉穩。', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[SongComments] ([SongCommentID], [SongID], [UserID], [Comment], [CommentTime]) VALUES (5, 4, 2, N'電音節奏太炸，現場一定超嗨！', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[SongComments] ([SongCommentID], [SongID], [UserID], [Comment], [CommentTime]) VALUES (6, 4, 3, N'這首歌真的好有故事感，歌詞太打動人心了。', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[SongComments] ([SongCommentID], [SongID], [UserID], [Comment], [CommentTime]) VALUES (7, 4, 4, N'電音 Drop 的部分是不是超炸。', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[SongComments] ([SongCommentID], [SongID], [UserID], [Comment], [CommentTime]) VALUES (8, 1, 2, N'Melody太好聽了，感覺可以上榜！', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[SongComments] ([SongCommentID], [SongID], [UserID], [Comment], [CommentTime]) VALUES (9, 3, 4, N'Bob 這首爵士真的是神作，推薦給所有樂迷。', CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
SET IDENTITY_INSERT [dbo].[SongComments] OFF
GO
INSERT [dbo].[SongRank] ([SongID], [PreRank], [CurrentRank], [RankPeriod]) VALUES (1, NULL, 1, CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[SongRank] ([SongID], [PreRank], [CurrentRank], [RankPeriod]) VALUES (1, 1, 2, CAST(N'2025-02-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[SongRank] ([SongID], [PreRank], [CurrentRank], [RankPeriod]) VALUES (2, NULL, 2, CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[SongRank] ([SongID], [PreRank], [CurrentRank], [RankPeriod]) VALUES (2, 2, 1, CAST(N'2025-02-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[SongRank] ([SongID], [PreRank], [CurrentRank], [RankPeriod]) VALUES (3, NULL, 3, CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[SongRank] ([SongID], [PreRank], [CurrentRank], [RankPeriod]) VALUES (3, 3, 3, CAST(N'2025-02-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[SongRank] ([SongID], [PreRank], [CurrentRank], [RankPeriod]) VALUES (4, NULL, 4, CAST(N'2025-01-01T00:00:00.0000000' AS DateTime2))
INSERT [dbo].[SongRank] ([SongID], [PreRank], [CurrentRank], [RankPeriod]) VALUES (4, 4, 4, CAST(N'2025-02-01T00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Songs] ON 

INSERT [dbo].[Songs] ([SongID], [SongName], [Artist], [SongPath], [CoverPhotoPath], [SongCategoryID], [PlayCount], [LikeCount], [SongDescription], [Lyrics], [CreateTime], [SongStatus], [IsRemove]) VALUES (1, N'給艾莉絲', 3, N'/audio/sample.mp3', N'/image/Song/default.jpg', 4, 2521, 33, N'', N'', CAST(N'2018-11-20T00:00:00.0000000' AS DateTime2), 1, 0)
INSERT [dbo].[Songs] ([SongID], [SongName], [Artist], [SongPath], [CoverPhotoPath], [SongCategoryID], [PlayCount], [LikeCount], [SongDescription], [Lyrics], [CreateTime], [SongStatus], [IsRemove]) VALUES (2, N'查理沒有茶包', 4, N'/audio/sample.mp3', N'/image/Song/default.jpg', 3, 1203, 14, N'', N'', CAST(N'2019-03-17T00:00:00.0000000' AS DateTime2), 1, 0)
INSERT [dbo].[Songs] ([SongID], [SongName], [Artist], [SongPath], [CoverPhotoPath], [SongCategoryID], [PlayCount], [LikeCount], [SongDescription], [Lyrics], [CreateTime], [SongStatus], [IsRemove]) VALUES (3, N'不要BB', 3, N'/audio/sample.mp3', N'/image/Song/default.jpg', 4, 251, 31, N'', N'', CAST(N'2020-07-20T00:00:00.0000000' AS DateTime2), 1, 0)
INSERT [dbo].[Songs] ([SongID], [SongName], [Artist], [SongPath], [CoverPhotoPath], [SongCategoryID], [PlayCount], [LikeCount], [SongDescription], [Lyrics], [CreateTime], [SongStatus], [IsRemove]) VALUES (4, N'經過路面查看左右後方', 4, N'/audio/sample.mp3', N'/image/Song/default.jpg', 3, 120, 12, N'', N'', CAST(N'2021-04-17T00:00:00.0000000' AS DateTime2), 1, 0)
SET IDENTITY_INSERT [dbo].[Songs] OFF
GO
INSERT [dbo].[UserFollows] ([UserID], [FollowedUserID], [FollowTime]) VALUES (2, 3, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[UserFollows] ([UserID], [FollowedUserID], [FollowTime]) VALUES (2, 4, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[UserFollows] ([UserID], [FollowedUserID], [FollowTime]) VALUES (3, 2, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
INSERT [dbo].[UserFollows] ([UserID], [FollowedUserID], [FollowTime]) VALUES (3, 4, CAST(N'2025-03-22T02:25:59.7733333' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [UserName], [AuthorityID], [Account], [Password], [TempPassword], [CreateTime], [Status]) VALUES (1, N'AdminUser', 1, N'admin', N'$2a$11$wUQgyXgfrZtWQ3jkimjU0Ot2B1k/zeziq9B7h0nDynCJxqsXhomZW', NULL, CAST(N'2021-03-07T00:00:00.0000000' AS DateTime2), 1)
INSERT [dbo].[Users] ([UserID], [UserName], [AuthorityID], [Account], [Password], [TempPassword], [CreateTime], [Status]) VALUES (2, N'Alice艾莉絲', 2, N'alice123', N'$2a$11$.lsQm1gjcgkWT1yp9F0QdeOyZyXD09C6bHdlEdvuPiqVVoKA71XDi', NULL, CAST(N'2025-01-03T00:00:00.0000000' AS DateTime2), 1)
INSERT [dbo].[Users] ([UserID], [UserName], [AuthorityID], [Account], [Password], [TempPassword], [CreateTime], [Status]) VALUES (3, N'我是Bob', 2, N'bob456', N'$2a$11$T8XxJwkzs2NUE3Rv6wEdE..HJz794FvYjWkHKLsDT8yFdRqnWSfwC', NULL, CAST(N'2025-01-31T00:00:00.0000000' AS DateTime2), 1)
INSERT [dbo].[Users] ([UserID], [UserName], [AuthorityID], [Account], [Password], [TempPassword], [CreateTime], [Status]) VALUES (4, N'查理解不能', 2, N'charlie789', N'$2a$11$iPwWL9mFORJrm8b8nhybkedqENDSqg8u4rN1WyMCz2eAMwFgp3AVa', NULL, CAST(N'2025-02-04T00:00:00.0000000' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
INSERT [dbo].[UsersInfo] ([UserID], [BannerImagePath], [AvatarPath], [Birthday], [PersonalIntroduction], [Email]) VALUES (1, N'', N'/image/Avatar/default.png', CAST(N'1990-01-01' AS Date), N'系統管理者', N'admin@example.com')
INSERT [dbo].[UsersInfo] ([UserID], [BannerImagePath], [AvatarPath], [Birthday], [PersonalIntroduction], [Email]) VALUES (2, N'', N'/image/Avatar/default.png', CAST(N'1995-05-12' AS Date), N'喜愛音樂', N'alice@example.com')
INSERT [dbo].[UsersInfo] ([UserID], [BannerImagePath], [AvatarPath], [Birthday], [PersonalIntroduction], [Email]) VALUES (3, N'', N'/image/Avatar/default.png', CAST(N'1993-08-23' AS Date), N'吉他愛好者', N'bob@example.com')
INSERT [dbo].[UsersInfo] ([UserID], [BannerImagePath], [AvatarPath], [Birthday], [PersonalIntroduction], [Email]) VALUES (4, N'', N'/image/Avatar/default.png', CAST(N'1998-03-15' AS Date), N'唱歌是我的熱情', N'charlie@example.com')
GO
INSERT [dbo].[UserVoted] ([SelectionDetailID], [UserID], [VoteTime]) VALUES (1, 2, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[UserVoted] ([SelectionDetailID], [UserID], [VoteTime]) VALUES (3, 2, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[UserVoted] ([SelectionDetailID], [UserID], [VoteTime]) VALUES (2, 3, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[UserVoted] ([SelectionDetailID], [UserID], [VoteTime]) VALUES (3, 3, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[UserVoted] ([SelectionDetailID], [UserID], [VoteTime]) VALUES (2, 4, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
INSERT [dbo].[UserVoted] ([SelectionDetailID], [UserID], [VoteTime]) VALUES (4, 4, CAST(N'2025-03-22T02:25:59.7766667' AS DateTime2))
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Authority]    Script Date: 2025/3/23 上午 02:12:50 ******/
ALTER TABLE [dbo].[Authority] ADD  CONSTRAINT [UQ_Authority] UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_Interested]    Script Date: 2025/3/23 上午 02:12:50 ******/
ALTER TABLE [dbo].[Interested] ADD  CONSTRAINT [UQ_Interested] UNIQUE NONCLUSTERED 
(
	[ActivityID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_LikeSongs]    Script Date: 2025/3/23 上午 02:12:50 ******/
ALTER TABLE [dbo].[LikeSongs] ADD  CONSTRAINT [UQ_LikeSongs] UNIQUE NONCLUSTERED 
(
	[SongID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_PlayList]    Script Date: 2025/3/23 上午 02:12:50 ******/
ALTER TABLE [dbo].[PlayList] ADD  CONSTRAINT [UQ_PlayList] UNIQUE NONCLUSTERED 
(
	[Name] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Selection]    Script Date: 2025/3/23 上午 02:12:50 ******/
ALTER TABLE [dbo].[Selection] ADD  CONSTRAINT [UQ_Selection] UNIQUE NONCLUSTERED 
(
	[Title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_SelectionDetail]    Script Date: 2025/3/23 上午 02:12:50 ******/
ALTER TABLE [dbo].[SelectionDetail] ADD  CONSTRAINT [UQ_SelectionDetail] UNIQUE NONCLUSTERED 
(
	[SelectionID] ASC,
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_SongCategory]    Script Date: 2025/3/23 上午 02:12:50 ******/
CREATE NONCLUSTERED INDEX [UQ_SongCategory] ON [dbo].[SongCategory]
(
	[CategoryName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ_SongRank]    Script Date: 2025/3/23 上午 02:12:50 ******/
CREATE NONCLUSTERED INDEX [UQ_SongRank] ON [dbo].[SongRank]
(
	[RankPeriod] ASC,
	[CurrentRank] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Users]    Script Date: 2025/3/23 上午 02:12:50 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ_Users] UNIQUE NONCLUSTERED 
(
	[Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_UsersInfo]    Script Date: 2025/3/23 上午 02:12:50 ******/
ALTER TABLE [dbo].[UsersInfo] ADD  CONSTRAINT [UQ_UsersInfo] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Activity] ADD  CONSTRAINT [DF__Activity__EventT__693CA210]  DEFAULT (getdate()) FOR [EventTime]
GO
ALTER TABLE [dbo].[Activity] ADD  CONSTRAINT [DF__Activity__Approv__6A30C649]  DEFAULT ((0)) FOR [ApprovalStatus]
GO
ALTER TABLE [dbo].[Activity] ADD  CONSTRAINT [DF_Activity_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[ActivityComments] ADD  CONSTRAINT [DF_ActivityComments_CommentTime]  DEFAULT (getdate()) FOR [CommentTime]
GO
ALTER TABLE [dbo].[Donations] ADD  CONSTRAINT [DF_Payment_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Ecpay] ADD  CONSTRAINT [DF_Ecpay_TradeNo]  DEFAULT ('') FOR [MerchantID]
GO
ALTER TABLE [dbo].[Favoriteplaylist] ADD  CONSTRAINT [DF__Favoritep__Creat__6EF57B66]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Interested] ADD  CONSTRAINT [DF__Intereste__Inter__68487DD7]  DEFAULT (getdate()) FOR [InterestedDate]
GO
ALTER TABLE [dbo].[LikeSongs] ADD  CONSTRAINT [DF__LikeSongs__Creat__5441852A]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[PlayHistory] ADD  CONSTRAINT [DF_PlayHistory_PlayTime]  DEFAULT (getdate()) FOR [PlayTime]
GO
ALTER TABLE [dbo].[PlayList] ADD  CONSTRAINT [DF__PlayList__Create__71D1E811]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[PlayList] ADD  CONSTRAINT [DF__PlayList__IsPubl__72C60C4A]  DEFAULT ((1)) FOR [IsPublic]
GO
ALTER TABLE [dbo].[PlayListSongs] ADD  CONSTRAINT [DF__PlayListS__Creat__5165187F]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Selection] ADD  CONSTRAINT [DF_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Selection] ADD  CONSTRAINT [DF__Selection__Visib__4316F928]  DEFAULT ((1)) FOR [Visible]
GO
ALTER TABLE [dbo].[SelectionDetail] ADD  CONSTRAINT [DF_VoteCount]  DEFAULT ((0)) FOR [VoteCount]
GO
ALTER TABLE [dbo].[SelectionDetail] ADD  CONSTRAINT [DF__Selection__Creat__45F365D3]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[SelectionDetail] ADD  CONSTRAINT [DF_SelectionDetail_ReviewStatus]  DEFAULT ((0)) FOR [ReviewStatus]
GO
ALTER TABLE [dbo].[SongComments] ADD  CONSTRAINT [DF_CommentTime]  DEFAULT (getdate()) FOR [CommentTime]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_CoverPhotoPath]  DEFAULT ('/image/Song/default.jpg') FOR [CoverPhotoPath]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_PlayCount]  DEFAULT ((0)) FOR [PlayCount]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF__Songs__LikeCount__7A672E12]  DEFAULT ((0)) FOR [LikeCount]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_SongDescription]  DEFAULT ('') FOR [SongDescription]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_Lyrics]  DEFAULT ('') FOR [Lyrics]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF__Songs__CreateTim__7B5B524B]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_Songs_SongStatus]  DEFAULT ((0)) FOR [SongStatus]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_IsRemove]  DEFAULT ((0)) FOR [IsRemove]
GO
ALTER TABLE [dbo].[UserFollows] ADD  CONSTRAINT [DF__UserFollo__Follo__00200768]  DEFAULT (getdate()) FOR [FollowTime]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_UserName]  DEFAULT ('') FOR [UserName]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_TempPassword]  DEFAULT ('') FOR [TempPassword]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__Status__3B75D760]  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[UsersInfo] ADD  CONSTRAINT [DF_UsersInfo_BannerImagePath]  DEFAULT ('') FOR [BannerImagePath]
GO
ALTER TABLE [dbo].[UsersInfo] ADD  CONSTRAINT [DF_AvatarPath]  DEFAULT ('/image/Avatar/default.png') FOR [AvatarPath]
GO
ALTER TABLE [dbo].[UserVoted] ADD  CONSTRAINT [DF__UserVoted__VoteT__6B24EA82]  DEFAULT (getdate()) FOR [VoteTime]
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK__Activity__Upload__00200768] FOREIGN KEY([UploaderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK__Activity__Upload__00200768]
GO
ALTER TABLE [dbo].[ActivityComments]  WITH CHECK ADD  CONSTRAINT [FK__ActivityC__Activ__0A9D95DB] FOREIGN KEY([ActivityID])
REFERENCES [dbo].[Activity] ([ActivityID])
GO
ALTER TABLE [dbo].[ActivityComments] CHECK CONSTRAINT [FK__ActivityC__Activ__0A9D95DB]
GO
ALTER TABLE [dbo].[ActivityComments]  WITH CHECK ADD  CONSTRAINT [FK__ActivityC__UserI__07C12930] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[ActivityComments] CHECK CONSTRAINT [FK__ActivityC__UserI__07C12930]
GO
ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Users_DonationID] FOREIGN KEY([DonationID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Users_DonationID]
GO
ALTER TABLE [dbo].[Donations]  WITH CHECK ADD  CONSTRAINT [FK_Donations_Users_ReceiverID] FOREIGN KEY([ReceiverID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Donations] CHECK CONSTRAINT [FK_Donations_Users_ReceiverID]
GO
ALTER TABLE [dbo].[Ecpay]  WITH CHECK ADD  CONSTRAINT [FK_Ecpay_Donations] FOREIGN KEY([DonationID])
REFERENCES [dbo].[Donations] ([DonationID])
GO
ALTER TABLE [dbo].[Ecpay] CHECK CONSTRAINT [FK_Ecpay_Donations]
GO
ALTER TABLE [dbo].[Favoriteplaylist]  WITH CHECK ADD  CONSTRAINT [FK__Favoritep__PlayL__0F624AF8] FOREIGN KEY([PlayListID])
REFERENCES [dbo].[PlayList] ([PlayListID])
GO
ALTER TABLE [dbo].[Favoriteplaylist] CHECK CONSTRAINT [FK__Favoritep__PlayL__0F624AF8]
GO
ALTER TABLE [dbo].[Favoriteplaylist]  WITH CHECK ADD  CONSTRAINT [FK__Favoritep__UserI__74AE54BC] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Favoriteplaylist] CHECK CONSTRAINT [FK__Favoritep__UserI__74AE54BC]
GO
ALTER TABLE [dbo].[Interested]  WITH CHECK ADD  CONSTRAINT [FK__Intereste__Activ__06CD04F7] FOREIGN KEY([ActivityID])
REFERENCES [dbo].[Activity] ([ActivityID])
GO
ALTER TABLE [dbo].[Interested] CHECK CONSTRAINT [FK__Intereste__Activ__06CD04F7]
GO
ALTER TABLE [dbo].[Interested]  WITH CHECK ADD  CONSTRAINT [FK__Intereste__UserI__01142BA1] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Interested] CHECK CONSTRAINT [FK__Intereste__UserI__01142BA1]
GO
ALTER TABLE [dbo].[LikeSongs]  WITH CHECK ADD  CONSTRAINT [FK__LikeSongs__SongI__7E37BEF6] FOREIGN KEY([SongID])
REFERENCES [dbo].[Songs] ([SongID])
GO
ALTER TABLE [dbo].[LikeSongs] CHECK CONSTRAINT [FK__LikeSongs__SongI__7E37BEF6]
GO
ALTER TABLE [dbo].[LikeSongs]  WITH CHECK ADD  CONSTRAINT [FK__LikeSongs__UserI__7F2BE32F] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[LikeSongs] CHECK CONSTRAINT [FK__LikeSongs__UserI__7F2BE32F]
GO
ALTER TABLE [dbo].[PlayHistory]  WITH CHECK ADD  CONSTRAINT [FK_PlayHistory_Songs] FOREIGN KEY([SongID])
REFERENCES [dbo].[Songs] ([SongID])
GO
ALTER TABLE [dbo].[PlayHistory] CHECK CONSTRAINT [FK_PlayHistory_Songs]
GO
ALTER TABLE [dbo].[PlayHistory]  WITH CHECK ADD  CONSTRAINT [FK_PlayHistory_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[PlayHistory] CHECK CONSTRAINT [FK_PlayHistory_Users]
GO
ALTER TABLE [dbo].[PlayList]  WITH CHECK ADD  CONSTRAINT [FK__PlayList__UserID__72C60C4A] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[PlayList] CHECK CONSTRAINT [FK__PlayList__UserID__72C60C4A]
GO
ALTER TABLE [dbo].[PlayListSongs]  WITH CHECK ADD  CONSTRAINT [FK__PlayListS__PlayL__73BA3083] FOREIGN KEY([PlayListID])
REFERENCES [dbo].[PlayList] ([PlayListID])
GO
ALTER TABLE [dbo].[PlayListSongs] CHECK CONSTRAINT [FK__PlayListS__PlayL__73BA3083]
GO
ALTER TABLE [dbo].[PlayListSongs]  WITH CHECK ADD  CONSTRAINT [FK__PlayListS__SongI__75A278F5] FOREIGN KEY([SongID])
REFERENCES [dbo].[Songs] ([SongID])
GO
ALTER TABLE [dbo].[PlayListSongs] CHECK CONSTRAINT [FK__PlayListS__SongI__75A278F5]
GO
ALTER TABLE [dbo].[SelectionDetail]  WITH CHECK ADD  CONSTRAINT [FK__Selection__Selec__05D8E0BE] FOREIGN KEY([SelectionID])
REFERENCES [dbo].[Selection] ([SelectionID])
GO
ALTER TABLE [dbo].[SelectionDetail] CHECK CONSTRAINT [FK__Selection__Selec__05D8E0BE]
GO
ALTER TABLE [dbo].[SelectionDetail]  WITH CHECK ADD  CONSTRAINT [FK__Selection__SongI__787EE5A0] FOREIGN KEY([SongID])
REFERENCES [dbo].[Songs] ([SongID])
GO
ALTER TABLE [dbo].[SelectionDetail] CHECK CONSTRAINT [FK__Selection__SongI__787EE5A0]
GO
ALTER TABLE [dbo].[SongComments]  WITH CHECK ADD  CONSTRAINT [FK__SongComme__SongI__02FC7413] FOREIGN KEY([SongID])
REFERENCES [dbo].[Songs] ([SongID])
GO
ALTER TABLE [dbo].[SongComments] CHECK CONSTRAINT [FK__SongComme__SongI__02FC7413]
GO
ALTER TABLE [dbo].[SongComments]  WITH CHECK ADD  CONSTRAINT [FK__SongComme__UserI__08B54D69] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[SongComments] CHECK CONSTRAINT [FK__SongComme__UserI__08B54D69]
GO
ALTER TABLE [dbo].[SongRank]  WITH CHECK ADD  CONSTRAINT [FK__SongRank__SongID__7A672E12] FOREIGN KEY([SongID])
REFERENCES [dbo].[Songs] ([SongID])
GO
ALTER TABLE [dbo].[SongRank] CHECK CONSTRAINT [FK__SongRank__SongID__7A672E12]
GO
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD  CONSTRAINT [FK__Songs__Artist__7D439ABD] FOREIGN KEY([Artist])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Songs] CHECK CONSTRAINT [FK__Songs__Artist__7D439ABD]
GO
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD  CONSTRAINT [FK__Songs__SongCateg__1DB06A4F] FOREIGN KEY([SongCategoryID])
REFERENCES [dbo].[SongCategory] ([SongCategoryID])
GO
ALTER TABLE [dbo].[Songs] CHECK CONSTRAINT [FK__Songs__SongCateg__1DB06A4F]
GO
ALTER TABLE [dbo].[UserFollows]  WITH CHECK ADD  CONSTRAINT [FK__UserFollo__Follo__778AC167] FOREIGN KEY([FollowedUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UserFollows] CHECK CONSTRAINT [FK__UserFollo__Follo__778AC167]
GO
ALTER TABLE [dbo].[UserFollows]  WITH CHECK ADD  CONSTRAINT [FK__UserFollo__UserI__76969D2E] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UserFollows] CHECK CONSTRAINT [FK__UserFollo__UserI__76969D2E]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK__Users__Authority__71D1E811] FOREIGN KEY([AuthorityID])
REFERENCES [dbo].[Authority] ([AuthorityID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK__Users__Authority__71D1E811]
GO
ALTER TABLE [dbo].[UsersInfo]  WITH CHECK ADD  CONSTRAINT [FK_UsersInfo_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UsersInfo] CHECK CONSTRAINT [FK_UsersInfo_Users]
GO
ALTER TABLE [dbo].[UserVoted]  WITH CHECK ADD  CONSTRAINT [FK__UserVoted__Selec__03F0984C] FOREIGN KEY([SelectionDetailID])
REFERENCES [dbo].[SelectionDetail] ([SelectionDetailID])
GO
ALTER TABLE [dbo].[UserVoted] CHECK CONSTRAINT [FK__UserVoted__Selec__03F0984C]
GO
ALTER TABLE [dbo].[UserVoted]  WITH CHECK ADD  CONSTRAINT [FK__UserVoted__UserI__04E4BC85] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[UserVoted] CHECK CONSTRAINT [FK__UserVoted__UserI__04E4BC85]
GO
ALTER TABLE [dbo].[UserFollows]  WITH CHECK ADD  CONSTRAINT [CHK_SelfFollow] CHECK  (([UserID]<>[FollowedUserID]))
GO
ALTER TABLE [dbo].[UserFollows] CHECK CONSTRAINT [CHK_SelfFollow]
GO
/****** Object:  StoredProcedure [dbo].[AddAuthority]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  CREATE PROCEDURE [dbo].[AddAuthority]
  AS
  BEGIN
    SET NOCOUNT ON;

    -- 刪除原本資料表中的所有資料
	DELETE FROM Authority;
	DBCC CHECKIDENT ('Authority', RESEED, 0); -- 設定 0 表示下一筆資料的 ID 會從 

   INSERT INTO [Authority]([Name])VALUES
   ('user'),
   ('manager'),
   ('admin')
  END
GO
/****** Object:  StoredProcedure [dbo].[AddSelection]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddSelection]
  AS
  BEGIN
      SET NOCOUNT ON;

    -- 刪除原本資料表中的所有資料
	DELETE FROM [Selection];
	DBCC CHECKIDENT ('Selection', RESEED, 0); -- 設定 0 表示下一筆資料的 ID 會從 1 開始
	
	DECLARE @UserID BIGINT;
	SET @UserID =(SELECT [UserID] FROM Users WHERE Account = 'admin')
	-----------------------------------------------
	/*

	IF NOT EXISTS (SELECT 1 FROM Users WHERE Account = 'AAA')
	BEGIN
			INSERT INTO Users(AuthorityID,Account,Password) VALUES(2,'AAA','AAA123')

			-- 取得剛插入的 UserID

			SET @UserID = SCOPE_IDENTITY();

			-- 檢查是否成功取得 UserID
			--SELECT @UserID AS UserID;

			IF @UserID IS NOT NULL
			BEGIN
				INSERT INTO UsersInfo(UserID, PersonalIntroduction, Email)
				VALUES(@UserID, 'HELLO WORLD!', 'AAA123@gmail.com');
			END
	END
	ELSE
	BEGIN
		SET @UserID =(SELECT [UserID] FROM Users WHERE Account = 'AAA')
	END

	*/
	--------------------------------------------------------------

	INSERT INTO [Selection] (
		   [UserID]
		  ,[Title]
		  ,[SelectionCoverPath]
		  ,[StartDate]
		  ,[EndDate]
		  ,[VotingStartDate]
		  ,[VotingEndDate]
		  ,[Description])
	VALUES
	(@UserID,'2025 夏日音樂大賽', '/image/selection/cover1.jpg', '2025-06-01', '2025-06-30', '2025-07-01', '2025-07-15', '夏日來臨，一起用音樂點燃熱情！'),
	(@UserID,'新聲代音樂徵選', '/image/selection/cover2.jpg', '2025-04-10', '2025-05-10', '2025-05-15', '2025-06-01', '發掘新生代音樂人才，讓你的聲音被世界聽見！'),
	(@UserID,'原創音樂大賞', '/image/selection/cover3.jpg', '2025-05-01', '2025-05-31', '2025-06-05', '2025-06-20', '原創音樂盛典，展現你的創作才華！'),
	(@UserID,'校園音樂徵選賽', '/image/selection/cover4.jpg', '2025-03-15', '2025-04-15', '2025-04-20', '2025-05-05', '專為學生打造的音樂徵選比賽，勇敢追夢！'),
	(@UserID,'電音製作人大賽', '/image/selection/cover5.jpg', '2025-07-01', '2025-07-31', '2025-08-05', '2025-08-20', '專為電子音樂製作人而生，展現你的DJ與混音技巧！'),
	(@UserID,'爵士新星徵選', '/image/selection/cover6.jpg', '2025-06-10', '2025-07-10', '2025-07-15', '2025-07-30', '尋找未來的爵士新星，展現你的獨特風格！'),
	(@UserID,'嘻哈饒舌大戰', '/image/selection/cover7.jpg', '2025-08-01', '2025-08-30', '2025-09-05', '2025-09-20', '你有炸裂的Flow嗎？來這裡挑戰最強饒舌歌手！'),
	(@UserID,'搖滾樂團徵選', '/image/selection/cover8.jpg', '2025-09-01', '2025-09-30', '2025-10-05', '2025-10-20', '搖滾魂不滅，帶著你的樂團來征服舞台！'),
	(@UserID,'民謠創作比賽', '/image/selection/cover9.jpg', '2025-05-20', '2025-06-20', '2025-06-25', '2025-07-10', '用民謠講故事，讓你的音樂感動人心！'),
	(@UserID,'流行金曲挑戰', '/image/selection/cover10.jpg', '2025-10-01', '2025-10-31', '2025-11-05', '2025-11-20', '挑戰流行金曲，看看誰能成為下一位音樂明星！'),
	(@UserID,'全球音樂新秀賽', '/image/selection/cover1.jpg', '2025-04-05', '2025-05-05', '2025-05-10', '2025-05-25', '來自世界各地的音樂新星，誰能脫穎而出？'),
	(@UserID,'嘻哈對決', '/image/selection/cover2.jpg', '2025-07-10', '2025-08-10', '2025-08-15', '2025-08-30', '誰才是最具實力的地下饒舌王？'),
	(@UserID,'經典翻唱大賽', '/image/selection/cover3.jpg', '2025-05-25', '2025-06-25', '2025-07-01', '2025-07-15', '翻唱經典歌曲，展現你獨特的音樂風格！'),
	(@UserID,'重金屬樂團決戰', '/image/selection/cover4.jpg', '2025-06-01', '2025-06-30', '2025-07-05', '2025-07-20', '重金屬狂熱，燃爆全場！'),
	(@UserID,'偶像歌手選拔', '/image/selection/cover5.jpg', '2025-09-05', '2025-09-30', '2025-10-05', '2025-10-25', '展現你的舞台魅力，成為下一位音樂偶像！'),
	(@UserID,'聲林之王選秀', '/image/selection/cover6.jpg', '2025-03-10', '2025-04-10', '2025-04-15', '2025-04-30', '發掘最具潛力的音樂之星！'),
	(@UserID,'電音Remix大賽', '/image/selection/cover7.jpg', '2025-08-10', '2025-09-10', '2025-09-15', '2025-09-30', '改編熱門曲目，秀出你的混音才華！'),
	(@UserID,'世界音樂之夜', '/image/selection/cover8.jpg', '2025-07-15', '2025-08-15', '2025-08-20', '2025-09-05', '融合不同文化的音樂風格，一場聽覺盛宴！'),
	(@UserID,'饒舌王之爭', '/image/selection/cover9.jpg', '2025-06-15', '2025-07-15', '2025-07-20', '2025-08-05', '快嘴對決，誰是最強饒舌高手？'),
	(@UserID,'亞洲偶像歌手賽', '/image/selection/cover10.jpg', '2025-10-05', '2025-10-31', '2025-11-05', '2025-11-25', '挑戰亞洲流行音樂，邁向偶像之路！'),
	(@UserID,'樂隊之巔', '/image/selection/cover1.jpg', '2025-05-10', '2025-06-10', '2025-06-15', '2025-06-30', '全國最強樂隊競技，一較高下！'),
	(@UserID,'R&B靈魂歌手選拔', '/image/selection/cover2.jpg', '2025-09-15', '2025-10-15', '2025-10-20', '2025-11-05', '展現你的靈魂樂嗓音，打動評審！'),
	(@UserID,'電影配樂創作大賽', '/image/selection/cover3.jpg', '2025-08-01', '2025-08-31', '2025-09-05', '2025-09-20', '創作最動人的電影配樂，讓旋律訴說故事！');
  END;
GO
/****** Object:  StoredProcedure [dbo].[AddSongCategory]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddSongCategory]  
AS  
BEGIN  
    SET NOCOUNT ON;  

    -- 只在資料不存在時插入
    INSERT INTO [VocalSpaceDB].[dbo].[SongCategory] ([CategoryName])  
    SELECT N'搖滾' WHERE NOT EXISTS (SELECT 1 FROM [VocalSpaceDB].[dbo].[SongCategory] WHERE [CategoryName] = N'搖滾')
    UNION ALL
    SELECT N'民謠' WHERE NOT EXISTS (SELECT 1 FROM [VocalSpaceDB].[dbo].[SongCategory] WHERE [CategoryName] = N'民謠')
    UNION ALL
    SELECT N'嘻哈' WHERE NOT EXISTS (SELECT 1 FROM [VocalSpaceDB].[dbo].[SongCategory] WHERE [CategoryName] = N'嘻哈')
    UNION ALL
    SELECT N'都會' WHERE NOT EXISTS (SELECT 1 FROM [VocalSpaceDB].[dbo].[SongCategory] WHERE [CategoryName] = N'都會')
    UNION ALL
    SELECT N'電子' WHERE NOT EXISTS (SELECT 1 FROM [VocalSpaceDB].[dbo].[SongCategory] WHERE [CategoryName] = N'電子')
    UNION ALL
    SELECT N'探索' WHERE NOT EXISTS (SELECT 1 FROM [VocalSpaceDB].[dbo].[SongCategory] WHERE [CategoryName] = N'探索');
END  
GO
/****** Object:  StoredProcedure [dbo].[InsertDefaultData]    Script Date: 2025/3/23 上午 02:12:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsertDefaultData]
AS
BEGIN
    SET NOCOUNT ON;

	-- 刪除並 Reseed


	DELETE FROM UserVoted

	DELETE FROM SelectionDetail
	DBCC CHECKIDENT ('SelectionDetail', RESEED, 0);

	DELETE FROM Selection;
	DBCC CHECKIDENT ('Selection', RESEED, 0); -- 設定 0 表示下一筆資料的 ID 會從 1 開始

	DELETE FROM ActivityComments
	DBCC CHECKIDENT ('ActivityComments', RESEED, 0);

	DELETE FROM SongComments
	DBCC CHECKIDENT ('SongComments', RESEED, 0);

	DELETE FROM Interested;

	DELETE FROM Activity;
    DBCC CHECKIDENT ('Activity', RESEED, 0);

	DELETE FROM PlayHistory;
    DBCC CHECKIDENT ('PlayHistory', RESEED, 0);

	DELETE FROM Favoriteplaylist;

	DELETE FROM PlayListSongs;

	DELETE FROM PlayList;
    DBCC CHECKIDENT ('PlayList', RESEED, 0);

	DELETE FROM SongRank;

	DELETE FROM LikeSongs;
    DBCC CHECKIDENT ('LikeSongs', RESEED, 0);

    DELETE FROM Songs;
    DBCC CHECKIDENT ('Songs', RESEED, 0);

	DELETE FROM UserFollows;

	DELETE FROM UsersInfo;

    DELETE FROM Users;
    DBCC CHECKIDENT ('Users', RESEED, 0);

    --插入 Users
    INSERT INTO Users (UserName, AuthorityID, Account, Password, TempPassword, CreateTime, Status) 
    VALUES 
    ('AdminUser', 1, 'admin', '$2a$11$wUQgyXgfrZtWQ3jkimjU0Ot2B1k/zeziq9B7h0nDynCJxqsXhomZW', NULL, '2021-03-07', 1),
    ('Alice艾莉絲', 2, 'alice123', '$2a$11$.lsQm1gjcgkWT1yp9F0QdeOyZyXD09C6bHdlEdvuPiqVVoKA71XDi', NULL, '2025-01-03', 1),
    ('我是Bob', 2, 'bob456', '$2a$11$T8XxJwkzs2NUE3Rv6wEdE..HJz794FvYjWkHKLsDT8yFdRqnWSfwC', NULL, '2025-01-31', 1),
    ('查理解不能', 2, 'charlie789', '$2a$11$iPwWL9mFORJrm8b8nhybkedqENDSqg8u4rN1WyMCz2eAMwFgp3AVa', NULL, '2025-02-04', 1);

    --插入 UsersInfo
    INSERT INTO UsersInfo (UserID, Birthday, PersonalIntroduction, Email) 
    VALUES 
    (1, '1990-01-01', '系統管理者', 'admin@example.com'),		--admin
    (2, '1995-05-12', '喜愛音樂', 'alice@example.com'),			--alice
    (3, '1993-08-23', '吉他愛好者', 'bob@example.com'),			--bob
    (4, '1998-03-15', '唱歌是我的熱情', 'charlie@example.com'); --charlie

    --插入 UserFollows
    INSERT INTO UserFollows (UserID, FollowedUserID, FollowTime) 
    VALUES 
    (2, 3, GETDATE()),  -- Alice 追蹤 Bob
    (3, 4, GETDATE()),  -- Bob 追蹤 Charlie
	(3, 2, GETDATE()),  -- Bob 追蹤 Alice
    (2, 4, GETDATE());  -- Alice 追蹤 Charlie

    --插入 Songs
    INSERT INTO Songs (SongName, Artist, SongPath, LikeCount, CreateTime, SongCategoryID, PlayCount, SongStatus) 
    VALUES 
    ('給艾莉絲', 3, '/audio/sample.mp3', 31, '2018-11-20', 4, 2521, 1),		--這是bob的歌
    ('查理沒有茶包', 4, '/audio/sample.mp3', 12, '2019-03-17', 3, 1203, 1),	--這是查理的歌
	('不要BB', 3, '/audio/sample.mp3', 31, '2020-07-20', 4, 251, 1),		--這是bob的歌
    ('經過路面查看左右後方', 4, '/audio/sample.mp3', 12, '2021-04-17', 3, 120, 1);	--這是查理的歌

	--插入 LikeSongs
    INSERT INTO LikeSongs([UserID],[SongID]) 
    VALUES 
    (2,1),		
    (3,1),
	(3,2),
	(4,2);

	--插入 SongRank
	INSERT INTO SongRank ([SongID], [PreRank], [CurrentRank], [RankPeriod])
	VALUES
	(1, NULL, 1, '2025-01-01'), -- 新進榜
	(2, NULL, 2, '2025-01-01'), -- 新進榜  
	(3, NULL, 3, '2025-01-01'), -- 新進榜  
	(4, NULL, 4, '2025-01-01'), -- 新進榜
	(1, 1, 2, '2025-02-01'),    -- 掉一名
	(2, 2, 1, '2025-02-01'),    -- 升一名
	(3, 3, 3, '2025-02-01'),   
	(4, 4, 4, '2025-02-01');

	--插入 PlayList
    INSERT INTO PlayList ([UserID],[Name]) 
    VALUES 
    (2,'Alice的歌單'),
	(2,'Alice的歌單2'),
	(3,'bob的歌曲精選'),
	(4,'查理歌單');

	--插入 PlayListSongs
    INSERT INTO PlayListSongs([PlayListID],[SongID]) 
    VALUES 
    (1,1),
	(1,2),
	(3,1),
	(4,1);

	--插入 Favoriteplaylist
    INSERT INTO Favoriteplaylist ([UserID],[PlayListID]) 
    VALUES 
	(2,1),
	(2,2),
	(3,1),
	(3,2),
	(4,4);

	--插入 PlayHistory
    INSERT INTO PlayHistory([UserID],[SongID],[PlayTime]) 
    VALUES 
    (2,1,'2025-03-10'),
	(2,2,'2025-03-11'),
	(3,2,'2025-03-12'),
	(4,1,'2025-03-13'),
    (2,1,'2025-03-14');

	--插入 Activity
    INSERT INTO Activity([UploaderID],[EventCoverPath],[Title],[EventTime],[Location],[City],[ActivityDescription]) 
    VALUES 
	(2, '/image/Activity/cs.jpg','Alice 的生日演唱會','2025-06-01', '台北市民廣場','台北市','Alice將舉辦演唱會，一起用音樂點燃熱情！'),
	(3, '/image/Activity/cs.jpg','Bob爵士','2025-05-10','勤美綠園道','台中市','擅長薩克斯風的Bob，將會展現前所未見的獨特風格！'),
	(4, '/image/Activity/cs.jpg','Chainsmoker電音狂歡節', '2025-05-01','台北小巨蛋','台北市','雙人組合 The Chainsmokers，帶來一場震撼音樂盛會必定讓您嗨翻全場！');

	--插入 interested
    INSERT INTO Interested([UserID],[ActivityID]) 
    VALUES 
	(2,1),
	(3,1),
	(3,2),
	(2,2),
	(4,3),
	(2,3),
	(3,3);

	--插入 Selection
	DECLARE @UserID BIGINT;
	SET @UserID =(SELECT [UserID] FROM Users WHERE Account = 'admin')
	INSERT INTO [Selection] (
		   [UserID]
		  ,[Title]
		  ,[SelectionCoverPath]
		  ,[StartDate]
		  ,[EndDate]
		  ,[VotingStartDate]
		  ,[VotingEndDate]
		  ,[Description])
	VALUES
	(@UserID,'2025 夏日音樂大賽', '/image/selection/cover1.jpg', '2025-06-01', '2025-06-30', '2025-07-01', '2025-07-15', '夏日來臨，一起用音樂點燃熱情！'),
	(@UserID,'新聲代音樂徵選', '/image/selection/cover2.jpg', '2025-04-10', '2025-05-10', '2025-05-15', '2025-06-01', '發掘新生代音樂人才，讓你的聲音被世界聽見！'),
	(@UserID,'原創音樂大賞', '/image/selection/cover3.jpg', '2025-05-01', '2025-05-31', '2025-06-05', '2025-06-20', '原創音樂盛典，展現你的創作才華！'),
	(@UserID,'校園音樂徵選賽', '/image/selection/cover4.jpg', '2025-03-15', '2025-04-15', '2025-04-20', '2025-05-05', '專為學生打造的音樂徵選比賽，勇敢追夢！'),
	(@UserID,'電音製作人大賽', '/image/selection/cover5.jpg', '2025-07-01', '2025-07-31', '2025-08-05', '2025-08-20', '專為電子音樂製作人而生，展現你的DJ與混音技巧！'),
	(@UserID,'爵士新星徵選', '/image/selection/cover6.jpg', '2025-06-10', '2025-07-10', '2025-07-15', '2025-07-30', '尋找未來的爵士新星，展現你的獨特風格！'),
	(@UserID,'嘻哈饒舌大戰', '/image/selection/cover7.jpg', '2025-08-01', '2025-08-30', '2025-09-05', '2025-09-20', '你有炸裂的Flow嗎？來這裡挑戰最強饒舌歌手！'),
	(@UserID,'搖滾樂團徵選', '/image/selection/cover8.jpg', '2025-09-01', '2025-09-30', '2025-10-05', '2025-10-20', '搖滾魂不滅，帶著你的樂團來征服舞台！'),
	(@UserID,'民謠創作比賽', '/image/selection/cover9.jpg', '2025-05-20', '2025-06-20', '2025-06-25', '2025-07-10', '用民謠講故事，讓你的音樂感動人心！'),
	(@UserID,'流行金曲挑戰', '/image/selection/cover10.jpg', '2025-10-01', '2025-10-31', '2025-11-05', '2025-11-20', '挑戰流行金曲，看看誰能成為下一位音樂明星！'),
	(@UserID,'全球音樂新秀賽', '/image/selection/cover1.jpg', '2025-04-05', '2025-05-05', '2025-05-10', '2025-05-25', '來自世界各地的音樂新星，誰能脫穎而出？'),
	(@UserID,'嘻哈對決', '/image/selection/cover2.jpg', '2025-07-10', '2025-08-10', '2025-08-15', '2025-08-30', '誰才是最具實力的地下饒舌王？'),
	(@UserID,'經典翻唱大賽', '/image/selection/cover3.jpg', '2025-05-25', '2025-06-25', '2025-07-01', '2025-07-15', '翻唱經典歌曲，展現你獨特的音樂風格！'),
	(@UserID,'重金屬樂團決戰', '/image/selection/cover4.jpg', '2025-06-01', '2025-06-30', '2025-07-05', '2025-07-20', '重金屬狂熱，燃爆全場！'),
	(@UserID,'偶像歌手選拔', '/image/selection/cover5.jpg', '2025-09-05', '2025-09-30', '2025-10-05', '2025-10-25', '展現你的舞台魅力，成為下一位音樂偶像！'),
	(@UserID,'聲林之王選秀', '/image/selection/cover6.jpg', '2025-03-10', '2025-04-10', '2025-04-15', '2025-04-30', '發掘最具潛力的音樂之星！'),
	(@UserID,'電音Remix大賽', '/image/selection/cover7.jpg', '2025-08-10', '2025-09-10', '2025-09-15', '2025-09-30', '改編熱門曲目，秀出你的混音才華！'),
	(@UserID,'世界音樂之夜', '/image/selection/cover8.jpg', '2025-07-15', '2025-08-15', '2025-08-20', '2025-09-05', '融合不同文化的音樂風格，一場聽覺盛宴！'),
	(@UserID,'饒舌王之爭', '/image/selection/cover9.jpg', '2025-06-15', '2025-07-15', '2025-07-20', '2025-08-05', '快嘴對決，誰是最強饒舌高手？'),
	(@UserID,'亞洲偶像歌手賽', '/image/selection/cover10.jpg', '2025-10-05', '2025-10-31', '2025-11-05', '2025-11-25', '挑戰亞洲流行音樂，邁向偶像之路！'),
	(@UserID,'樂隊之巔', '/image/selection/cover1.jpg', '2025-05-10', '2025-06-10', '2025-06-15', '2025-06-30', '全國最強樂隊競技，一較高下！'),
	(@UserID,'R&B靈魂歌手選拔', '/image/selection/cover2.jpg', '2025-09-15', '2025-10-15', '2025-10-20', '2025-11-05', '展現你的靈魂樂嗓音，打動評審！'),
	(@UserID,'電影配樂創作大賽', '/image/selection/cover3.jpg', '2025-08-01', '2025-08-31', '2025-09-05', '2025-09-20', '創作最動人的電影配樂，讓旋律訴說故事！');

	--插入 SelectionDetail
	INSERT INTO SelectionDetail ([SelectionID],[SongID],[ReviewStatus])
	VALUES
	(1,1,1), --甄選1選項 1
	(1,2,1), --甄選1選項 2
	(2,3,1), --甄選2選項 1
	(2,4,1); --甄選2選項 2

	--插入 UserVoted
	INSERT INTO UserVoted (SelectionDetailID, UserID)
	VALUES
	(1, 2),   -- Alice		投給甄選1選項 1
	(2, 3),   -- Bob		投給甄選1選項 2
	(2, 4),   -- Charlie	投給甄選1選項 2
	(3, 2),   -- Alice		投給甄選2選項 1
	(3, 3),   -- Bob		投給甄選2選項 1
	(4, 4);   -- Charlie	投給甄選2選項 2

	--插入 SongComment
	INSERT INTO SongComments (SongID, UserID, Comment)
	VALUES
	(3, 3, '這首歌真的有夠炸！'),
	(4, 3, '副歌太洗腦了，已經單曲循環一整天了。'),
	(1, 2, '太甜了吧。'),
	(2, 3, '鼓點節奏感超讚，bassline 好沉穩。'),
	(4, 2, '電音節奏太炸，現場一定超嗨！'),
	(4, 3, '這首歌真的好有故事感，歌詞太打動人心了。'),
	(4, 4, '電音 Drop 的部分是不是超炸。'),
	(1, 2, 'Melody太好聽了，感覺可以上榜！'),
	(3, 4, 'Bob 這首爵士真的是神作，推薦給所有樂迷。');

	--插入 ActivityComment
	INSERT INTO ActivityComments (ActivityID, UserID, Comment)
	VALUES
	(1, 3, 'Alice 生日快樂！我一定到場支援!'),
	(1, 2, '謝謝各位<3。'),
	(2, 4, '爵士大好!!!!。'),
	(2, 3, '大好~!!!!!'),
	(3, 1, '這一次竟然在小巨蛋，好擠的感覺。'),
	(3, 4, '忠實粉絲簽到!'),
	(3, 3, '現場一定超嗨！');
END;
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:待審核 1:通過 2;未通過' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Activity', @level2type=N'COLUMN',@level2name=N'ApprovalStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'串接的廠商  [ecpay]綠界' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Donations', @level2type=N'COLUMN',@level2name=N'PaymentGateway'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'紀錄贊助' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Donations'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'對應綠界MerchantMemberID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay', @level2type=N'COLUMN',@level2name=N'DonationID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回傳的收款用戶註冊綠界時的ID(綠界辨識用)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay', @level2type=N'COLUMN',@level2name=N'MerchantID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回傳的交易編號,收款者可查詢交易狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay', @level2type=N'COLUMN',@level2name=N'TradeNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回傳的交易狀態碼。0 未付款,1成功' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay', @level2type=N'COLUMN',@level2name=N'RtnCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回傳的交易狀態訊息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay', @level2type=N'COLUMN',@level2name=N'RtnMsg'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'回傳的交易金額' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay', @level2type=N'COLUMN',@level2name=N'TradeAmt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付方式的類型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay', @level2type=N'COLUMN',@level2name=N'PaymentType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'支付手續費。 0表示沒有手續費' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay', @level2type=N'COLUMN',@level2name=N'PaymentTypeChargeFee'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'交易成立的時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay', @level2type=N'COLUMN',@level2name=N'MerchantTradeDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'紀錄贊助詳細資訊' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Ecpay'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0審核中 1通過 2失敗' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Songs', @level2type=N'COLUMN',@level2name=N'SongStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0未刪除 1已刪除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Songs', @level2type=N'COLUMN',@level2name=N'IsRemove'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0刪除 1啟用 2停用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Users', @level2type=N'COLUMN',@level2name=N'Status'
GO
USE [master]
GO
ALTER DATABASE [VocalSpaceDB] SET  READ_WRITE 
GO
