USE [master]
GO
/****** Object:  Database [FaceDB]    Script Date: 04/11/2019 09:09:27 ******/
CREATE DATABASE [FaceDB] ON  PRIMARY 
( NAME = N'FaceDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\FaceDB.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FaceDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\FaceDB_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [FaceDB] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [FaceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [FaceDB] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [FaceDB] SET ANSI_NULLS OFF
GO
ALTER DATABASE [FaceDB] SET ANSI_PADDING OFF
GO
ALTER DATABASE [FaceDB] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [FaceDB] SET ARITHABORT OFF
GO
ALTER DATABASE [FaceDB] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [FaceDB] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [FaceDB] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [FaceDB] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [FaceDB] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [FaceDB] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [FaceDB] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [FaceDB] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [FaceDB] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [FaceDB] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [FaceDB] SET  DISABLE_BROKER
GO
ALTER DATABASE [FaceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [FaceDB] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [FaceDB] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [FaceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [FaceDB] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [FaceDB] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [FaceDB] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [FaceDB] SET  READ_WRITE
GO
ALTER DATABASE [FaceDB] SET RECOVERY FULL
GO
ALTER DATABASE [FaceDB] SET  MULTI_USER
GO
ALTER DATABASE [FaceDB] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [FaceDB] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'FaceDB', N'ON'
GO
USE [FaceDB]
GO
/****** Object:  Table [dbo].[Face_UserInfo]    Script Date: 04/11/2019 09:09:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Face_UserInfo](
	[NumberId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NULL,
	[Month] [nvarchar](50) NULL,
	[Sex] [nvarchar](50) NULL,
	[Works] [nvarchar](50) NULL,
	[face_token] [nvarchar](50) NULL,
	[Guid_Id] [nvarchar](50) NULL,
 CONSTRAINT [PK_Face_UserInfo] PRIMARY KEY CLUSTERED 
(
	[NumberId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键自增' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Face_UserInfo', @level2type=N'COLUMN',@level2name=N'NumberId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Face_UserInfo', @level2type=N'COLUMN',@level2name=N'UserName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'出生年月' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Face_UserInfo', @level2type=N'COLUMN',@level2name=N'Month'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Face_UserInfo', @level2type=N'COLUMN',@level2name=N'Sex'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工作/学习单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Face_UserInfo', @level2type=N'COLUMN',@level2name=N'Works'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人脸唯一标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Face_UserInfo', @level2type=N'COLUMN',@level2name=N'face_token'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'人脸和数据库表关联字段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Face_UserInfo', @level2type=N'COLUMN',@level2name=N'Guid_Id'
GO
