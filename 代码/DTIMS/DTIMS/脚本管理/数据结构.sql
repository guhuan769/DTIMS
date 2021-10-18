USE [master]
GO
/****** Object:  Database [JFIMS]    Script Date: 2019/1/12 20:18:52 ******/
CREATE DATABASE [JFIMS]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CTQS2', FILENAME = N'D:\PRODUCT\sqlserver2017\MSSQL14.MSSQLSERVER\MSSQL\DATA\JFIMS.mdf' , SIZE = 11584KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'CTQS2_log', FILENAME = N'D:\PRODUCT\sqlserver2017\MSSQL14.MSSQLSERVER\MSSQL\DATA\JFIMS_0.ldf' , SIZE = 3840KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO
ALTER DATABASE [JFIMS] SET COMPATIBILITY_LEVEL = 100
--GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JFIMS].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [JFIMS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JFIMS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JFIMS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JFIMS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JFIMS] SET ARITHABORT OFF 
GO
ALTER DATABASE [JFIMS] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [JFIMS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JFIMS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JFIMS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JFIMS] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JFIMS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JFIMS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JFIMS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JFIMS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JFIMS] SET  DISABLE_BROKER 
GO
ALTER DATABASE [JFIMS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JFIMS] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JFIMS] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JFIMS] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JFIMS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JFIMS] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [JFIMS] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JFIMS] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [JFIMS] SET  MULTI_USER 
GO
ALTER DATABASE [JFIMS] SET PAGE_VERIFY TORN_PAGE_DETECTION  
GO
ALTER DATABASE [JFIMS] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JFIMS] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JFIMS] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [JFIMS] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'JFIMS', N'ON'
GO
ALTER DATABASE [JFIMS] SET QUERY_STORE = OFF
GO
USE [JFIMS]
GO
/****** Object:  User [user]    Script Date: 2019/1/12 20:18:53 ******/
CREATE USER [user] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Default [D_DeviceStatus]    Script Date: 2019/1/12 20:18:53 ******/
CREATE DEFAULT [dbo].[D_DeviceStatus] 
AS
1
GO
/****** Object:  Default [D_PortStatus]    Script Date: 2019/1/12 20:18:53 ******/
CREATE DEFAULT [dbo].[D_PortStatus] 
AS
1
GO
/****** Object:  Default [D_Type_CLTPortSelectMode]    Script Date: 2019/1/12 20:18:53 ******/
CREATE DEFAULT [dbo].[D_Type_CLTPortSelectMode] 
AS
'F'
GO
/****** Object:  Default [D_type_IsUsed]    Script Date: 2019/1/12 20:18:53 ******/
CREATE DEFAULT [dbo].[D_type_IsUsed] 
AS
'Y'
GO
/****** Object:  Rule [R_AutoIdentity]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_AutoIdentity] 
AS
@column is null or (@column >= 1 )
GO
/****** Object:  Rule [R_ChannelType]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_ChannelType] 
AS
@column is null or ( @column in ('Fast','InterLeaved') )
GO
/****** Object:  Rule [R_CheckPortSelectMode]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_CheckPortSelectMode] 
AS
@column is null or ( @column in ('F','L','H') )
GO
/****** Object:  Rule [R_CommandCheck]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_CommandCheck] 
AS
@column is null or ( @column in ('<T>','<F>') )
GO
/****** Object:  Rule [R_DeviceStatus]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_DeviceStatus] 
AS
@column is null or (@column between 1 and 2 )
GO
/****** Object:  Rule [R_PageModuleType]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_PageModuleType] 
AS
@column is null or ( @column in ('0','1') )
GO
/****** Object:  Rule [R_PageShowType]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_PageShowType] 
AS
@column is null or ( @column in ('0','1','2','3') )
GO
/****** Object:  Rule [R_PortStatus]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_PortStatus] 
AS
@column is null or (@column between 1 and 2 )
GO
/****** Object:  Rule [R_Type_CLTPortSelectMode]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_Type_CLTPortSelectMode] 
AS
@column is null or ( @column in ('F','L','A','T','H','P') )
GO
/****** Object:  Rule [R_type_IsUsed]    Script Date: 2019/1/12 20:18:53 ******/
CREATE RULE [dbo].[R_type_IsUsed] 
AS
@column is null or ( @column in ('Y','N') )
GO
/****** Object:  UserDefinedDataType [dbo].[AutoIdentity]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[AutoIdentity] FROM [smallint] NULL
GO
/****** Object:  UserDefinedDataType [dbo].[ChannelType]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[ChannelType] FROM [varchar](20) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[CheckPortSelectMode]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[CheckPortSelectMode] FROM [char](1) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[CommandCheck]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[CommandCheck] FROM [char](3) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[DeviceStatus]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[DeviceStatus] FROM [smallint] NULL
GO
/****** Object:  UserDefinedDataType [dbo].[PageModuleType]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[PageModuleType] FROM [char](1) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[PageShowType]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[PageShowType] FROM [char](1) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[PortStatus]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[PortStatus] FROM [smallint] NULL
GO
/****** Object:  UserDefinedDataType [dbo].[Type_CLTPortSelectMode]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[Type_CLTPortSelectMode] FROM [char](1) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[type_IsUsed]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[type_IsUsed] FROM [char](1) NULL
GO
/****** Object:  UserDefinedDataType [dbo].[Type_Remark]    Script Date: 2019/1/12 20:18:53 ******/
CREATE TYPE [dbo].[Type_Remark] FROM [nvarchar](50) NULL
GO
/****** Object:  UserDefinedFunction [dbo].[ConstructTableFromSplitString]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tymfr
-- Create date: 2009-11-19
-- Description:	把一逗号分隔的数据返回为一张表
-- =============================================
CREATE FUNCTION [dbo].[ConstructTableFromSplitString] 
(
	-- Add the parameters for the function here
	@SplitString Varchar(8000)
)
RETURNS 
 @ValuesTable TABLE 
(
	-- Add the column definitions for the TABLE variable here
	[Value] int
)
AS
BEGIN
	-- Fill the table variable with the rows for your result set
	--正对CTQS做特殊处理
	IF (@SplitString is null) OR (@SplitString ='')
	BEGIN
		INSERT INTO @ValuesTable SELECT ROLE_ID FROM S_Role;
		RETURN;
	END
	--处理后的字符串
    DECLARE @SingleValue varchar(8000);  
    --截取位置
    DECLARE @Index INT ;               
    SET @Index = 0;
    WHILE CHARINDEX(',',@SplitString) > 0
    BEGIN
		SELECT @Index = CHARINDEX(',',@SplitString);
		SELECT @SingleValue = LEFT(@SplitString,@Index-1);
		--插入数据
		INSERT INTO @ValuesTable VALUES(@SingleValue); 
		--截取已经处理好的数据
		SELECT @SplitString = SUBSTRING(@SplitString,@Index+1,LEN(@SplitString));
    END
    --仅有一个值，如（1）
    IF @SplitString <> ''
	INSERT INTO @ValuesTable VALUES(@SplitString);
	RETURN;
END
GO
/****** Object:  Table [dbo].[IPConifg]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IPConifg](
	[id] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[ip] [varchar](50) NULL,
	[prot] [varchar](20) NULL,
	[state] [int] NULL,
	[KM] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[S_LogFile]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[S_LogFile](
	[LOG_ID] [numeric](10, 0) IDENTITY(1,1) NOT NULL,
	[LItem_ID] [char](2) NOT NULL,
	[User_ID] [numeric](4, 0) NULL,
	[LOG_DATETIME] [datetime] NOT NULL,
	[LOG_Mode] [char](1) NOT NULL,
	[LOG_CONTENT] [varchar](520) NULL,
	[LOG_Client_IP] [varchar](30) NULL,
 CONSTRAINT [PK_S_LOGFILE] PRIMARY KEY CLUSTERED 
(
	[LOG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[S_LogItem]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[S_LogItem](
	[LItem_ID] [char](2) NOT NULL,
	[LItem_Name] [varchar](20) NOT NULL,
 CONSTRAINT [PK_S_LOGITEM] PRIMARY KEY CLUSTERED 
(
	[LItem_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[S_MainArea]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[S_MainArea](
	[MainArea_ID] [int] NOT NULL,
	[FatherMainArea_ID] [int] NOT NULL,
	[MainArea_Name] [varchar](30) NOT NULL,
	[MainArea_Remark] [varchar](100) NULL,
 CONSTRAINT [PK_S_MAINAREA] PRIMARY KEY CLUSTERED 
(
	[MainArea_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_IDENTIFIER_3_S_MAINAR] UNIQUE NONCLUSTERED 
(
	[MainArea_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_FunctionCategory]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_FunctionCategory](
	[FunCategory_ID] [int] NOT NULL,
	[FunCategory_Name] [varchar](100) NULL,
 CONSTRAINT [PK_SYS_FUNCTIONCATEGORY] PRIMARY KEY CLUSTERED 
(
	[FunCategory_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_FunctionItem]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_FunctionItem](
	[Fun_ID] [varchar](4) NOT NULL,
	[FunCategory_ID] [int] NULL,
	[Fun_ParentID] [varchar](4) NULL,
	[Fun_Name] [varchar](100) NOT NULL,
	[Fun_Desc] [varchar](100) NULL,
	[Fun_Sort] [int] NULL,
	[Fun_Url] [varchar](100) NULL,
	[Fun_Image] [varchar](100) NULL,
	[Fun_CssClass] [varchar](100) NULL,
 CONSTRAINT [PK_SYS_FUNCTIONITEM] PRIMARY KEY CLUSTERED 
(
	[Fun_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_PrivilegeFunctionMap]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_PrivilegeFunctionMap](
	[Fun_ID] [varchar](4) NOT NULL,
	[PrivGroup_ID] [numeric](8, 0) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_PrivilegeGroup]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_PrivilegeGroup](
	[PrivGroup_ID] [numeric](8, 0) IDENTITY(1,1) NOT NULL,
	[User_ID] [numeric](4, 0) NULL,
	[PrivGroup_Name] [varchar](20) NOT NULL,
	[PrivGroup_Desc] [varchar](50) NULL,
	[PrivGroup_IsPrivate] [int] NOT NULL,
 CONSTRAINT [PK_SYS_PRIVILEGEGROUP] PRIMARY KEY CLUSTERED 
(
	[PrivGroup_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_User]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_User](
	[User_ID] [numeric](4, 0) IDENTITY(1,1) NOT NULL,
	[UserRole_ID] [int] NULL,
	[MainArea_ID] [int] NULL,
	[User_Name] [varchar](20) NULL,
	[User_Login] [varchar](20) NOT NULL,
	[User_Password] [varchar](200) NOT NULL,
	[User_CreateDate] [datetime] NOT NULL,
	[User_Status] [int] NULL,
	[User_Remark] [varchar](50) NULL,
 CONSTRAINT [PK_SYS_USER] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_IDENTIFIER_2_SYS_USER] UNIQUE NONCLUSTERED 
(
	[User_Login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_UserPrivilegeMap]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_UserPrivilegeMap](
	[PrivGroup_ID] [numeric](8, 0) NOT NULL,
	[User_ID] [numeric](4, 0) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sys_UserRole]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sys_UserRole](
	[UserRole_ID] [int] NOT NULL,
	[UserRole_Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SYS_USERROLE] PRIMARY KEY CLUSTERED 
(
	[UserRole_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_PassLog]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_PassLog](
	[ID] [decimal](18, 0) IDENTITY(1,1) NOT NULL,
	[SFZMHM] [varchar](18) NOT NULL,
	[PASSTIME] [datetime] NOT NULL,
	[REASON] [nvarchar](100) NULL,
	[REASONID] [char](1) NULL,
	[PASS] [char](1) NOT NULL,
	[KSKM] [varchar](50) NOT NULL,
 CONSTRAINT [PK_T_PassLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[T_Student]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Student](
	[ID] [numeric](9, 0) IDENTITY(1,1) NOT NULL,
	[SFZMHM] [nvarchar](18) NOT NULL,
	[NAME] [nvarchar](18) NULL,
	[TESTDATE] [datetime] NOT NULL,
	[LSH] [varchar](13) NULL,
	[KSKM] [char](1) NULL,
	[JFBJ] [char](1) NOT NULL,
	[KSCS] [char](1) NULL,
	[KZCS] [int] NOT NULL,
	[PASS] [char](1) NULL,
 CONSTRAINT [PK_T_Student] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [Relationship_11_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_11_FK] ON [dbo].[S_LogFile]
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Relationship_47_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_47_FK] ON [dbo].[S_LogFile]
(
	[LItem_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Relationship_64_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_64_FK] ON [dbo].[Sys_FunctionItem]
(
	[FunCategory_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [Relationship_62_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_62_FK] ON [dbo].[Sys_PrivilegeFunctionMap]
(
	[Fun_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Relationship_63_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_63_FK] ON [dbo].[Sys_PrivilegeFunctionMap]
(
	[PrivGroup_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Relationship_16_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_16_FK] ON [dbo].[Sys_PrivilegeGroup]
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Relationship_10_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_10_FK] ON [dbo].[Sys_User]
(
	[MainArea_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Relationship_15_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_15_FK] ON [dbo].[Sys_User]
(
	[UserRole_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Relationship_60_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_60_FK] ON [dbo].[Sys_UserPrivilegeMap]
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [Relationship_61_FK]    Script Date: 2019/1/12 20:18:53 ******/
CREATE NONCLUSTERED INDEX [Relationship_61_FK] ON [dbo].[Sys_UserPrivilegeMap]
(
	[PrivGroup_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IPConifg] ADD  CONSTRAINT [DF_IPConifg_state]  DEFAULT ((1)) FOR [state]
GO
ALTER TABLE [dbo].[Sys_PrivilegeGroup] ADD  DEFAULT (1) FOR [PrivGroup_IsPrivate]
GO
ALTER TABLE [dbo].[Sys_User] ADD  DEFAULT (0) FOR [User_Status]
GO
ALTER TABLE [dbo].[T_Student] ADD  CONSTRAINT [DF_T_Student_KZCS]  DEFAULT ((0)) FOR [KZCS]
GO
ALTER TABLE [dbo].[T_Student] ADD  CONSTRAINT [DF_T_Student_PASS]  DEFAULT ((0)) FOR [PASS]
GO
ALTER TABLE [dbo].[S_LogFile]  WITH CHECK ADD  CONSTRAINT [FK_S_LOGFIL_RELATIONS_S_LOGITE] FOREIGN KEY([LItem_ID])
REFERENCES [dbo].[S_LogItem] ([LItem_ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[S_LogFile] CHECK CONSTRAINT [FK_S_LOGFIL_RELATIONS_S_LOGITE]
GO
ALTER TABLE [dbo].[S_LogFile]  WITH CHECK ADD  CONSTRAINT [FK_S_LOGFIL_RELATIONS_SYS_USER] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Sys_User] ([User_ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[S_LogFile] CHECK CONSTRAINT [FK_S_LOGFIL_RELATIONS_SYS_USER]
GO
ALTER TABLE [dbo].[Sys_FunctionItem]  WITH CHECK ADD  CONSTRAINT [FK_SYS_FUNC_RELATIONS_SYS_FUNC] FOREIGN KEY([FunCategory_ID])
REFERENCES [dbo].[Sys_FunctionCategory] ([FunCategory_ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Sys_FunctionItem] CHECK CONSTRAINT [FK_SYS_FUNC_RELATIONS_SYS_FUNC]
GO
ALTER TABLE [dbo].[Sys_PrivilegeFunctionMap]  WITH CHECK ADD  CONSTRAINT [FK_SYS_PRIV_RELATIONS_SYS_FUNC] FOREIGN KEY([Fun_ID])
REFERENCES [dbo].[Sys_FunctionItem] ([Fun_ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Sys_PrivilegeFunctionMap] CHECK CONSTRAINT [FK_SYS_PRIV_RELATIONS_SYS_FUNC]
GO
ALTER TABLE [dbo].[Sys_PrivilegeFunctionMap]  WITH CHECK ADD  CONSTRAINT [FK_SYS_PRIV_RELATIONS_SYS_PRIV] FOREIGN KEY([PrivGroup_ID])
REFERENCES [dbo].[Sys_PrivilegeGroup] ([PrivGroup_ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Sys_PrivilegeFunctionMap] CHECK CONSTRAINT [FK_SYS_PRIV_RELATIONS_SYS_PRIV]
GO
ALTER TABLE [dbo].[Sys_PrivilegeGroup]  WITH CHECK ADD  CONSTRAINT [FK_SYS_PRIV_RELATIONS_SYS_USER] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Sys_User] ([User_ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Sys_PrivilegeGroup] CHECK CONSTRAINT [FK_SYS_PRIV_RELATIONS_SYS_USER]
GO
ALTER TABLE [dbo].[Sys_User]  WITH CHECK ADD  CONSTRAINT [FK_SYS_USER_RELATIONS_S_MAINAR] FOREIGN KEY([MainArea_ID])
REFERENCES [dbo].[S_MainArea] ([MainArea_ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Sys_User] CHECK CONSTRAINT [FK_SYS_USER_RELATIONS_S_MAINAR]
GO
ALTER TABLE [dbo].[Sys_User]  WITH CHECK ADD  CONSTRAINT [FK_SYS_USER_RELATIONS_SYS_USER_1] FOREIGN KEY([UserRole_ID])
REFERENCES [dbo].[Sys_UserRole] ([UserRole_ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Sys_User] CHECK CONSTRAINT [FK_SYS_USER_RELATIONS_SYS_USER_1]
GO
ALTER TABLE [dbo].[Sys_UserPrivilegeMap]  WITH CHECK ADD  CONSTRAINT [FK_SYS_USER_RELATIONS_SYS_USER] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Sys_User] ([User_ID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Sys_UserPrivilegeMap] CHECK CONSTRAINT [FK_SYS_USER_RELATIONS_SYS_USER]
GO
ALTER TABLE [dbo].[S_LogFile]  WITH CHECK ADD  CONSTRAINT [CKC_LOG_MODE_S_LOGFIL] CHECK  (([LOG_Mode] = 'U' or ([LOG_Mode] = 'D' or [LOG_Mode] = 'A')))
GO
ALTER TABLE [dbo].[S_LogFile] CHECK CONSTRAINT [CKC_LOG_MODE_S_LOGFIL]
GO
/****** Object:  StoredProcedure [dbo].[ElementUsageStatistics]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Tymfr
-- Create date: 2009-11-19
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[ElementUsageStatistics] 
	-- Add the parameters for the stored procedure here
	@StartTime Datetime, 
	@EndTime Datetime,
	@AreaList varchar(8000) ---以逗号分隔的权限组ID
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	---@EUS Element Usage Statistics Table
	---存储统计数据
	CREATE TABLE #EUS
	(
		Project varchar(1000),---项目 AAA HLR AN-AAA
		FunctionType varchar(100),---功能项 查询 修改 开户 等等
		Area_Id int, ---地区ID
		Ammount int ---数量
	);
	
	--存储权限组信息
	Declare @AreaIds Table
	(
		AreaID int
	)
	
	INSERT INTO @AreaIds SELECT * FROM dbo.ConstructTableFromSplitString(@AreaList)
	
	---分别进行统计
	---统计AAA的数据
	INSERT INTO #EUS(Project,FunctionType,Area_Id,Ammount)
	SELECT 'AAA' AS Project, AAA_OperationTypeName,MainArea_ID,COUNT(AAA_LogInfoID) AS Ammount FROM
	AAA_OperationLogInfo INNER JOIN AAA_OperationType on AAA_OperationLogInfo.AAA_OperationTypeID = AAA_OperationType.AAA_OperationTypeID
	INNER JOIN Sys_User ON AAA_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( AAA_LogOperationTime>=@StartTime AND AAA_LogOperationTime<=@EndTime ) AND Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID,AAA_OperationTypeName
	---统计AN-AAA数据
	UNION ALL
	SELECT 'AN-AAA' AS Project, ANAAA_OperationTypeName,MainArea_ID,COUNT(ANAAA_LogInfoID) AS Ammount FROM
	ANAAA_OperationLogInfo INNER JOIN ANAAA_OperationType on ANAAA_OperationLogInfo.ANAAA_OperationTypeID = ANAAA_OperationType.ANAAA_OperationTypeID
	INNER JOIN Sys_User ON ANAAA_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( ANAAA_LogOperationTime>=@StartTime AND ANAAA_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID,ANAAA_OperationTypeName
	---统计HLR数据
	UNION ALL
	SELECT 'HLR' AS Project,HLR_OperationTypeName ,MainArea_ID,COUNT(HLR_LogInfoID) AS Ammount FROM
	HLR_OperationLogInfo INNER JOIN HLR_OperationType on HLR_OperationLogInfo.HLR_OperationTypeID = HLR_OperationType.HLR_OperationTypeID
	INNER JOIN Sys_User ON HLR_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( HLR_OperationTime>=@StartTime AND HLR_OperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID,HLR_OperationTypeName
	---统计SMS数据
	UNION ALL
	SELECT 'SMS' AS Project,SMS_OperationTypeName ,MainArea_ID,COUNT(SMS_LogInfoID) AS Ammount FROM
	SMS_OperationLogInfo INNER JOIN SMS_OperationType on SMS_OperationLogInfo.SMS_OperationTypeID = SMS_OperationType.SMS_OperationTypeID
	INNER JOIN Sys_User ON SMS_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( SMS_LogOperationTime>=@StartTime AND SMS_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID,SMS_OperationTypeName
	---统计WAP数据
	UNION ALL
	SELECT 'WAP' AS Project,WAP_OperationTypeName ,MainArea_ID,COUNT(WAP_LogInfoID) AS Ammount FROM
	WAP_OperationLogInfo INNER JOIN WAP_OperationType on WAP_OperationLogInfo.WAP_OperationTypeID = WAP_OperationType.WAP_OperationTypeID
	INNER JOIN Sys_User ON WAP_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( WAP_LogOperationTime>=@StartTime AND WAP_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID,WAP_OperationTypeName
	---统计IVPN数据
	UNION ALL
	SELECT 'IVPN' AS Project,IVPN_OperationTypeName ,MainArea_ID,COUNT(IVPN_LogInfoID) AS Ammount FROM
	IVPN_OperationLogInfo INNER JOIN IVPN_OperationType on IVPN_OperationType.IVPN_OperationTypeID = IVPN_OperationLogInfo.IVPN_OperationTypeID
	INNER JOIN Sys_User ON IVPN_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( IVPN_LogOperationTime>=@StartTime AND IVPN_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID,IVPN_OperationTypeName
	---统计CRM数据
	UNION ALL
	SELECT 'CRM' AS Project,'查询' ,MainArea_ID,COUNT(CRM_LogInfoID) AS Ammount FROM
	CRM_OperationLogInfo INNER JOIN Sys_User ON CRM_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( CRM_LogOperationTime>=@StartTime AND CRM_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID
	---统计IAD数据
	UNION ALL
	SELECT 'IAD' AS Project,IAD_OperationTypeName ,MainArea_ID,COUNT(IAD_LogInfoID) AS Ammount FROM
	IAD_OperationLogInfo INNER JOIN IAD_OperationType on IAD_OperationType.IAD_OperationTypeID = IAD_OperationLogInfo.IAD_OperationTypeID
	INNER JOIN Sys_User ON IAD_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( IAD_LogOperationTime>=@StartTime AND IAD_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID,IAD_OperationTypeName
	---统计Gota数据
	UNION ALL
	SELECT 'GOTA' AS Project,Gota_OperationTypeName ,MainArea_ID,COUNT(Gota_LogInfoID) AS Ammount FROM
	Gota_OperationLogInfo INNER JOIN Gota_OperationType on Gota_OperationType.Gota_OperationTypeID = Gota_OperationLogInfo.Gota_OperationTypeID
	INNER JOIN Sys_User ON Gota_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( Gota_LogOperationTime>=@StartTime AND Gota_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID,Gota_OperationTypeName
	---统计10001数据
	UNION ALL
	SELECT '10001' AS Project,'短信发送' ,MainArea_ID,COUNT(SendSMS_LogInfoID) AS Ammount FROM
	SendSMS_OperationLogInfo 
	INNER JOIN Sys_User ON SendSMS_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( SendSMS_LogOperationTime>=@StartTime AND SendSMS_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID
	---统计彩信数据
	UNION ALL
	SELECT 'MMS' AS Project,'查询' ,MainArea_ID,COUNT(MMS_LogInfoID) AS Ammount FROM
	MMS_OperationLogInfo 
	INNER JOIN Sys_User ON MMS_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( MMS_LogOperationTime>=@StartTime AND MMS_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID
	---手机终端数据
	UNION ALL
	SELECT 'MB' AS Project,'查询' ,MainArea_ID,COUNT(MB_LogInfoID) AS Ammount FROM
	MB_OperationLogInfo 
	INNER JOIN Sys_User ON MB_OperationLogInfo.User_ID = Sys_User.User_ID 
	WHERE ( MB_LogOperationTime>=@StartTime AND MB_LogOperationTime<=@EndTime ) AND  Sys_User.User_ID IN(select User_ID from Sys_User where MainArea_ID in (select * from @AreaIds)) GROUP BY Sys_User.MainArea_ID
	
	--select *From #EUS;
	SELECT Project,FunctionType,Area_Id,Ammount INTO #TMP FROM #EUS --INNER JOIN S_Role ON #EUS.Role_Id = S_Role.Role_Id order by ROLE_NAME

	if((SELECT COUNT(*) FROM #EUS)>0)
	BEGIN
		INSERT INTO #TMP
		SELECT Project,FunctionType,'-1',sum(Ammount) FROM #EUS GROUP BY Project,FunctionType
	END
	
	------返回统计数据
	SELECT * FROM #TMP
	
END

GO
/****** Object:  StoredProcedure [dbo].[proc_GetRecordFromPage]    Script Date: 2019/1/12 20:18:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[proc_GetRecordFromPage]
    @tblName      varchar(2000),         --TABLE名称
    @primarykey   varchar(255),         --主健列，多处主健列用“，”号分隔
    @PageSize     int = 10,            	--一页的数据条数
    @PageIndex    int = 1,              --当前页号
    @fileds       varchar(1500) = '*',   --需要查询的列，多列用","号分隔
    @strWhere     varchar(2500) = '',	--不包含where
    @order	  varchar(1500)  = '',	--不包含order
    @dataCount    int = 0 output,	--总数据量
    @curPageIndex int = 1 output	--当前页号
AS  
begin 
declare @i int,@IdStr nvarchar(4000),@index int,@t int,@filed varchar(50)
/*
分页存储过程
*/
	declare @strSQL nvarchar(4000),@topnum int ,@previous int

	if @strWhere <> '' 
		select @strWhere = 'where ' + @strWhere
	if @order <> ''
		select @order= 'order by ' + @order
	else 
	begin 
		select @order= 'order by ' + @primarykey
	end

	--计算数据总量
	select @strSQL = 'select @dataCount=count(1) from ' + @tblName + ' ' + @strWhere
	exec sp_executesql @strSQL,N'@dataCount int output',@dataCount output
	
	if @PageSize = 0 
	begin 
		select @dataCount 
		return 
	end
	--print @dataCount
	
	--计算输入页号是合法，如果大了就整成最大页号算了
	select @curPageIndex = @dataCount/@PageSize 
	if @curPageIndex * @PageSize < @dataCount 
	begin
		select @curPageIndex = @curPageIndex +1
	end
	if @curPageIndex = 0 
	begin 
		select @curPageIndex=1
	end
	if @curPageIndex < @PageIndex
	begin 
		select @PageIndex = @curPageIndex
	end
	select @curPageIndex = @PageIndex
	
	--判断是否是第一页，如果是第一页最简单了，直接返回得了
	--print @PageIndex
	if @PageIndex = 1 
	begin 
		--print '--'
		--print @strWhere
		--print '--'
		select @strSQL = 'select top ' + cast(@PageSize as varchar) + ' ' + @fileds + ' from ' + @tblName + ' ' + @strWhere + ' ' + @order 
		--print @strSQL
		--select @dataCount,@curPageIndex
	end
	else 
	begin
		
		select @i = 0 , @index=1,@t = 0
		select @topnum = @PageIndex * @PageSize
		select @previous = (@PageIndex - 1) * @PageSize

		select @strSQL = N''
		select @strSQL = @strSQL + N' select top '+str(@topnum)+ ' @i = @i + 1 '
		select @strSQL = @strSQL + N',  @IdStr = '
		select @strSQL = @strSQL + N'case when @i > '+str(@previous)+' then  @IdStr + ''(' 

		select @index = charindex(',' , @primarykey , 0)
		if @index = 0 
		begin
			if exists(select 1 from sysobjects join syscolumns on sysobjects.id = syscolumns.id
join systypes on syscolumns.xusertype = systypes.xusertype  where sysobjects.name = @tblName and syscolumns.name =@primarykey and systypes.name like '%datetime%')
			begin
				select @strSQL = @strSQL + @primarykey + '=''''''+ltrim(rtrim(convert(varchar,'+@primarykey+',121)))+'''''') or '''
			end
			else
			begin 
				select @strSQL = @strSQL + @primarykey + '=''''''+ltrim(rtrim(cast('+@primarykey+'  as varchar(256))))+'''''') or '''
			end		
		end
		else 
		begin
			while(@index >= 0)
			begin
				if @index = 0
				begin
					select @filed = ltrim(rtrim(substring(@primarykey , @t , len(@primarykey))))
					select @t = @index
					select @index = -1
				end
				else 
				begin

					--print substring(@primarykey , @t , @index - @t)
					select @filed = ltrim(rtrim(substring(@primarykey , @t , @index - @t)))
					select @t = @index + 1
					select @index = charindex(',' , @primarykey , @t)
				end
				if exists(select 1 from sysobjects join syscolumns on sysobjects.id = syscolumns.id
join systypes on syscolumns.xusertype = systypes.xusertype where sysobjects.name = @tblName and syscolumns.name =@primarykey and systypes.name like '%datetime%')
				begin
					select @strSQL = @strSQL + @filed + '=''''''+ltrim(rtrim(convert(varchar,'+@filed+',121)))+'''''' and '				
				end
				else
				begin 
					select @strSQL = @strSQL + @filed + '=''''''+ltrim(rtrim(cast('+@filed+'  as varchar(256))))+'''''' and '
				end	
			end
			select @strSQL = left(@strSQL,len(@strSQL)-5) + ''') or '''		
		end
		select @strSQL = @strSQL + N' else N''''end '
		select @strSQL = @strSQL + N'from ' + @tblName 
		select @strSQL = ltrim(rtrim(@strSQL)) + N' ' + @strWhere + ' ' + @order 
		Select @IdStr = N''

		exec sp_executesql @strSQL,N'@i int,@IdStr nvarchar(4000) output',@i,@IdStr output
		--print @IdStr
		if len(rtrim(ltrim(@IdStr))) > 0
		begin
		 select @IdStr = left(@IdStr,len(@IdStr)-3)
		end
		--print @IdStr
		
		select @strSQL = 'select ' + @fileds + ' from ' + @tblName + ' where ' + @IDStr + ' ' + @order 
		--select @dataCount,@curPageIndex
	end
	
	print @strSQL
	exec(@strSQL)
end
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PassLog', @level2type=N'COLUMN',@level2name=N'SFZMHM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'验证时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PassLog', @level2type=N'COLUMN',@level2name=N'PASSTIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原因（验证失败记录）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PassLog', @level2type=N'COLUMN',@level2name=N'REASON'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原因编号：1.无考生信息，2.未交费，3.黑名单，4.人脸验证失败' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PassLog', @level2type=N'COLUMN',@level2name=N'REASONID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0.通过，1.不通过' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_PassLog', @level2type=N'COLUMN',@level2name=N'PASS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID,自增长' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Student', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身份证号码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Student', @level2type=N'COLUMN',@level2name=N'SFZMHM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Student', @level2type=N'COLUMN',@level2name=N'NAME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'考试日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Student', @level2type=N'COLUMN',@level2name=N'TESTDATE'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'流水号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Student', @level2type=N'COLUMN',@level2name=N'LSH'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'考试科目' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Student', @level2type=N'COLUMN',@level2name=N'KSKM'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缴费标记' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Student', @level2type=N'COLUMN',@level2name=N'JFBJ'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'考试次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Student', @level2type=N'COLUMN',@level2name=N'KSCS'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'开闸次数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'T_Student', @level2type=N'COLUMN',@level2name=N'KZCS'
GO
USE [master]
GO
ALTER DATABASE [JFIMS] SET  READ_WRITE 
GO
