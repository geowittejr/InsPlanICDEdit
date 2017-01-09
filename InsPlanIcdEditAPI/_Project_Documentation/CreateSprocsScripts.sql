

/************************************************************************************************************************/




USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_AddInsPlanDxHistory]    Script Date: 12/11/2013 11:55:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Add a history record
	Usage:
					exec sp_LMN_AddInsPlanDxHistory N'174.0', N'ADP01', NULL, 1, 'gwitte'
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_AddInsPlanDxHistory]
	@Icd9 nvarchar(16),
	@InsPlanId nvarchar(50) = NULL,
	@DiseaseGroupId int = NULL,
	@ActionType int,
	@Username nvarchar(50) = N'Anonymous'
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [SQLDB].dbo.LMN_InsPlanDxHistory(Icd9, InsPlanId, TheDiseaseGroupsId, ActionType, Username)
	VALUES(@Icd9, @InsPlanId, @DiseaseGroupId, @ActionType, @Username)

END

GO




/************************************************************************************************************************/


USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_AddInsPlanIcd9]    Script Date: 12/11/2013 12:01:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Add a record to LMN_InsPlanIcd9s linking the specified plan to the specified ICD9 code
	Usage:
					exec sp_LMN_AddInsPlanIcd9 N'ADP01', N'200.30'
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_AddInsPlanIcd9]
	@PlanId nvarchar(50),
	@Icd9 nvarchar(16),
	@Username nvarchar(50) = N'Anonymous'
AS
BEGIN
	SET NOCOUNT ON;

	/* If the record already exists, just return without doing anything. */
	DECLARE @RecordCount int
	SET @RecordCount = (SELECT COUNT(InsPlanId) FROM [SQLDB].dbo.LMN_InsPlanIcd9s WHERE InsPlanId = @PlanId AND Icd9 = @Icd9)
	IF(@RecordCount > 0)
		RETURN
	
	INSERT INTO [SQLDB].dbo.LMN_InsPlanIcd9s(InsPlanId, Icd9)
		VALUES(@PlanId, @Icd9)

	/* If success, add a history row */
	IF(@@ERROR = 0)
	BEGIN
		EXEC sp_LMN_AddInsPlanDxHistory @Icd9 = @Icd9, @InsPlanId = @PlanId, @ActionType = 1, @Username = @Username
	END

END

GO





/************************************************************************************************************************/



USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_GetIcd9]    Script Date: 12/11/2013 11:16:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Get icd9
	Usage:
					exec sp_LMN_GetIcd9 N'174.0'
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_GetIcd9]
	@Icd9 nvarchar(16)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT icd.Icd9, icd.Description, icd.Icd10Codes, COUNT(ipicd.InsPlanId) InsPlanCount
	FROM LMN_Icd9Codes icd
		LEFT OUTER JOIN LMN_InsPlanIcd9s ipicd ON icd.Icd9 = ipicd.Icd9
	WHERE icd.Icd9 = @Icd9
	GROUP BY icd.Icd9, icd.Description, icd.Icd10Codes
	ORDER BY icd.Icd9
END

GO




/************************************************************************************************************************/



USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_GetIcd9s]    Script Date: 12/11/2013 11:24:34 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Get a filtered, sorted list of icd9s
	Usage:
					exec sp_LMN_GetIcd9s '', 0, 19, 'InsPlanCount', 1, NULL
					exec sp_LMN_GetIcd9s '174', 0, 19, 'InsPlanCount', 0, NULL
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_GetIcd9s]
	@FilterText nvarchar(100) = '',
	@StartIndex int,
	@EndIndex int,
	@SortColumn nvarchar(50) = 'Icd9',
	@SortDesc bit = 0,
	@TotalIcd9s int = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @OrderBy nvarchar(50) =	
	CASE 
		WHEN @SortColumn = 'InsPlanCount' AND @SortDesc = 1 THEN N'COUNT(ipicd.InsPlanId) DESC, icd.Icd9'
		WHEN @SortColumn = 'InsPlanCount' AND @SortDesc = 0 THEN N'COUNT(ipicd.InsPlanId), icd.Icd9'
		WHEN @SortColumn = 'Description' AND @SortDesc = 1 THEN N'icd.Description DESC, icd.Icd9'
		WHEN @SortColumn = 'Description' AND @SortDesc = 0 THEN N'icd.Description, icd.Icd9'
		WHEN @SortColumn = 'Icd9' AND @SortDesc = 1 THEN N'icd.Icd9 DESC'
		ELSE N'icd.Icd9' --default to icd.Icd9 ASC
	END	

	DECLARE @sql nvarchar(4000) = N'
	WITH Icd9Rows AS
	(
		 SELECT 
			icd.Icd9, 
			icd.Description, 
			icd.Icd10Codes, 
			COUNT(ipicd.InsPlanId) InsPlanCount,
			ROW_NUMBER() OVER (ORDER BY ' + @OrderBy + ') AS Row
		FROM LMN_Icd9Codes icd
				LEFT OUTER JOIN LMN_InsPlanIcd9s ipicd ON icd.Icd9 = ipicd.Icd9
		WHERE 
			icd.Icd9 LIKE ''%'' + @filterText + ''%''
			OR icd.Description LIKE ''%'' + @filterText + ''%''
		GROUP BY icd.Icd9, icd.Description, icd.Icd10Codes
	)
		
	SELECT *
	FROM Icd9Rows
	WHERE Row > @startIndex AND Row <= @endIndex + 1
	ORDER BY Row;
	
	WITH Icd9Rows AS
	(
		SELECT DISTINCT 
			icd.Icd9
		FROM LMN_Icd9Codes icd
				LEFT OUTER JOIN LMN_InsPlanIcd9s ipicd ON icd.Icd9 = ipicd.Icd9
		WHERE 
			icd.Icd9 LIKE ''%'' + @filterText + ''%''
			OR icd.Description LIKE ''%'' + @filterText + ''%''
	)
	
	SELECT @totalIcd9s = COUNT(*) FROM Icd9Rows;'
	

	DECLARE @parmDefinition nvarchar(500) = N'
		@filterText nvarchar(100),
		@startIndex int,
		@endIndex int,
		@totalIcd9s int OUTPUT'

	exec sp_executesql 
		@sql, 
		@parmDefinition,
		@filterText = @FilterText,
		@startIndex = @StartIndex,
		@endIndex = @EndIndex,
		@totalIcd9s = @TotalIcd9s OUTPUT

END

GO





/************************************************************************************************************************/



USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_GetIcd9InsPlans]    Script Date: 12/11/2013 12:09:18 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Get a filtered, sorted list of insurance plans for a specific icd9
	Usage:
					exec sp_LMN_GetIcd9InsPlans N'174.0', '', 0, 9, 'on', 'Icd9', 0, null
					exec sp_LMN_GetIcd9InsPlans N'174.0', '', 0, 9, 'off', 'Icd9', 1, null
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_GetIcd9InsPlans]
	@Icd9 nvarchar(16),
	@FilterText nvarchar(100) = '',
	@StartIndex int,
	@EndIndex int,
	@Status nvarchar(10) = '',
	@SortColumn nvarchar(50) = 'Icd9',
	@SortDesc bit = 0,
	@TotalPlans int = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	/* If this icd9 doesn't exist return no records */
	SET @Icd9 = (SELECT icd.Icd9 FROM [SQLDB].dbo.LMN_Icd9Codes icd WHERE icd.Icd9 = @Icd9)
	IF(@Icd9 IS NULL)
	BEGIN
		SELECT NULL AS InsPlanId, NULL AS InsCoId, NULL AS InsCoId, NULL AS InsCoIdAlternate, NULL AS InsCoDescAlternate, NULL AS EnabledOnIcd9, NULL AS Icd9Count, NULL AS [Row] WHERE 1 = 2
		RETURN
	END

	DECLARE @OrderBy nvarchar(50) =	
	CASE 
		WHEN @SortColumn = 'Icd9Count' AND @SortDesc = 1 THEN N'icd9Count.Icd9Count DESC, ip.InsPlanId'
		WHEN @SortColumn = 'Icd9Count' AND @SortDesc = 0 THEN N'icd9Count.Icd9Count, ip.InsPlanId'
		WHEN @SortColumn = 'InsCoDesc' AND @SortDesc = 1 THEN N'ip.InsCoDesc DESC, ip.InsPlanId'
		WHEN @SortColumn = 'InsCoDesc' AND @SortDesc = 0 THEN N'ip.InsCoDesc, ip.InsPlanId'
		WHEN @SortColumn = 'InsPlanId' AND @SortDesc = 1 THEN N'ip.InsPlanId DESC'
		ELSE N'ip.InsPlanId' --default to ip.InsPlanId ASC
	END	

	DECLARE @StatusWhereClause nvarchar(50) =
	CASE
		WHEN @Status = 'on' THEN N'AND ipicd.EnabledOnIcd9 = 1'
		WHEN @Status = 'off' THEN N'AND ipicd.EnabledOnIcd9 = 0'
		ELSE N''
	END

	DECLARE @sql nvarchar(4000) = N'
	WITH PlanRows AS
	(
		SELECT 
			ip.InsPlanId, 
			ip.InsCoId, 
			ip.InsCoDesc, 
			ip.InsCoIdAlternate, 
			ip.InsCoDescAlternate, 
			ipicd.EnabledOnIcd9,
			icd9Count.Icd9Count,
			planHist.ActionDate,
			planHist.Username,
			ROW_NUMBER() OVER (ORDER BY ' + @OrderBy + ') AS Row
		FROM LMN_InsPlans ip
			LEFT OUTER JOIN (
				SELECT ins.InsPlanId, CASE WHEN ipicds.InsPlanId IS NOT NULL THEN 1 ELSE 0 END EnabledOnIcd9
				FROM LMN_InsPlans ins
				LEFT OUTER JOIN (SELECT InsPlanId FROM LMN_InsPlanIcd9s WHERE Icd9 = @icd9) ipicds ON ipicds.InsPlanId = ins.InsPlanId
			) ipicd ON ipicd.InsPlanId = ip.InsPlanId
			LEFT OUTER JOIN (
				SELECT ins.InsPlanId, CASE WHEN icdcount.InsPlanId IS NOT NULL THEN icdcount.Icd9Count ELSE 0 END Icd9Count
				FROM LMN_InsPlans ins
				LEFT OUTER JOIN (
					SELECT InsPlanId, COUNT(Icd9) Icd9Count
					FROM [SQLDB].dbo.LMN_InsPlanIcd9s
					GROUP BY InsPlanId				
				) icdcount ON icdcount.InsPlanId = ins.InsPlanId
			) icd9Count ON icd9Count.InsPlanId = ip.InsPlanId
			LEFT OUTER JOIN(
			  SELECT hist.InsPlanId, hist.ActionDate, hist.Username
			  FROM LMN_InsPlanDxHistory hist
				INNER JOIN(
					SELECT InsPlanId, MAX(ID) AS ID
					FROM LMN_InsPlanDxHistory
					WHERE Icd9 = @icd9
					GROUP BY InsPlanId
				)recent ON hist.ID = recent.ID
			) planHist ON planHist.InsPlanId = ip.InsPlanId
		WHERE 
			(ip.InsPlanId LIKE ''%'' + @filterText + ''%''
			OR ip.InsCoDesc LIKE ''%'' + @filterText + ''%'')
			' + @StatusWhereClause + '
	)
		
	SELECT *
	FROM PlanRows
	WHERE Row > @startIndex AND Row <= @endIndex + 1
	ORDER BY Row;
	
	WITH PlanRows AS
	(
		SELECT ip.InsPlanId
		FROM LMN_InsPlans ip	
			LEFT OUTER JOIN (
				SELECT ins.InsPlanId, CASE WHEN ipicds.InsPlanId IS NOT NULL THEN 1 ELSE 0 END EnabledOnIcd9
				FROM LMN_InsPlans ins
				LEFT OUTER JOIN (SELECT InsPlanId FROM LMN_InsPlanIcd9s WHERE Icd9 = @icd9) ipicds ON ipicds.InsPlanId = ins.InsPlanId
			) ipicd ON ipicd.InsPlanId = ip.InsPlanId
		WHERE 
			(ip.InsPlanId LIKE ''%'' + @filterText + ''%''
			OR ip.InsCoDesc LIKE ''%'' + @filterText + ''%'')
			' + @StatusWhereClause + '
	)
	
	SELECT @totalPlans = COUNT(*) FROM PlanRows;'
	
	DECLARE @parmDefinition nvarchar(500) = N'
		@icd9 nvarchar(16),
		@filterText nvarchar(100),
		@startIndex int,
		@endIndex int,
		@totalPlans int OUTPUT'

	exec sp_executesql 
		@sql, 
		@parmDefinition,
		@icd9 = @Icd9,
		@filterText = @FilterText,
		@startIndex = @StartIndex,
		@endIndex = @EndIndex,
		@totalPlans = @TotalPlans OUTPUT

END


GO




/************************************************************************************************************************/




USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_GetInsPlan]    Script Date: 12/11/2013 11:29:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Get ins plan
	Usage:
					exec sp_LMN_GetInsPlan N'AEP001'
					exec sp_LMN_GetInsPlan N'BSS001'
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_GetInsPlan]
	@PlanId nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT ip.InsPlanId, ip.InsCoId, ip.InsCoDesc, ip.InsCoIdAlternate, ip.InsCoDescAlternate, COUNT(ipicd.Icd9) Icd9Count
	FROM LMN_InsPlans ip
		LEFT OUTER JOIN LMN_InsPlanIcd9s ipicd ON ip.InsPlanId = ipicd.InsPlanId
	WHERE ip.InsPlanId = @PlanId
	GROUP BY ip.InsPlanId, ip.InsCoId, ip.InsCoDesc, ip.InsCoIdAlternate, ip.InsCoDescAlternate
	ORDER BY ip.InsPlanId
END

GO



/************************************************************************************************************************/




USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_GetInsPlanDxHistory]    Script Date: 12/12/2013 10:46:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Get a filtered, sorted list of history records
	Usage:
					exec sp_LMN_GetInsPlanDxHistory '174.0', 'Icd9', 0, 19, 'ActionDate', 1, NULL
					exec sp_LMN_GetInsPlanDxHistory 'ADP01', 'InsPlanId', 0, 19, 'ActionDate', 1, NULL
					exec sp_LMN_GetInsPlanDxHistory 'gwitte', 'Username', 0, 19, 'ActionDate', 1, NULL
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_GetInsPlanDxHistory]
	@EntityId nvarchar(50) = NULL,
	@EntityType nvarchar(50) = NULL,
	@StartIndex int,
	@EndIndex int,
	@SortColumn nvarchar(50) = 'ActionDate',
	@SortDesc bit = 1,
	@TotalTrans int = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @OrderBy nvarchar(50) =	
	CASE 
		WHEN @SortColumn = 'Username' AND @SortDesc = 1 THEN N'hist.Username DESC, hist.ActionDate DESC'
		WHEN @SortColumn = 'Username' AND @SortDesc = 0 THEN N'hist.Username, hist.ActionDate DESC'
		WHEN @SortColumn = 'ActionDate' AND @SortDesc = 0 THEN N'hist.ActionDate'
		ELSE N'hist.ActionDate DESC' --default
	END	

	DECLARE @Where nvarchar(2000) = 
	CASE
		WHEN @EntityId IS NULL AND @EntityType IS NULL THEN N''
		WHEN @EntityType = 'Icd9' THEN N'WHERE hist.Icd9 = @entityId'
		WHEN @EntityType = 'Username' THEN N'WHERE hist.Username = @entityId'
		WHEN @EntityType = 'InsPlanId' THEN N'WHERE hist.InsPlanId = @entityId'
		
		--@EntityType does not contain a valid value, so the where clause will return no records
		ELSE N'WHERE 1 = 2'
	END

	DECLARE @sql nvarchar(4000) = N'
	WITH TranRows AS
	(
		 SELECT 
			hist.Id,
			hist.Icd9, 
			hist.InsPlanId,
			hist.TheDiseaseGroupsId DiseaseGroupId,
			hist.ActionType,
			hist.Username,
			hist.ActionDate,
			ROW_NUMBER() OVER (ORDER BY ' + @OrderBy + ') AS Row
		FROM LMN_InsPlanDxHistory hist
		' + @Where + '
	)
		
	SELECT *
	FROM TranRows
	WHERE Row > @startIndex AND Row <= @endIndex + 1
	ORDER BY Row;

	SELECT @totalTrans = (SELECT COUNT(*) FROM LMN_InsPlanDxHistory hist ' + @Where + ')'

	DECLARE @parmDefinition nvarchar(500) = N'
		@entityId nvarchar(50),
		@startIndex int,
		@endIndex int,
		@totalTrans int OUTPUT'

	exec sp_executesql 
		@sql, 
		@parmDefinition,
		@entityId = @EntityId,
		@startIndex = @StartIndex,
		@endIndex = @EndIndex,
		@totalTrans = @TotalTrans OUTPUT

END

GO






/************************************************************************************************************************/


USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_GetInsPlanIcd9s]    Script Date: 12/11/2013 11:10:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Get a filtered, sorted list of icd9s for a given ins plan
	Usage:
					exec sp_LMN_GetInsPlanIcd9s 'ADP01', '', 0, 19, 'off', 'InsPlanCount', 1, NULL
					exec sp_LMN_GetInsPlanIcd9s 'ADP01', '174', 0, 19, 'on', 'InsPlanCount', 1, NULL
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_GetInsPlanIcd9s]
	@PlanId nvarchar(50),
	@FilterText nvarchar(100) = '',
	@StartIndex int,
	@EndIndex int,
	@Status nvarchar(10) = '',
	@SortColumn nvarchar(50) = 'Icd9',
	@SortDesc bit = 0,
	@TotalIcd9s int = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	/* If this plan id doesn't exist return no records */
	SET @PlanId = (SELECT InsPlanId FROM [SQLDB].dbo.LMN_InsPlans ip WHERE ip.InsPlanId = @PlanId)
	IF(@PlanId IS NULL)
	BEGIN
		SELECT NULL AS Icd9, NULL AS Description, NULL AS Icd10Codes, NULL AS EnabledOnPlan, NULL AS InsPlanCount, NULL AS [Row] WHERE 1 = 2
		RETURN
	END

	DECLARE @OrderBy nvarchar(50) =	
	CASE 
		WHEN @SortColumn = 'Description' AND @SortDesc = 1 THEN N'codes.Description DESC, codes.Icd9'
		WHEN @SortColumn = 'Description' AND @SortDesc = 0 THEN N'codes.Description, codes.Icd9'
		WHEN @SortColumn = 'InsPlanCount' AND @SortDesc = 1 THEN N'ipcount.InsPlanCount DESC, codes.Icd9'
		WHEN @SortColumn = 'InsPlanCount' AND @SortDesc = 0 THEN N'ipcount.InsPlanCount, codes.Icd9'
		WHEN @SortColumn = 'Icd9' AND @SortDesc = 1 THEN N'codes.Icd9 DESC'
		ELSE N'codes.Icd9' --default to codes.Icd9 ASC
	END	

	DECLARE @StatusWhereClause nvarchar(50) =
	CASE
		WHEN @Status = 'on' THEN N'AND ipicd.EnabledOnPlan = 1'
		WHEN @Status = 'off' THEN N'AND ipicd.EnabledOnPlan = 0'
		ELSE N''
	END

	DECLARE @sql nvarchar(4000) = N'
	WITH Icd9Rows AS
	(
		 SELECT 
			codes.Icd9, 
			codes.Description, 
			codes.Icd10Codes, 
			ipicd.EnabledOnPlan,
			ipcount.InsPlanCount,
			ROW_NUMBER() OVER (ORDER BY ' + @OrderBy + ') AS Row
		FROM [SQLDB].dbo.LMN_Icd9Codes codes	
			LEFT OUTER JOIN (
				SELECT icd.Icd9, CASE WHEN ipicds.Icd9 IS NOT NULL THEN 1 ELSE 0 END EnabledOnPlan
				FROM LMN_Icd9Codes icd
				LEFT OUTER JOIN (SELECT Icd9 FROM LMN_InsPlanIcd9s WHERE InsPlanId = @planId) ipicds ON ipicds.Icd9 = icd.Icd9
			) ipicd ON ipicd.Icd9 = codes.Icd9
			LEFT OUTER JOIN (
				SELECT icd.Icd9, CASE WHEN plancount.Icd9 IS NOT NULL THEN plancount.InsPlanCount ELSE 0 END InsPlanCount
				FROM LMN_Icd9Codes icd
				LEFT OUTER JOIN (
					SELECT Icd9, COUNT(InsPlanId) InsPlanCount
					FROM [SQLDB].dbo.LMN_InsPlanIcd9s
					GROUP BY Icd9				
				) plancount ON plancount.Icd9 = icd.Icd9
			) ipcount ON ipcount.Icd9 = codes.Icd9
		WHERE 
			(codes.Icd9 LIKE ''%'' + @filterText + ''%''
			OR codes.Description LIKE ''%'' + @filterText + ''%'')
			' + @StatusWhereClause + '
	)
		
	SELECT *
	FROM Icd9Rows
	WHERE Row > @startIndex AND Row <= @endIndex + 1
	ORDER BY Row;
	
	WITH Icd9Rows AS
	(
		SELECT codes.Icd9 
		FROM [SQLDB].dbo.LMN_Icd9Codes codes	
			LEFT OUTER JOIN (
				SELECT icd.Icd9, CASE WHEN ipicds.Icd9 IS NOT NULL THEN 1 ELSE 0 END EnabledOnPlan
				FROM LMN_Icd9Codes icd
				LEFT OUTER JOIN (SELECT Icd9 FROM LMN_InsPlanIcd9s WHERE InsPlanId = @planId) ipicds ON ipicds.Icd9 = icd.Icd9
			) ipicd ON ipicd.Icd9 = codes.Icd9
		WHERE 
			(codes.Icd9 LIKE ''%'' + @filterText + ''%''
			OR codes.Description LIKE ''%'' + @filterText + ''%'')
			' + @StatusWhereClause + '
	)
	
	SELECT @totalIcd9s = COUNT(*) FROM Icd9Rows;'
	
	DECLARE @parmDefinition nvarchar(500) = N'
		@planId nvarchar(50),
		@filterText nvarchar(100),
		@startIndex int,
		@endIndex int,
		@totalIcd9s int OUTPUT'

	exec sp_executesql 
		@sql, 
		@parmDefinition,
		@planId = @PlanId,
		@filterText = @FilterText,
		@startIndex = @StartIndex,
		@endIndex = @EndIndex,
		@totalIcd9s = @TotalIcd9s OUTPUT

END

GO




/************************************************************************************************************************/


USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_GetInsPlans]    Script Date: 12/11/2013 11:04:39 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=================================================================================
	Author:			George Witte
	Create date:	10/07/2013
	Description:	Get a filtered, sorted list of insurance plans 
	Usage:
					exec sp_LMN_GetInsPlans '', 0, 19, 'InsPlanId', 0, NULL
					exec sp_LMN_GetInsPlans 'AEP', 0, 19, 'InsPlanId', 1, NULL
	Updates:
					2013/10/07	GFW	- Created the sproc
					2013/11/30	GFW - Added ability to sort and page records
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_GetInsPlans]
	@FilterText nvarchar(100) = '',
	@StartIndex int,
	@EndIndex int,
	@SortColumn nvarchar(50) = 'InsPlanId',
	@SortDesc bit = 0,
	@TotalPlans int = 0 OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @OrderBy nvarchar(50) =	
	CASE 
		WHEN @SortColumn = 'Icd9Count' AND @SortDesc = 1 THEN N'COUNT(ipicd.Icd9) DESC, ip.InsPlanId'
		WHEN @SortColumn = 'Icd9Count' AND @SortDesc = 0 THEN N'COUNT(ipicd.Icd9), ip.InsPlanId'
		WHEN @SortColumn = 'InsCoDesc' AND @SortDesc = 1 THEN N'ip.InsCoDesc DESC, ip.InsPlanId'
		WHEN @SortColumn = 'InsCoDesc' AND @SortDesc = 0 THEN N'ip.InsCoDesc, ip.InsPlanId'
		WHEN @SortColumn = 'InsPlanId' AND @SortDesc = 1 THEN N'ip.InsPlanId DESC'
		ELSE N'ip.InsPlanId' --default to ip.InsPlanId ASC
	END	

	DECLARE @sql nvarchar(4000) = N'
	WITH PlanRows AS
	(
		 SELECT 
			ip.InsPlanId, 
			ip.InsCoId, 
			ip.InsCoDesc, 
			ip.InsCoIdAlternate, 
			ip.InsCoDescAlternate, 
			COUNT(ipicd.Icd9) Icd9Count,
			ROW_NUMBER() OVER (ORDER BY ' + @OrderBy + ') AS Row
		FROM LMN_InsPlans ip
				LEFT OUTER JOIN LMN_InsPlanIcd9s ipicd ON ip.InsPlanId = ipicd.InsPlanId
		WHERE 
			ip.InsPlanId LIKE ''%'' + @filterText + ''%''
			OR ip.InsCoDesc LIKE ''%'' + @filterText + ''%''
		GROUP BY ip.InsPlanId, ip.InsCoId, ip.InsCoDesc, ip.InsCoIdAlternate, ip.InsCoDescAlternate
	)
		
	SELECT *
	FROM PlanRows
	WHERE Row > @startIndex AND Row <= @endIndex + 1
	ORDER BY Row;
	
	WITH PlanRows AS
	(
		 SELECT DISTINCT 
			ip.InsPlanId
		FROM LMN_InsPlans ip
				LEFT OUTER JOIN LMN_InsPlanIcd9s ipicd ON ip.InsPlanId = ipicd.InsPlanId
		WHERE 
			ip.InsPlanId LIKE ''%'' + @filterText + ''%''
			OR ip.InsCoDesc LIKE ''%'' + @filterText + ''%''
	)
	
	SELECT @totalPlans = COUNT(*) FROM PlanRows;'
	

	DECLARE @parmDefinition nvarchar(500) = N'
		@filterText nvarchar(100),
		@startIndex int,
		@endIndex int,
		@totalPlans int OUTPUT'

	exec sp_executesql 
		@sql, 
		@parmDefinition,
		@filterText = @FilterText,
		@startIndex = @StartIndex,
		@endIndex = @EndIndex,
		@totalPlans = @TotalPlans OUTPUT

END

GO




/************************************************************************************************************************/



USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_GetUser]    Script Date: 12/19/2013 4:35:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Get user
	Usage:
					exec sp_LMN_GetUser N'gwitte'
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_GetUser]
	@Username nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		Users.user_id AS Id, 
		Users.username AS Username, 
		Users.first_name AS FirstName, 
		Users.last_name AS LastName,
		CASE
			WHEN Users.user_type = 'IN' OR Users.SQLDBAdmin = 1 THEN 1
			ELSE 0
		END AS IsAuthorized
	FROM Users
	WHERE
		Users.username = @Username
		AND Users.disabled = 0

END

GO




/************************************************************************************************************************/


USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_PopulateIcd9CodesTable]    Script Date: 12/11/2013 11:31:26 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Populate LMN_Icd9Codes table
	Usage:
					exec sp_LMN_PopulateIcd9CodesTable
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_PopulateIcd9CodesTable]
AS
BEGIN

	/* Delete the existing data, then repopulate it */
	DELETE LMN_Icd9Codes;

	DECLARE @Icd9 varchar(16);
	DECLARE @Icd10 varchar(16);
	DECLARE @Description varchar(128);

	-- Create the cursor
	DECLARE dssCursor CURSOR FAST_FORWARD FOR 
		SELECT Icd9, Icd10, [Description]
		FROM eXRx_DiagnosisSubsites
		ORDER BY Icd9

	-- Open the cursor;
	OPEN dssCursor;

	-- Fetch the first record
	FETCH NEXT FROM dssCursor INTO @Icd9, @Icd10, @Description;

	DECLARE @CombinedDescription varchar(900) = @Description;
	DECLARE @CombinedIcd10Codes varchar(500) = @Icd10;
	DECLARE @CurrentIcd9 varchar(16) = @Icd9;

	-- Check the fetch operation result, if it was a success, continue into the block
	WHILE @@FETCH_STATUS = 0
	BEGIN 
	  -- Fetch next record
	  FETCH NEXT FROM dssCursor INTO @Icd9, @Icd10, @Description;

	  -- Process records
	  IF(@Icd9 = @CurrentIcd9)
	  BEGIN
		SET @CombinedDescription = @CombinedDescription + ' / ' + @Description;
		SET @CombinedIcd10Codes = @CombinedIcd10Codes + ' / ' + @Icd10;
	  END
	  ELSE
		BEGIN
			INSERT INTO LMN_Icd9Codes(Icd9, Description, Icd10Codes)
			VALUES(@CurrentIcd9, @CombinedDescription, @CombinedIcd10Codes);

			SET @CurrentIcd9 = @Icd9;
			SET @CombinedIcd10Codes = @Icd10;
			SET @CombinedDescription = @Description;
		END

	END;

	-- Close the cursor
	CLOSE dssCursor;

	-- Deallocate the cursor
	DEALLOCATE dssCursor;

END

GO



/************************************************************************************************************************/


USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_PopulateInsPlansTable]    Script Date: 12/11/2013 11:32:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Populate LMN_InsPlans table
	Usage:
					exec sp_LMN_PopulateInsPlansTable
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_PopulateInsPlansTable]
AS
BEGIN

SET NOCOUNT ON;

DELETE LMN_InsPlans;

INSERT INTO LMN_InsPlans(InsPlanId, InsCoId, InsCoDesc, InsCoIdAlternate, InsCoDescAlternate)
	SELECT 
		PlanIds.InsPlanId, MIN(PatIns.InsCoId) InsCoId, MIN(PatIns.InsCoDesc) InsCoDesc,
		CASE WHEN MAX(PatIns.InsCoId) <> MIN(PatIns.InsCoId) THEN MAX(PatIns.InsCoId) ELSE NULL END InsCoIdAlternate, 
		CASE WHEN MAX(PatIns.InsCoDesc) <> MIN(PatIns.InsCoDesc) THEN MAX(PatIns.InsCoDesc) ELSE NULL END InsCoDescAlternate
	FROM (SELECT DISTINCT InsPlanId FROM PatientInsurance WHERE PatInsStatus = 1) PlanIds
		inner join (
			SELECT InsPlanId, InsCoId, (InsCoId + ' - ' + InsCoName + ISNULL(NULLIF(', ' + RTRIM(InsCoAddress), ', '), '') + ISNULL(NULLIF(', ' + RTRIM(InsCoCity), ', '), '') + ISNULL(NULLIF(', ' + RTRIM(InsCoState), ', '), '') + ISNULL(NULLIF(', ' + RTRIM(InsCoZip), ', '), '') + ISNULL(NULLIF(', ' + RTRIM(InsCoCountry), ', '), '')) AS InsCoDesc
			FROM PatientInsurance WHERE PatInsStatus = 1) PatIns 
			ON PlanIds.InsPlanId = PatIns.InsPlanId
	GROUP BY PlanIds.InsPlanId
	ORDER BY PlanIds.InsPlanId

END


GO






/************************************************************************************************************************/



USE [SQLDB]
GO

/****** Object:  StoredProcedure [dbo].[sp_LMN_RemoveInsPlanIcd9]    Script Date: 12/11/2013 12:03:24 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
=================================================================================
	Author:			George Witte
	Create date:	12/04/2013
	Description:	Remove record from LMN_InsPlanIcd9s for the specified plan and ICD9 code
	Usage:
					exec sp_LMN_RemoveInsPlanIcd9 N'ADP01', N'140.0'
	Updates:
					2013/12/04	GFW	- Created the sproc
=================================================================================
*/
CREATE PROCEDURE [dbo].[sp_LMN_RemoveInsPlanIcd9]
	@PlanId nvarchar(50),
	@Icd9 nvarchar(16),
	@Username nvarchar(50) = N'Anonymous'
AS
BEGIN
	SET NOCOUNT ON;

	/* If the record doesn't exist, just return without doing anything. */
	DECLARE @RecordCount int
	SET @RecordCount = (SELECT COUNT(InsPlanId) FROM [SQLDB].dbo.LMN_InsPlanIcd9s WHERE InsPlanId = @PlanId AND Icd9 = @Icd9)
	IF(@RecordCount = 0)
		RETURN
	
	DELETE [SQLDB].dbo.LMN_InsPlanIcd9s
	WHERE InsPlanId = @PlanId AND Icd9 = @Icd9

	/* If success, add a history row */
	IF(@@ERROR = 0)
	BEGIN
		EXEC sp_LMN_AddInsPlanDxHistory @Icd9 = @Icd9, @InsPlanId = @PlanId, @ActionType = 2, @Username = @Username
	END

END

GO





/************************************************************************************************************************/