USE master
GO
DROP DATABASE IF EXISTS Globomantics
GO

CREATE DATABASE Globomantics
GO 
USE Globomantics
GO 
CREATE TABLE [dbo].[GlobomanticsUser](
	[UserId] [BIGINT] IDENTITY(1,1) NOT NULL,
	[LoginName] [VARCHAR](255) NOT NULL,
	[PasswordHash] [NVARCHAR](255) NULL,
	[PasswordSalt] [NVARCHAR](255) NULL,
	[PasswordModifiedDate] [SMALLDATETIME] NULL,
	[LastLoginDate] [DATETIME] NULL,
	[CreateDate] [DATETIME] NOT NULL,
	[Status] [SMALLINT] NULL,
	[SecurityStamp] NVARCHAR(MAX) NULL,
	[EmailConfirmed] BIT NULL,
	[AuthenticatorKey] NVARCHAR(MAX) NULL,
	[TwoFactorEnabled] [BIT] NOT NULL DEFAULT 0,
	[AccessFailedCount] [SMALLINT] NOT NULL DEFAULT 0,
	[LockoutEnd] [DATETIMEOFFSET](7) NULL,
	[LockoutEnabled] BIT NOT NULL DEFAULT 0
 CONSTRAINT [PK_GloboUser] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_GloboUser_LoginId] UNIQUE NONCLUSTERED 
(
	[LoginName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Companies](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[Name] [NVARCHAR](MAX) NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[CompanyMembers](
	[Id] [INT] IDENTITY(1,1) NOT NULL,
	[CompanyId] [INT] NOT NULL,
	[MemberEmail] [NVARCHAR](MAX) NULL,
 CONSTRAINT [PK_CompanyMembers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CompanyMembers]  WITH CHECK ADD  CONSTRAINT [FK_CompanyMembers_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CompanyMembers] CHECK CONSTRAINT [FK_CompanyMembers_Companies_CompanyId]
GO

INSERT INTO dbo.Companies ( Name ) VALUES ( N'Acme Blasting' )
GO
INSERT INTO dbo.Companies ( Name ) VALUES ( N'Mars Exploration' )
GO
INSERT INTO dbo.CompanyMembers ( CompanyId, MemberEmail ) VALUES ( 1, N'wile@acme.com' )
GO
INSERT INTO dbo.CompanyMembers ( CompanyId, MemberEmail ) VALUES ( 1, N'rr@acme.com' )
GO
INSERT INTO dbo.CompanyMembers ( CompanyId, MemberEmail ) VALUES ( 1, N'marvin@acme.com' )
GO
INSERT INTO dbo.CompanyMembers ( CompanyId, MemberEmail ) VALUES ( 2, N'kim@mars.com' )
GO
INSERT INTO dbo.CompanyMembers ( CompanyId, MemberEmail ) VALUES ( 2, N'stanley@mars.com' )
GO
INSERT INTO dbo.CompanyMembers ( CompanyId, MemberEmail ) VALUES ( 2, N'robinson@mars.com' )
GO

INSERT INTO dbo.GlobomanticsUser ( LoginName, PasswordHash, PasswordSalt, PasswordModifiedDate, LastLoginDate, CreateDate, Status,
SecurityStamp, EmailConfirmed, AuthenticatorKey, TwoFactorEnabled, AccessFailedCount, LockoutEnd, LockoutEnabled)
						  VALUES (
									'wile@acme.com'
								   ,N'JtDLryRhMRtrGSwBn8GBLz0hfZI7SL/uDV75sJmroSw/hp1hQWw06e3zgL2UGalHmM+XBfsXSeSkCb8WuUoyzTicVwuRkIRDlWPEOqbjipjW/nDvzJadNlX1lYPgNgSSZdDEdpOyzMIMbK6BVakGzpliqoEJWe2AGnM7xwjMZi0='                   
								   ,N'vM66K0GClzdt0S83jG4d6VX1B54uT2E4mVVOuc24BXGFmQNDjKrf13vceqKou5/USGSCZwumm6G1aZ2ZPQNKaLcOz52qDeUx6eW5aqR77H30TQ6f3zSVU8WfjqCC83j8s0gsw/UA/QG3OuuhXE9n8ceYxdTVFYEj2Qbb9LH0QFU='                   
								   ,GETDATE() 
								   ,NULL             
								   ,GETDATE()             
								   ,1, NULL, 1, NULL, 0, 0, NULL, 1 )
GO
INSERT INTO dbo.GlobomanticsUser ( LoginName, PasswordHash, PasswordSalt, PasswordModifiedDate, LastLoginDate, CreateDate, Status,
SecurityStamp, EmailConfirmed, AuthenticatorKey, TwoFactorEnabled, AccessFailedCount, LockoutEnd, LockoutEnabled)
						  VALUES (
									'rr@acme.com'
								   ,N'FM02FEvApDwXfyOjDyI67flBYIot4t6FVqu+Myu6qnFAPnyYsZk5pSbxFrPQyHl0JE/BiDDVvwPQZB+OOmsmMb4RFjEc8e79Aa8XDblcwrap0UFCNzG52TXm8sTLscVVNZZYKZFlMao2b13V4GM6BxYz8DYhyhHzZoD6f2HEu1E='                   
								   ,N'zIA0xNUgZL2b+b0lrtcMfNiKC+tpwgh6mdmhpj8mfdXemJXcckXH8IptqorWVGKVQe27VxBD3rlktrn0+z2yjBWztwvfN/xsnekC2yxY3eTb6wtunS8IS52eW4dJrBFSxpIB2zLW7WH/hzbWLt3NvDTJYxk9btP0DdG2ZeJMyVw='                   
								   ,GETDATE() 
								   ,NULL             
								   ,GETDATE()             
								   ,1, NULL, 1, NULL, 0, 0, NULL, 1 )
GO
INSERT INTO dbo.GlobomanticsUser ( LoginName, PasswordHash, PasswordSalt, PasswordModifiedDate, LastLoginDate, CreateDate, Status,
SecurityStamp, EmailConfirmed, AuthenticatorKey, TwoFactorEnabled, AccessFailedCount, LockoutEnd, LockoutEnabled) 
						  VALUES (
									'kim@mars.com'
								   ,N'BvJyJI8wmY9r/4NgVud2EwwbDqNzK9UQ3+Oxy/erYoq3aUm+E1wCgwkyvkagdkucs/LaBS56ddTWS1xogL7msAeM0We37suklRIt6QFSWlqef//SxDcKO8I7bpConj/0ydGu8ix9Fpwi1R5IoEjHns+qfR6hII1Rn0POHTz6UdA='                   
								   ,N'igxbd/GdeN+TrJTsFXdkv1I6kD4t+9fCZIIWM3KmkwXNR2koIR2SPHf8ikr8yO2wkaJrVnNZtyL4+v9+o7JaHw4ZHgmPSWN4NSaRIvZU2arpjQDCAsm6NWpVrfFy4lWMz766kSIG1WxK+mS5Cfy2sfLGA2ejlzgGUxyEa19jFOY='                   
								   ,GETDATE() 
								   ,NULL             
								   ,GETDATE()             
								   ,1, NULL, 1, NULL, 0, 0, NULL, 1 )
GO
INSERT INTO dbo.GlobomanticsUser ( LoginName, PasswordHash, PasswordSalt, PasswordModifiedDate, LastLoginDate, CreateDate, Status,
SecurityStamp, EmailConfirmed, AuthenticatorKey, TwoFactorEnabled, AccessFailedCount, LockoutEnd, LockoutEnabled)
						  VALUES (
									'stanley@mars.com'
								   ,N'nvJhGa7F2a4NttgXGMNTyLIVvaxuEA0Dvgz39XCkeU6NyVY2GqIbpkIRATOfBV88QFjjop6uunmu7164VKBluQ9arS+x90UzLrS2UW7I9SMDNP+gGXin33EULiBiQ1suFlCYEcYkKR/k3eMZot366zUffxGfmwJ6+ycUbbbso+c='                   
								   ,N'Mg414KZXHjCMs5UGkdssUdeAK6sMMLk+2lgicRCIAALElCevDIgzDIgqPVY5Elkhg7Xl8XoJWE96cKW1GHFw2Nu61a+lT+6YsZ61Wu6PTsa36Vt5I4uVlbd3uo+sbucGijvHAPaqYM3Da5CpmJovMI0t2Sonql6yabqGEbMdrMM='                   
								   ,GETDATE() 
								   ,NULL             
								   ,GETDATE()             
								   ,1, NULL, 1, NULL, 0, 0, NULL, 1 )
								   
GO

CREATE TABLE [dbo].[UserToken](
	[UserId] [bigint] NOT NULL,
	[LoginProvider] [NVARCHAR](200) NOT NULL,
	[Name] [NVARCHAR](200) NOT NULL,
	[Value] [NVARCHAR](MAX) NULL,
 CONSTRAINT [PK_UserToken] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
	   ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE dbo.[UserToken]  WITH CHECK ADD  CONSTRAINT [FK_UserToken_User_UserId] FOREIGN KEY([UserId])
REFERENCES dbo.[GlobomanticsUser] ([UserId])
ON DELETE CASCADE
GO

ALTER TABLE dbo.[UserToken] CHECK CONSTRAINT [FK_UserToken_User_UserId]
GO


