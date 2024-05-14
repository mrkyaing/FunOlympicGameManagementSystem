create database FOGMS;

USE [FOGMS]
GO


/****** Object:  Table [dbo].[Users]    Script Date: 07/02/2023 21:52:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
  [Id] [nvarchar](450) NOT NULL,
  [UserName] [nvarchar](max) NOT NULL,
  [Email] [nvarchar](max) NOT NULL,
  [Password] [nvarchar](max) NOT NULL,
  [DOB] [datetime2](7) NOT NULL,
  [Gender] [nvarchar](max) NOT NULL,
  [Country] [nvarchar](max) NULL,
  [Address] [nvarchar](max) NOT NULL,
  [IsEmailVerification] [bit] NOT NULL,
  [CreatedOn] [datetime2](7) NOT NULL,
  [IsActive] [bit] NOT NULL,
  [Role] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Role]
GO

USE [FOGMS]
GO
/****** Object:  Table [dbo].[OTP]    Script Date: 07/02/2023 21:52:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OTP](
  [Id] [nvarchar](450) NOT NULL,
  [EmailId] [nvarchar](max) NOT NULL,
  [OTP] [nvarchar](max) NOT NULL,
  [CreatedOn] [datetime2](7) NOT NULL,
  [IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_OTP] PRIMARY KEY CLUSTERED 
(
  [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--to update role from user to admin,system admin
update users set Role='Admin' where id='b41e519a-91d7-4dba-82df-ef2d9509675f'