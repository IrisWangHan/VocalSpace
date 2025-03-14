USE [master]
GO
/****** Object:  Database [VocalSpaceDB]    Script Date: 2025/3/10 下午 05:22:11 ******/
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
/****** Object:  Table [dbo].[Activity]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Activity](
	[ActivityID] [bigint] IDENTITY(1,1) NOT NULL,
	[UploaderID] [bigint] NOT NULL,
	[EventCoverPath] [nvarchar](255) NULL,
	[EventTime] [datetime2](7) NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Location] [nvarchar](50) NOT NULL,
	[City] [nvarchar](5) NOT NULL,
	[ApprovalStatus] [tinyint] NULL,
	[CreateTime] [datetime2](7) NULL,
	[ActivityDescription] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ActivityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActivityComments]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityComments](
	[ActivityCommentID] [int] IDENTITY(1,1) NOT NULL,
	[ActivityID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Comment] [nvarchar](255) NOT NULL,
	[CommentTime] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ActivityCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Authority]    Script Date: 2025/3/10 下午 05:22:11 ******/
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
/****** Object:  Table [dbo].[Donations]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Donations](
	[DonationID] [bigint] IDENTITY(1,1) NOT NULL,
	[SponsorID] [bigint] NOT NULL,
	[ReceiverID] [bigint] NOT NULL,
	[PaymentGateway] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime2](7) NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[DonationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ecpay]    Script Date: 2025/3/10 下午 05:22:11 ******/
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
/****** Object:  Table [dbo].[Favoriteplaylist]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Favoriteplaylist](
	[PlayListID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[CreateTime] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[PlayListID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Interested]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Interested](
	[InterestedID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[ActivityID] [bigint] NOT NULL,
	[InterestedDate] [datetime2](7) NULL,
 CONSTRAINT [PK__Interest__4F84193ECCCE3ACD] PRIMARY KEY CLUSTERED 
(
	[InterestedID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LikeSongs]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LikeSongs](
	[LikeID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[SongID] [bigint] NOT NULL,
	[CreateTime] [datetime2](7) NULL,
 CONSTRAINT [PK__LikeSong__85F843E85F24E7A1] PRIMARY KEY CLUSTERED 
(
	[LikeID] ASC,
	[UserID] ASC,
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayList]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayList](
	[PlayListID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[CoverImagePath] [nvarchar](255) NULL,
	[CreateTime] [datetime2](7) NULL,
	[PlaylistDescription] [nvarchar](255) NULL,
	[IsPublic] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PlayListID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlayListSongs]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayListSongs](
	[PlayListID] [bigint] NOT NULL,
	[SongID] [bigint] NOT NULL,
	[CreateTime] [datetime2](7) NULL,
 CONSTRAINT [PK__PlayList__595EA2D4E5686B5E] PRIMARY KEY CLUSTERED 
(
	[PlayListID] ASC,
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Selection]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Selection](
	[SelectionID] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](20) NOT NULL,
	[SelectionCoverPath] [nvarchar](255) NOT NULL,
	[CreateTime] [datetime2](7) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[VotingStartDate] [datetime2](7) NULL,
	[VotingEndDate] [datetime2](7) NULL,
	[Visible] [bit] NULL,
	[Description] [nvarchar](255) NOT NULL,
	[UserID] [bigint] NOT NULL,
 CONSTRAINT [PK__Selectio__7F17912F0F589520] PRIMARY KEY CLUSTERED 
(
	[SelectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SelectionDetail]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SelectionDetail](
	[SelectionDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[SelectionID] [bigint] NOT NULL,
	[SongID] [bigint] NOT NULL,
	[VoteCount] [int] NULL,
	[CreateTime] [datetime2](7) NULL,
 CONSTRAINT [PK__Selectio__BFA38A20B31A69DA] PRIMARY KEY CLUSTERED 
(
	[SelectionDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SongCategory]    Script Date: 2025/3/10 下午 05:22:11 ******/
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
/****** Object:  Table [dbo].[SongComments]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SongComments](
	[SongCommentID] [int] IDENTITY(1,1) NOT NULL,
	[SongID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Comment] [nvarchar](255) NOT NULL,
	[CommentTime] [datetime2](7) NULL,
 CONSTRAINT [PK__SongComm__CDDE91BDF671567C] PRIMARY KEY CLUSTERED 
(
	[SongCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SongRank]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SongRank](
	[SongID] [bigint] NOT NULL,
	[PreRank] [tinyint] NOT NULL,
	[CurrentRank] [tinyint] NOT NULL,
 CONSTRAINT [PK__SongRank__12E3D6F75D69C5FC] PRIMARY KEY CLUSTERED 
(
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Songs]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Songs](
	[SongPath] [nvarchar](255) NOT NULL,
	[SongID] [bigint] IDENTITY(1,1) NOT NULL,
	[Artist] [bigint] NOT NULL,
	[CoverPhotoPath] [nvarchar](255) NULL,
	[LikeCount] [int] NULL,
	[CreateTime] [datetime2](7) NULL,
	[SongDescription] [nvarchar](255) NULL,
	[Lyrics] [nvarchar](max) NULL,
	[SongStatus] [tinyint] NOT NULL,
	[IsRemove] [bit] NULL,
	[SongCategoryID] [tinyint] NOT NULL,
	[SongName] [nvarchar](20) NOT NULL,
	[PlayCount] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserFollows]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserFollows](
	[UserID] [bigint] NOT NULL,
	[FollowedUserID] [bigint] NOT NULL,
	[FollowTime] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[FollowedUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2025/3/10 下午 05:22:11 ******/
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
	[CreateTime] [datetime2](7) NULL,
	[Status] [bit] NULL,
 CONSTRAINT [PK__Users__1788CCACF244AD17] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsersInfo]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UsersInfo](
	[UserID] [bigint] NOT NULL,
	[BannerImagePath] [nvarchar](255) NULL,
	[AvatarPath] [nvarchar](255) NULL,
	[Birthday] [date] NULL,
	[PersonalIntroduction] [nvarchar](max) NULL,
	[Email] [nvarchar](254) NOT NULL,
 CONSTRAINT [PK__UsersInf__1788CCAC81CC84F7] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserVoted]    Script Date: 2025/3/10 下午 05:22:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserVoted](
	[SelectionDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[VoteTime] [datetime2](7) NULL,
 CONSTRAINT [PK__UserVote__6EDB06EA57146ADA] PRIMARY KEY CLUSTERED 
(
	[SelectionDetailID] ASC,
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Authority] ON 

INSERT [dbo].[Authority] ([AuthorityID], [Name]) VALUES (1, N'user')
INSERT [dbo].[Authority] ([AuthorityID], [Name]) VALUES (2, N'manager')
INSERT [dbo].[Authority] ([AuthorityID], [Name]) VALUES (3, N'admin')
SET IDENTITY_INSERT [dbo].[Authority] OFF
GO
SET IDENTITY_INSERT [dbo].[Selection] ON 

INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (1, N'2025 夏日音樂大賽', N'/image/selection/cover1.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), 1, N'夏日來臨，一起用音樂點燃熱情！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (2, N'新聲代音樂徵選', N'/image/selection/cover2.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-04-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-01T00:00:00.0000000' AS DateTime2), 1, N'發掘新生代音樂人才，讓你的聲音被世界聽見！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (3, N'原創音樂大賞', N'/image/selection/cover3.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-05-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-20T00:00:00.0000000' AS DateTime2), 1, N'原創音樂盛典，展現你的創作才華！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (4, N'校園音樂徵選賽', N'/image/selection/cover4.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-03-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-05T00:00:00.0000000' AS DateTime2), 1, N'專為學生打造的音樂徵選比賽，勇敢追夢！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (5, N'電音製作人大賽', N'/image/selection/cover5.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-07-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-20T00:00:00.0000000' AS DateTime2), 1, N'專為電子音樂製作人而生，展現你的DJ與混音技巧！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (6, N'爵士新星徵選', N'/image/selection/cover6.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-06-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-30T00:00:00.0000000' AS DateTime2), 1, N'尋找未來的爵士新星，展現你的獨特風格！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (7, N'嘻哈饒舌大戰', N'/image/selection/cover7.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-08-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-20T00:00:00.0000000' AS DateTime2), 1, N'你有炸裂的Flow嗎？來這裡挑戰最強饒舌歌手！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (8, N'搖滾樂團徵選', N'/image/selection/cover8.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-09-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-20T00:00:00.0000000' AS DateTime2), 1, N'搖滾魂不滅，帶著你的樂團來征服舞台！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (9, N'民謠創作比賽', N'/image/selection/cover9.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-05-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-25T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-10T00:00:00.0000000' AS DateTime2), 1, N'用民謠講故事，讓你的音樂感動人心！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (10, N'流行金曲挑戰', N'/image/selection/cover10.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-10-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-20T00:00:00.0000000' AS DateTime2), 1, N'挑戰流行金曲，看看誰能成為下一位音樂明星！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (11, N'全球音樂新秀賽', N'/image/selection/cover1.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-04-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-25T00:00:00.0000000' AS DateTime2), 1, N'來自世界各地的音樂新星，誰能脫穎而出？', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (12, N'嘻哈對決', N'/image/selection/cover2.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-07-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-30T00:00:00.0000000' AS DateTime2), 1, N'誰才是最具實力的地下饒舌王？', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (13, N'經典翻唱大賽', N'/image/selection/cover3.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-05-25T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-25T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), 1, N'翻唱經典歌曲，展現你獨特的音樂風格！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (14, N'重金屬樂團決戰', N'/image/selection/cover4.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-20T00:00:00.0000000' AS DateTime2), 1, N'重金屬狂熱，燃爆全場！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (15, N'偶像歌手選拔', N'/image/selection/cover5.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-09-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-30T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-25T00:00:00.0000000' AS DateTime2), 1, N'展現你的舞台魅力，成為下一位音樂偶像！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (16, N'聲林之王選秀', N'/image/selection/cover6.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-03-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-04-30T00:00:00.0000000' AS DateTime2), 1, N'發掘最具潛力的音樂之星！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (17, N'電音Remix大賽', N'/image/selection/cover7.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-08-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-30T00:00:00.0000000' AS DateTime2), 1, N'改編熱門曲目，秀出你的混音才華！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (18, N'世界音樂之夜', N'/image/selection/cover8.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-05T00:00:00.0000000' AS DateTime2), 1, N'融合不同文化的音樂風格，一場聽覺盛宴！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (19, N'饒舌王之爭', N'/image/selection/cover9.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-06-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-07-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-05T00:00:00.0000000' AS DateTime2), 1, N'快嘴對決，誰是最強饒舌高手？', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (20, N'亞洲偶像歌手賽', N'/image/selection/cover10.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-10-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-25T00:00:00.0000000' AS DateTime2), 1, N'挑戰亞洲流行音樂，邁向偶像之路！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (21, N'樂隊之巔', N'/image/selection/cover1.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-05-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-10T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-06-30T00:00:00.0000000' AS DateTime2), 1, N'全國最強樂隊競技，一較高下！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (22, N'R&B靈魂歌手選拔', N'/image/selection/cover2.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-09-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-15T00:00:00.0000000' AS DateTime2), CAST(N'2025-10-20T00:00:00.0000000' AS DateTime2), CAST(N'2025-11-05T00:00:00.0000000' AS DateTime2), 1, N'展現你的靈魂樂嗓音，打動評審！', 9)
INSERT [dbo].[Selection] ([SelectionID], [Title], [SelectionCoverPath], [CreateTime], [StartDate], [EndDate], [VotingStartDate], [VotingEndDate], [Visible], [Description], [UserID]) VALUES (23, N'電影配樂創作大賽', N'/image/selection/cover3.jpg', CAST(N'2025-03-10T14:25:16.8466667' AS DateTime2), CAST(N'2025-08-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-08-31T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-05T00:00:00.0000000' AS DateTime2), CAST(N'2025-09-20T00:00:00.0000000' AS DateTime2), 1, N'創作最動人的電影配樂，讓旋律訴說故事！', 9)
SET IDENTITY_INSERT [dbo].[Selection] OFF
GO
SET IDENTITY_INSERT [dbo].[SongCategory] ON 

INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (1, N'搖滾')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (2, N'民謠')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (3, N'嘻哈')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (4, N'都會')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (5, N'電子')
INSERT [dbo].[SongCategory] ([SongCategoryID], [CategoryName]) VALUES (6, N'探索')
SET IDENTITY_INSERT [dbo].[SongCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [UserName], [AuthorityID], [Account], [Password], [TempPassword], [CreateTime], [Status]) VALUES (9, N'', 2, N'AAA', N'AAA123', N'', CAST(N'2025-03-06T16:39:41.4400000' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
INSERT [dbo].[UsersInfo] ([UserID], [BannerImagePath], [AvatarPath], [Birthday], [PersonalIntroduction], [Email]) VALUES (9, N'', N'', CAST(N'2025-03-06' AS Date), N'HELLO WORLD!', N'AAA123@gmail.com')
GO
/****** Object:  Index [UQ__Activity__45F4A7F09510B12B]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[Activity] ADD UNIQUE NONCLUSTERED 
(
	[ActivityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Activity__CDDE91BC7B0CD57B]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[ActivityComments] ADD UNIQUE NONCLUSTERED 
(
	[ActivityCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Authorit__433B1E6C0F417D64]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[Authority] ADD UNIQUE NONCLUSTERED 
(
	[AuthorityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Interest__45F4A7F09CF90B37]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[Interested] ADD  CONSTRAINT [UQ__Interest__45F4A7F09CF90B37] UNIQUE NONCLUSTERED 
(
	[ActivityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__PlayList__38709FBA3A337F35]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[PlayList] ADD UNIQUE NONCLUSTERED 
(
	[PlayListID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__PlayList__12E3D6F68D7A392C]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[PlayListSongs] ADD  CONSTRAINT [UQ__PlayList__12E3D6F68D7A392C] UNIQUE NONCLUSTERED 
(
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__SongCate__BD6CF5D880EFE2AD]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[SongCategory] ADD UNIQUE NONCLUSTERED 
(
	[SongCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__SongComm__CDDE91BCC8084B24]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[SongComments] ADD  CONSTRAINT [UQ__SongComm__CDDE91BCC8084B24] UNIQUE NONCLUSTERED 
(
	[SongCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__SongRank__12E3D6F6353DFB7C]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[SongRank] ADD  CONSTRAINT [UQ__SongRank__12E3D6F6353DFB7C] UNIQUE NONCLUSTERED 
(
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Songs__12E3D6F67B409495]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[Songs] ADD UNIQUE NONCLUSTERED 
(
	[SongID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [IX_Users] UNIQUE NONCLUSTERED 
(
	[Account] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__Users__1788CCAD1150EEC1]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ__Users__1788CCAD1150EEC1] UNIQUE NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_UsersInfo]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[UsersInfo] ADD  CONSTRAINT [IX_UsersInfo] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [UQ__UsersInf__1788CCAD339DF13E]    Script Date: 2025/3/10 下午 05:22:11 ******/
ALTER TABLE [dbo].[UsersInfo] ADD  CONSTRAINT [UQ__UsersInf__1788CCAD339DF13E] UNIQUE NONCLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Activity] ADD  DEFAULT (getdate()) FOR [EventTime]
GO
ALTER TABLE [dbo].[Activity] ADD  DEFAULT ((0)) FOR [ApprovalStatus]
GO
ALTER TABLE [dbo].[Activity] ADD  CONSTRAINT [DF_Activity_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[ActivityComments] ADD  CONSTRAINT [DF_ActivityComments_CommentTime]  DEFAULT (getdate()) FOR [CommentTime]
GO
ALTER TABLE [dbo].[Donations] ADD  CONSTRAINT [DF_Payment_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Ecpay] ADD  CONSTRAINT [DF_Ecpay_TradeNo]  DEFAULT ('') FOR [MerchantID]
GO
ALTER TABLE [dbo].[Favoriteplaylist] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Interested] ADD  CONSTRAINT [DF__Intereste__Inter__68487DD7]  DEFAULT (getdate()) FOR [InterestedDate]
GO
ALTER TABLE [dbo].[LikeSongs] ADD  CONSTRAINT [DF__LikeSongs__Creat__5441852A]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[PlayList] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[PlayList] ADD  DEFAULT ((1)) FOR [IsPublic]
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
ALTER TABLE [dbo].[SongComments] ADD  CONSTRAINT [DF_CommentTime]  DEFAULT (getdate()) FOR [CommentTime]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_CoverPhotoPath]  DEFAULT ('') FOR [CoverPhotoPath]
GO
ALTER TABLE [dbo].[Songs] ADD  DEFAULT ((0)) FOR [LikeCount]
GO
ALTER TABLE [dbo].[Songs] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_SongDescription]  DEFAULT ('') FOR [SongDescription]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_Lyrics]  DEFAULT ('') FOR [Lyrics]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_IsRemove]  DEFAULT ((0)) FOR [IsRemove]
GO
ALTER TABLE [dbo].[Songs] ADD  CONSTRAINT [DF_PlayCount]  DEFAULT ((0)) FOR [PlayCount]
GO
ALTER TABLE [dbo].[UserFollows] ADD  DEFAULT (getdate()) FOR [FollowTime]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_UserName]  DEFAULT ('') FOR [UserName]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_TempPassword]  DEFAULT ('') FOR [TempPassword]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__Status__3B75D760]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[UsersInfo] ADD  CONSTRAINT [DF_BannerImagePath]  DEFAULT ('') FOR [BannerImagePath]
GO
ALTER TABLE [dbo].[UsersInfo] ADD  CONSTRAINT [DF_AvatarPath]  DEFAULT ('') FOR [AvatarPath]
GO
ALTER TABLE [dbo].[UsersInfo] ADD  CONSTRAINT [DF_Birthday]  DEFAULT (CONVERT([date],getdate())) FOR [Birthday]
GO
ALTER TABLE [dbo].[UsersInfo] ADD  CONSTRAINT [DF_PersonalIntroduction]  DEFAULT ('') FOR [PersonalIntroduction]
GO
ALTER TABLE [dbo].[UserVoted] ADD  CONSTRAINT [DF__UserVoted__VoteT__6B24EA82]  DEFAULT (getdate()) FOR [VoteTime]
GO
ALTER TABLE [dbo].[Activity]  WITH CHECK ADD  CONSTRAINT [FK__Activity__Upload__00200768] FOREIGN KEY([UploaderID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Activity] CHECK CONSTRAINT [FK__Activity__Upload__00200768]
GO
ALTER TABLE [dbo].[ActivityComments]  WITH CHECK ADD FOREIGN KEY([ActivityID])
REFERENCES [dbo].[Activity] ([ActivityID])
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
ALTER TABLE [dbo].[Favoriteplaylist]  WITH CHECK ADD FOREIGN KEY([PlayListID])
REFERENCES [dbo].[PlayList] ([PlayListID])
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
ALTER TABLE [dbo].[Songs]  WITH CHECK ADD FOREIGN KEY([SongCategoryID])
REFERENCES [dbo].[SongCategory] ([SongCategoryID])
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
/****** Object:  StoredProcedure [dbo].[AddAuthority]    Script Date: 2025/3/10 下午 05:22:11 ******/
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
/****** Object:  StoredProcedure [dbo].[AddSelection]    Script Date: 2025/3/10 下午 05:22:11 ******/
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

	-----------------------------------------------

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
/****** Object:  StoredProcedure [dbo].[AddSongCategory]    Script Date: 2025/3/10 下午 05:22:11 ******/
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
USE [master]
GO
ALTER DATABASE [VocalSpaceDB] SET  READ_WRITE 
GO
