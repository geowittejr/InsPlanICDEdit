


/************************************************************************************************************************/


USE [SQLDB]
GO

/****** Object:  Table [dbo].[LMN_InsPlans]    Script Date: 10/5/2013 11:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LMN_InsPlans](
	[InsPlanId] [varchar](50) NOT NULL,
	[InsCoId] [varchar](50) NOT NULL,
	[InsCoDesc] [varchar](700) NOT NULL,
	[InsCoIdAlternate] [varchar](50) NULL,
	[InsCoDescAlternate] [varchar](700) NULL,
 CONSTRAINT [PK_LMN_InsPlans] PRIMARY KEY CLUSTERED 
(
	[InsPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Index [IX_LMN_InsPlans_InsCoDesc]    Script Date: 10/3/2013 10:46:35 AM ******/
CREATE NONCLUSTERED INDEX [IX_LMN_InsPlans_InsCoDesc] ON [dbo].[LMN_InsPlans]
(
	[InsCoDesc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_LMN_InsPlans_InsCoDescAlternate]    Script Date: 10/3/2013 10:47:36 AM ******/
CREATE NONCLUSTERED INDEX [IX_LMN_InsPlans_InsCoDescAlternate] ON [dbo].[LMN_InsPlans]
(
	[InsCoDescAlternate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_LMN_InsPlans_InsCoId]    Script Date: 10/3/2013 10:47:53 AM ******/
CREATE NONCLUSTERED INDEX [IX_LMN_InsPlans_InsCoId] ON [dbo].[LMN_InsPlans]
(
	[InsCoId] ASC,
	[InsCoIdAlternate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



/************************************************************************************************************************/



USE [SQLDB]
GO

/****** Object:  Table [dbo].[LMN_Icd9Codes]    Script Date: 10/3/2013 9:37:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LMN_Icd9Codes](
	[Icd9] [varchar](16) NOT NULL,
	[Description] [varchar](900) NOT NULL,
	[Icd10Codes] [varchar](500) NOT NULL,
 CONSTRAINT [PK_LMN_Icd9Codes] PRIMARY KEY CLUSTERED 
(
	[Icd9] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Index [IX_LMN_Icd9Codes_Description]    Script Date: 10/3/2013 9:37:48 AM ******/
CREATE NONCLUSTERED INDEX [IX_LMN_Icd9Codes_Description] ON [dbo].[LMN_Icd9Codes]
(
	[Description] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/****** Object:  Index [IX_LMN_Icd9Codes_Icd10Codes]    Script Date: 10/3/2013 9:38:14 AM ******/
CREATE NONCLUSTERED INDEX [IX_LMN_Icd9Codes_Icd10Codes] ON [dbo].[LMN_Icd9Codes]
(
	[Icd10Codes] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



/************************************************************************************************************************/



USE [SQLDB]
GO

/****** Object:  Table [dbo].[LMN_InsPlanIcd9s]    Script Date: 10/4/2013 5:12:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[LMN_InsPlanIcd9s](
	[InsPlanId] [varchar](50) NOT NULL,
	[Icd9] [varchar](16) NOT NULL,
 CONSTRAINT [PK_LMN_InsPlanIcd9s] PRIMARY KEY CLUSTERED 
(
	[InsPlanId] ASC,
	[Icd9] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



/************************************************************************************************************************/



USE [SQLDB]
GO

/****** Object:  Table [dbo].[LMN_InsPlanDxHistory]    Script Date: 12/13/2013 3:48:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LMN_InsPlanDxHistory](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Icd9] [nvarchar](16) NOT NULL,
	[InsPlanId] [nvarchar](50) NULL,
	[TheDiseaseGroupsId] [int] NULL,
	[ActionType] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[ActionDate] [datetime] NOT NULL,
 CONSTRAINT [PK_LMN_InsPlanDxHistory] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[LMN_InsPlanDxHistory] ADD  CONSTRAINT [DF_LMN_InsPlanDxHistory_ActionDate]  DEFAULT (getdate()) FOR [ActionDate]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 = AddInsPlanIcd9, 2 = RemoveInsPlanIcd9, 3 = CreateIcd9Dx, 4 = EditIcd9Dx, 5 = DeleteIcd9Dx' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LMN_InsPlanDxHistory', @level2type=N'COLUMN',@level2name=N'ActionType'
GO

USE [SQLDB]
GO

/****** Object:  Index [IX_LMN_InsPlanDxHistory_ActionDate]    Script Date: 12/13/2013 3:49:37 PM ******/
CREATE NONCLUSTERED INDEX [IX_LMN_InsPlanDxHistory_ActionDate] ON [dbo].[LMN_InsPlanDxHistory]
(
	[ActionDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

USE [SQLDB]
GO

/****** Object:  Index [IX_LMN_InsPlanDxHistory_Icd9]    Script Date: 12/13/2013 3:49:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_LMN_InsPlanDxHistory_Icd9] ON [dbo].[LMN_InsPlanDxHistory]
(
	[Icd9] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

USE [SQLDB]
GO

/****** Object:  Index [IX_LMN_InsPlanDxHistory_InsPlanId]    Script Date: 12/13/2013 3:49:53 PM ******/
CREATE NONCLUSTERED INDEX [IX_LMN_InsPlanDxHistory_InsPlanId] ON [dbo].[LMN_InsPlanDxHistory]
(
	[InsPlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

USE [SQLDB]
GO

/****** Object:  Index [IX_LMN_InsPlanDxHistory_Username]    Script Date: 12/13/2013 3:50:04 PM ******/
CREATE NONCLUSTERED INDEX [IX_LMN_InsPlanDxHistory_Username] ON [dbo].[LMN_InsPlanDxHistory]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO





/************************************************************************************************************************/
