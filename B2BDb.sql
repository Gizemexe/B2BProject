USE [master]
GO
/****** Object:  Database [B2BDb]    Script Date: 3.06.2024 20:58:53 ******/
CREATE DATABASE [B2BDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'B2BDbb', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\B2BDbb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'B2BDbb_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\B2BDbb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [B2BDb] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [B2BDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [B2BDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [B2BDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [B2BDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [B2BDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [B2BDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [B2BDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [B2BDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [B2BDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [B2BDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [B2BDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [B2BDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [B2BDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [B2BDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [B2BDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [B2BDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [B2BDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [B2BDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [B2BDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [B2BDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [B2BDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [B2BDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [B2BDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [B2BDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [B2BDb] SET  MULTI_USER 
GO
ALTER DATABASE [B2BDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [B2BDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [B2BDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [B2BDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [B2BDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [B2BDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [B2BDb] SET QUERY_STORE = ON
GO
ALTER DATABASE [B2BDb] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [B2BDb]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 3.06.2024 20:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[Cart_id] [int] IDENTITY(1,1) NOT NULL,
	[User_id] [int] NOT NULL,
	[Product_id] [int] NOT NULL,
	[Quantity] [int] NULL,
	[Price] [decimal](18, 0) NULL,
	[Total] [decimal](18, 0) NULL,
	[Options] [nvarchar](50) NULL,
 CONSTRAINT [PK_Cartt] PRIMARY KEY CLUSTERED 
(
	[Cart_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 3.06.2024 20:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Category_id] [int] IDENTITY(1,1) NOT NULL,
	[Category_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Categoriess] PRIMARY KEY CLUSTERED 
(
	[Category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coupons]    Script Date: 3.06.2024 20:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupons](
	[Coupon_id] [int] IDENTITY(1,1) NOT NULL,
	[Coupon_Code] [nvarchar](50) NOT NULL,
	[Discount] [decimal](18, 0) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[User_id] [int] NOT NULL,
	[Product_id] [int] NOT NULL,
	[Start_date] [datetime] NOT NULL,
	[End_date] [datetime] NOT NULL,
 CONSTRAINT [PK_Coupons] PRIMARY KEY CLUSTERED 
(
	[Coupon_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messaging]    Script Date: 3.06.2024 20:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messaging](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Sender_Id] [int] NOT NULL,
	[Recipient_Id] [int] NOT NULL,
	[Message] [nvarchar](max) NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_Messaging] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 3.06.2024 20:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Order_id] [int] NOT NULL,
	[Product_id] [int] NOT NULL,
	[Quantity] [int] NULL,
	[UnitPrice] [decimal](18, 0) NULL,
	[Total] [decimal](18, 0) NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 3.06.2024 20:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Order_id] [int] IDENTITY(1,1) NOT NULL,
	[User_id] [int] NOT NULL,
	[Date] [datetime] NULL,
	[Total] [decimal](18, 0) NULL,
	[Address] [nvarchar](max) NULL,
	[Order_status] [nvarchar](50) NULL,
	[Payment_status] [nvarchar](50) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 3.06.2024 20:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Product_id] [int] IDENTITY(1,1) NOT NULL,
	[Product_code] [nvarchar](50) NOT NULL,
	[Product_name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Category_id] [int] NOT NULL,
	[User_id] [int] NOT NULL,
	[Price] [int] NULL,
	[Stock] [int] NULL,
	[Image] [nvarchar](max) NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 3.06.2024 20:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Rol_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Rols] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 3.06.2024 20:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_id] [int] IDENTITY(1,1) NOT NULL,
	[Rol_id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[Company_name] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Userss] PRIMARY KEY CLUSTERED 
(
	[User_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cart] ON 

INSERT [dbo].[Cart] ([Cart_id], [User_id], [Product_id], [Quantity], [Price], [Total], [Options]) VALUES (21, 2, 14, 1, CAST(24 AS Decimal(18, 0)), CAST(24 AS Decimal(18, 0)), N'Adet')
SET IDENTITY_INSERT [dbo].[Cart] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Category_id], [Category_name]) VALUES (1, N'Cosmetic')
INSERT [dbo].[Categories] ([Category_id], [Category_name]) VALUES (2, N'Cleaning')
INSERT [dbo].[Categories] ([Category_id], [Category_name]) VALUES (3, N'Stationery')
INSERT [dbo].[Categories] ([Category_id], [Category_name]) VALUES (4, N'Food')
INSERT [dbo].[Categories] ([Category_id], [Category_name]) VALUES (5, N'Fabric')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Coupons] ON 

INSERT [dbo].[Coupons] ([Coupon_id], [Coupon_Code], [Discount], [IsActive], [User_id], [Product_id], [Start_date], [End_date]) VALUES (3, N'INDIRIM50', CAST(50 AS Decimal(18, 0)), 1, 5, 1, CAST(N'2024-06-02T23:07:40.840' AS DateTime), CAST(N'2024-06-04T00:00:00.000' AS DateTime))
INSERT [dbo].[Coupons] ([Coupon_id], [Coupon_Code], [Discount], [IsActive], [User_id], [Product_id], [Start_date], [End_date]) VALUES (4, N'INDIRIM25', CAST(25 AS Decimal(18, 0)), 0, 5, 2, CAST(N'2024-06-02T23:31:53.977' AS DateTime), CAST(N'2024-06-03T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Coupons] OFF
GO
SET IDENTITY_INSERT [dbo].[Messaging] ON 

INSERT [dbo].[Messaging] ([Id], [Sender_Id], [Recipient_Id], [Message], [Date]) VALUES (1, 2, 3, N'aaaaa', CAST(N'2024-06-03T16:35:56.790' AS DateTime))
INSERT [dbo].[Messaging] ([Id], [Sender_Id], [Recipient_Id], [Message], [Date]) VALUES (2, 3, 3, N'müşteriye cevap verildi.', CAST(N'2024-06-03T16:45:29.697' AS DateTime))
INSERT [dbo].[Messaging] ([Id], [Sender_Id], [Recipient_Id], [Message], [Date]) VALUES (4, 2, 6, NULL, CAST(N'2024-06-03T20:45:59.030' AS DateTime))
INSERT [dbo].[Messaging] ([Id], [Sender_Id], [Recipient_Id], [Message], [Date]) VALUES (5, 2, 6, NULL, CAST(N'2024-06-03T20:46:03.770' AS DateTime))
INSERT [dbo].[Messaging] ([Id], [Sender_Id], [Recipient_Id], [Message], [Date]) VALUES (6, 2, 5, NULL, CAST(N'2024-06-03T20:46:25.717' AS DateTime))
INSERT [dbo].[Messaging] ([Id], [Sender_Id], [Recipient_Id], [Message], [Date]) VALUES (7, 2, 3, NULL, CAST(N'2024-06-03T20:48:27.597' AS DateTime))
SET IDENTITY_INSERT [dbo].[Messaging] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (1, 1, 1, 2, CAST(12 AS Decimal(18, 0)), CAST(260 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (2, 1, 12, 1, CAST(250 AS Decimal(18, 0)), CAST(250 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (3, 1, 2, 1, CAST(20 AS Decimal(18, 0)), CAST(20 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (4, 2, 27, 1, CAST(630 AS Decimal(18, 0)), CAST(630 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (5, 2, 1, 1, CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (6, 3, 2, 1, CAST(20 AS Decimal(18, 0)), CAST(20 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (7, 3, 1, 1, CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (8, 4, 1, 1, CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (9, 4, 16, 1, CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (10, 5, 23, 1, CAST(88 AS Decimal(18, 0)), CAST(88 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (11, 5, 1, 1, CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (12, 6, 5, 1, CAST(124 AS Decimal(18, 0)), CAST(124 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (13, 6, 1, 2, CAST(12 AS Decimal(18, 0)), CAST(24 AS Decimal(18, 0)))
INSERT [dbo].[OrderDetails] ([Id], [Order_id], [Product_id], [Quantity], [UnitPrice], [Total]) VALUES (14, 7, 1, 1, CAST(12 AS Decimal(18, 0)), CAST(12 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([Order_id], [User_id], [Date], [Total], [Address], [Order_status], [Payment_status]) VALUES (1, 2, CAST(N'2024-06-03T12:41:27.750' AS DateTime), NULL, N'aaaaaaaaaaaaaaaaaa', N'Pending', N'Unpaid')
INSERT [dbo].[Orders] ([Order_id], [User_id], [Date], [Total], [Address], [Order_status], [Payment_status]) VALUES (2, 2, CAST(N'2024-06-03T12:57:24.853' AS DateTime), NULL, N'aaaaaaaaaaaaaaaaaa', N'Pending', N'Unpaid')
INSERT [dbo].[Orders] ([Order_id], [User_id], [Date], [Total], [Address], [Order_status], [Payment_status]) VALUES (3, 2, CAST(N'2024-06-03T13:26:05.613' AS DateTime), NULL, N'', N'Pending', N'Unpaid')
INSERT [dbo].[Orders] ([Order_id], [User_id], [Date], [Total], [Address], [Order_status], [Payment_status]) VALUES (4, 2, CAST(N'2024-06-03T13:27:00.980' AS DateTime), NULL, N'xxxxxxxxxxxxxx', N'Pending', N'Unpaid')
INSERT [dbo].[Orders] ([Order_id], [User_id], [Date], [Total], [Address], [Order_status], [Payment_status]) VALUES (5, 2, CAST(N'2024-06-03T13:29:13.230' AS DateTime), NULL, N'', N'Pending', N'Unpaid')
INSERT [dbo].[Orders] ([Order_id], [User_id], [Date], [Total], [Address], [Order_status], [Payment_status]) VALUES (6, 2, CAST(N'2024-06-03T13:31:28.600' AS DateTime), NULL, N'', N'Pending', N'Unpaid')
INSERT [dbo].[Orders] ([Order_id], [User_id], [Date], [Total], [Address], [Order_status], [Payment_status]) VALUES (7, 2, CAST(N'2024-06-03T13:39:09.950' AS DateTime), CAST(112 AS Decimal(18, 0)), N'', N'Pending', N'Unpaid')
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (1, N'101200', N'Asperox', N'Asperox Süper Sarı Güç', 2, 3, 12, 91, N'Content/Images/Asperox.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (2, N'101201', N'Domestos', N'Domestos Etkili Güç', 2, 3, 20, 98, N'Content/Images/Domestos.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (3, N'101202', N'Camsil', N'Camsil ile camlarınız ayna gibi olsun! ', 2, 3, 5, 100, N'Content/Images/camsil.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (4, N'101203', N'Omo', N'Omo Deterjanın Benzersiz Parfümüyle Mükemmel Temizlik, Mis Gibi Koku! ', 2, 3, 85, 200, N'Content/Images/omo.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (5, N'101204', N'Pronto', N'
Pronto Ahşap Yüzey Temizleyici Klasik 750ml', 2, 3, 124, 499, N'Content/Images/pronto.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (7, N'101205', N'Sponge', N'	
Deep Bright Scrubber Sponge, Bulaşık Süngeri - Çift Taraflı, Çizmez', 2, 3, 25, 250, N'Content/Images/sponge.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (8, N'102200', N'Foundation', N'Maybelline Fit Me Matte+Poreless Fondöten - 112 Soft Beige', 1, 5, 350, 500, N'Content/Images/foundation.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (10, N'102201', N'Mac Powder', N'Pudramsı dokunuşu hisset! Powder Kiss göz farı mat dokusuyla göz kapaklarında yumuşak ve pudramsı bir his yaratır. ', 1, 5, 600, 500, N'Content/Images/powder.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (11, N'102202', N'Highlighter', N'PASTEL MOONLIGHT HIGHLIGHTER 100', 1, 5, 150, 800, N'Content/Images/highlighter.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (12, N'102203', N'Lipstick', N'Maybelline New York Super Stay Vinyl Ink Likit Parlak Ruj ', 1, 5, 250, 399, N'Content/Images/lipstick.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (13, N'102204', N'Mascara', N'MAYBELLINE mascara lash sensational intense Black', 1, 5, 373, 200, N'Content/Images/mascara.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (14, N'102205', N'Nail Polish', N'Beaulis Paint It Oje Pink', 1, 5, 24, 100, N'Content/Images/nailpolish.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (15, N'103200', N'Eraser', N'PEN toptan satış Pentel Hi-polimer  ', 3, 3, 5, 150, N'Content/Images/eraser.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (16, N'103201', N'Notebook', N'notebook', 3, 3, 12, 349, N'Content/Images/notebook.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (17, N'103202', N'Paper', N'Copier Bond Fotokopi A4 Kağıdı 80 G
', 3, 3, 107, 120, N'Content/Images/paper.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (18, N'103203', N'Pen', N'Dip Pen Set Kaligrafi Kalemi ', 3, 3, 252, 240, N'Content/Images/pen.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (19, N'103204', N'Post it', N'Modellino Tükenmeyen Yapışkan Postit ', 3, 3, 69, 150, N'Content/Images/postit.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (20, N'103205', N'Scissor', N'MAS 410 Scissors Fantastic', 3, 3, 49, 200, N'Content/Images/scissors.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (23, N'104200', N'Chicken', N'chicken', 4, 6, 88, 99, N'Content/Images/chicken.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (24, N'104201', N'Fish', N'Fresh Fish', 4, 6, 239, 50, N'Content/Images/fish.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (25, N'104202', N'Oil', N'Olive Oil', 4, 6, 450, 100, N'Content/Images/oil.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (26, N'104203', N'Salt', N'Salt', 4, 6, 229, 500, N'Content/Images/salt.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (27, N'104204', N'Steak', N'Steak', 4, 6, 630, 99, N'Content/Images/steak.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (28, N'104205', N'Tea', N'Lipton Turkish Rize Tea', 4, 6, 250, 250, N'Content/Images/tea.jpeg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (29, N'105200', N'Chevron', N'Chevron ', 5, 7, 133, 100, N'Content/Images/chevron.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (30, N'105201', N'Cotton', N'Cotton', 5, 7, 100, 100, N'Content/Images/cotton.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (31, N'105202', N'Floral', N'Floral', 5, 7, 90, 100, N'Content/Images/floral.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (32, N'105203', N'Lace', N'Lace', 5, 7, 350, 100, N'Content/Images/lace.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (33, N'105204', N'Silk', N'Silk', 5, 7, 188, 100, N'Content/Images/silk.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (34, N'105205', N'Linen', N'Linen', 5, 7, 230, 100, N'Content/Images/linen.jpg')
INSERT [dbo].[Products] ([Product_id], [Product_code], [Product_name], [Description], [Category_id], [User_id], [Price], [Stock], [Image]) VALUES (36, N'101206', N'Alo', N'Alo Toz Deterjan, Lekelerden Korkmayın!', 2, 5, 120, 100, N'Content/Images/149696131alo.jpg')
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([id], [Rol_name]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([id], [Rol_name]) VALUES (2, N'Seller')
INSERT [dbo].[Roles] ([id], [Rol_name]) VALUES (3, N'Buyer')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([User_id], [Rol_id], [Name], [Surname], [Company_name], [Email], [Phone], [Password]) VALUES (1, 3, N'xxxxx', N'xxxxx', N'xxxxx', N'xxxxxxxxxxxx', N'xxxxxxxxxxxxxxxx', N'123456asdf')
INSERT [dbo].[Users] ([User_id], [Rol_id], [Name], [Surname], [Company_name], [Email], [Phone], [Password]) VALUES (2, 3, N'xxxxx', N'aaaaabbb', N'aaaaa', N'aaaaaaaaaa', N'12345678911', N'aaaaaa122')
INSERT [dbo].[Users] ([User_id], [Rol_id], [Name], [Surname], [Company_name], [Email], [Phone], [Password]) VALUES (3, 2, N'xxxxx', N'aaaaaaaaaaaaa', N'aaaa', N'aaaa', N'aaaaa', N'aaaaaaa')
INSERT [dbo].[Users] ([User_id], [Rol_id], [Name], [Surname], [Company_name], [Email], [Phone], [Password]) VALUES (4, 1, N'Admin', N'adminn', N'-', N'admin@admin.com', N'12345678910', N'admin123')
INSERT [dbo].[Users] ([User_id], [Rol_id], [Name], [Surname], [Company_name], [Email], [Phone], [Password]) VALUES (5, 2, N'Ali', N'SAĞLAM', N'Sağlam Toptan Market', N'alisağlam@hotmail.com', N'12345678911', N'alisağlam')
INSERT [dbo].[Users] ([User_id], [Rol_id], [Name], [Surname], [Company_name], [Email], [Phone], [Password]) VALUES (6, 2, N'Ayşe', N'BETON', N'BETON Toptan Market', N'beton@hotmail.com', N'12345678911', N'ayşebeton')
INSERT [dbo].[Users] ([User_id], [Rol_id], [Name], [Surname], [Company_name], [Email], [Phone], [Password]) VALUES (7, 2, N'Max', N'Maxima', N'Max Fabric ', N'max@hotmail.com', N'12345678911', N'max12345')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[Coupons] ADD  CONSTRAINT [DF_Coupons_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_Products] FOREIGN KEY([Product_id])
REFERENCES [dbo].[Products] ([Product_id])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_Products]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_Users] FOREIGN KEY([User_id])
REFERENCES [dbo].[Users] ([User_id])
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_Users]
GO
ALTER TABLE [dbo].[Coupons]  WITH CHECK ADD  CONSTRAINT [FK_Coupons_Products] FOREIGN KEY([Product_id])
REFERENCES [dbo].[Products] ([Product_id])
GO
ALTER TABLE [dbo].[Coupons] CHECK CONSTRAINT [FK_Coupons_Products]
GO
ALTER TABLE [dbo].[Coupons]  WITH CHECK ADD  CONSTRAINT [FK_Coupons_Users] FOREIGN KEY([User_id])
REFERENCES [dbo].[Users] ([User_id])
GO
ALTER TABLE [dbo].[Coupons] CHECK CONSTRAINT [FK_Coupons_Users]
GO
ALTER TABLE [dbo].[Messaging]  WITH CHECK ADD  CONSTRAINT [FK_Messaging_Receiver] FOREIGN KEY([Recipient_Id])
REFERENCES [dbo].[Users] ([User_id])
GO
ALTER TABLE [dbo].[Messaging] CHECK CONSTRAINT [FK_Messaging_Receiver]
GO
ALTER TABLE [dbo].[Messaging]  WITH CHECK ADD  CONSTRAINT [FK_Messaging_Sender] FOREIGN KEY([Sender_Id])
REFERENCES [dbo].[Users] ([User_id])
GO
ALTER TABLE [dbo].[Messaging] CHECK CONSTRAINT [FK_Messaging_Sender]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY([Order_id])
REFERENCES [dbo].[Orders] ([Order_id])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Orders]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Products] FOREIGN KEY([Product_id])
REFERENCES [dbo].[Products] ([Product_id])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Products]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([User_id])
REFERENCES [dbo].[Users] ([User_id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([Category_id])
REFERENCES [dbo].[Categories] ([Category_id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Users] FOREIGN KEY([User_id])
REFERENCES [dbo].[Users] ([User_id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Rol_id] FOREIGN KEY([Rol_id])
REFERENCES [dbo].[Roles] ([id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Rol_id]
GO
USE [master]
GO
ALTER DATABASE [B2BDb] SET  READ_WRITE 
GO
