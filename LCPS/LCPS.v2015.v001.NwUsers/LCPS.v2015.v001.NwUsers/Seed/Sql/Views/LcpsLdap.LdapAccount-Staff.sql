
/****** Object:  View [LcpsLdap].[LdapAccount-Staff]    Script Date: 7/29/2015 1:21:50 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsLdap].[LdapAccount-Staff]'))
DROP VIEW [LcpsLdap].[LdapAccount-Staff]
/****** Object:  View [LcpsLdap].[LdapAccount-Staff]    Script Date: 7/29/2015 1:21:50 PM ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsLdap].[LdapAccount-Staff]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [LcpsLdap].[LdapAccount-Staff]
AS
SELECT        HumanResources.HRStaff.StaffId, HumanResources.HRStaff.LastName, HumanResources.HRStaff.FirstName, HumanResources.HRStaff.MiddleInitial, 
                         LcpsLdap.LdapAccount.UserName, LcpsLdap.LdapAccount.Active
FROM            LcpsLdap.LdapAccount INNER JOIN
                         HumanResources.HRStaff ON LcpsLdap.LdapAccount.UserKey = HumanResources.HRStaff.StaffKey
' 
