
UPDATE dbo.Sys_FunctionItem 
SET Fun_Name = '学员管理',Fun_Desc = '学员管理'
WHERE  Fun_Name = '黑白名单管理'
GO
UPDATE dbo.Sys_FunctionItem 
SET Fun_Name = '学员查询',Fun_Desc = '学员查询'
WHERE  Fun_Name = '黑白名单查询'
GO
UPDATE dbo.Sys_FunctionItem 
SET Fun_Name = '学员日志',Fun_Desc = '学员日志'
WHERE  Fun_Name = '黑白名单日志'
GO
INSERT [dbo].[S_LogItem] ([LItem_ID], [LItem_Name]) VALUES (N'1 ', N'操作员管理')
INSERT [dbo].[S_LogItem] ([LItem_ID], [LItem_Name]) VALUES (N'2 ', N'权限管理')
INSERT [dbo].[S_LogItem] ([LItem_ID], [LItem_Name]) VALUES (N'3 ', N'地区管理')
INSERT [dbo].[S_LogItem] ([LItem_ID], [LItem_Name]) VALUES (N'6 ', N'系统日志管理')
INSERT [dbo].[S_MainArea] ([MainArea_ID], [FatherMainArea_ID], [MainArea_Name], [MainArea_Remark]) VALUES (1, 0, N'地区管理', N'地区管理')
INSERT [dbo].[Sys_UserRole] ([UserRole_ID], [UserRole_Name]) VALUES (1, N'超级管理员')
INSERT [dbo].[Sys_UserRole] ([UserRole_ID], [UserRole_Name]) VALUES (2, N'系统管理员')
INSERT [dbo].[Sys_UserRole] ([UserRole_ID], [UserRole_Name]) VALUES (3, N'普通用户')
SET IDENTITY_INSERT [dbo].[Sys_User] ON 
INSERT [dbo].[Sys_User] ([User_ID], [UserRole_ID], [MainArea_ID], [User_Name], [User_Login], [User_Password], [User_CreateDate], [User_Status], [User_Remark]) VALUES (CAST(1 AS Numeric(4, 0)), 1, 1, N'super', N'super', N'oYoapTfTnoM=', CAST(N'2009-03-29T00:00:00.000' AS DateTime), 0, N'abc')
SET IDENTITY_INSERT [dbo].[Sys_User] OFF
INSERT [dbo].[Sys_FunctionCategory] ([FunCategory_ID], [FunCategory_Name]) VALUES (1, N'功能大类')
INSERT [dbo].[Sys_FunctionCategory] ([FunCategory_ID], [FunCategory_Name]) VALUES (2, N'功能项')
INSERT [dbo].[Sys_FunctionCategory] ([FunCategory_ID], [FunCategory_Name]) VALUES (3, N'子功能')
INSERT [dbo].[Sys_FunctionItem] ([Fun_ID], [FunCategory_ID], [Fun_ParentID], [Fun_Name], [Fun_Desc], [Fun_Sort], [Fun_Url], [Fun_Image], [Fun_CssClass]) VALUES (N'1', 1, NULL, N'系统管理', N'系统管理', 9999, N'', N'', N'theme')
INSERT [dbo].[Sys_FunctionItem] ([Fun_ID], [FunCategory_ID], [Fun_ParentID], [Fun_Name], [Fun_Desc], [Fun_Sort], [Fun_Url], [Fun_Image], [Fun_CssClass]) VALUES (N'11', 2, N'1', N'用户管理', N'用户管理', 11, N'../../sys/UserManage/UserManage.aspx', N'', N'theme')
INSERT [dbo].[Sys_FunctionItem] ([Fun_ID], [FunCategory_ID], [Fun_ParentID], [Fun_Name], [Fun_Desc], [Fun_Sort], [Fun_Url], [Fun_Image], [Fun_CssClass]) VALUES (N'12', 2, N'1', N'修改密码', N'修改密码', 12, N'../../sys/UserManage/changepassword.aspx', N'', N'theme')
INSERT [dbo].[Sys_FunctionItem] ([Fun_ID], [FunCategory_ID], [Fun_ParentID], [Fun_Name], [Fun_Desc], [Fun_Sort], [Fun_Url], [Fun_Image], [Fun_CssClass]) VALUES (N'13', 2, N'1', N'权限组管理', N'权限组管理', 13, N'../../sys/PrivilegeManage/PrivilegeManage.aspx', N'', N'theme')
INSERT [dbo].[Sys_FunctionItem] ([Fun_ID], [FunCategory_ID], [Fun_ParentID], [Fun_Name], [Fun_Desc], [Fun_Sort], [Fun_Url], [Fun_Image], [Fun_CssClass]) VALUES (N'15', 2, N'1', N'系统日志', N'系统日志', 15, N'../../sys/systemlog/systemlog.aspx', N'', N'theme')
INSERT [dbo].[Sys_FunctionItem] ([Fun_ID], [FunCategory_ID], [Fun_ParentID], [Fun_Name], [Fun_Desc], [Fun_Sort], [Fun_Url], [Fun_Image], [Fun_CssClass]) VALUES (N'16', 2, N'2', N'学员查询', N'学员查询', 16, N'../../sys/hbmdMnager/hbmdQuery.aspx', N'', N'theme')
INSERT [dbo].[Sys_FunctionItem] ([Fun_ID], [FunCategory_ID], [Fun_ParentID], [Fun_Name], [Fun_Desc], [Fun_Sort], [Fun_Url], [Fun_Image], [Fun_CssClass]) VALUES (N'17', 2, N'2', N'学员日志', N'学员日志', 17, N'../../sys/hbmdMnager/hbmdLog.aspx', N'', N'theme')
INSERT [dbo].[Sys_FunctionItem] ([Fun_ID], [FunCategory_ID], [Fun_ParentID], [Fun_Name], [Fun_Desc], [Fun_Sort], [Fun_Url], [Fun_Image], [Fun_CssClass]) VALUES (N'19', 2, N'2', N'IP管理', N'IP管理', 19, N'../../sys/IPMnager/IpMnager.aspx', N'', N'theme')
INSERT [dbo].[Sys_FunctionItem] ([Fun_ID], [FunCategory_ID], [Fun_ParentID], [Fun_Name], [Fun_Desc], [Fun_Sort], [Fun_Url], [Fun_Image], [Fun_CssClass]) VALUES (N'2', 1, NULL, N'学员管理', N'学员管理', 9990, N'', N'', N'theme')
SET IDENTITY_INSERT [dbo].[Sys_PrivilegeGroup] ON 
INSERT [dbo].[Sys_PrivilegeGroup] ([PrivGroup_ID], [User_ID], [PrivGroup_Name], [PrivGroup_Desc], [PrivGroup_IsPrivate]) VALUES (CAST(872 AS Numeric(8, 0)), CAST(1 AS Numeric(4, 0)), N'网络部', N'网络部管理员', 1)
SET IDENTITY_INSERT [dbo].[Sys_PrivilegeGroup] OFF
INSERT [dbo].[Sys_PrivilegeFunctionMap] ([Fun_ID], [PrivGroup_ID]) VALUES (N'1', CAST(872 AS Numeric(8, 0)))
SET IDENTITY_INSERT [dbo].[IPConifg] ON 
INSERT [dbo].[IPConifg] ([id], [ip], [prot], [state], [KM]) VALUES (CAST(36 AS Numeric(18, 0)), N'192.168.1.12', N'888', 1, N'1')
SET IDENTITY_INSERT [dbo].[IPConifg] OFF
GO

