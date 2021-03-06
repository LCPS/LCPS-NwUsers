IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsLdap].[Ldap-Staff-Import]'))
DROP VIEW [LcpsLdap].[Ldap-Staff-Import]

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsLdap].[Ldap-Staff-Import]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [LcpsLdap].[Ldap-Staff-Import]
AS
SELECT        TOP (100) PERCENT HumanResources.HRStaffRecord.StaffKey, HumanResources.HRStaffRecord.StaffId, HumanResources.HRStaffRecord.LastName, 
                         HumanResources.HRStaffRecord.FirstName, HumanResources.HRStaffRecord.MiddleInitial, HumanResources.HRStaffRecord.BuildingKey, 
                         HumanResources.HRStaffRecord.BuildingName, HumanResources.HRStaffRecord.EmployeeTypeKey, HumanResources.HRStaffRecord.EmployeeTypeName, 
                         HumanResources.HRStaffRecord.JobTitleKey, HumanResources.HRStaffRecord.JobTitleName, HumanResources.HRStaffRecord.StatusVal, 
                         LcpsLdap.LdapAccount.AccountId, LcpsLdap.LdapAccount.UserName, LcpsLdap.LdapAccount.InitialPassword
FROM            HumanResources.HRStaffRecord INNER JOIN
                         LcpsLdap.LdapAccount ON HumanResources.HRStaffRecord.StaffKey = LcpsLdap.LdapAccount.UserKey
ORDER BY HumanResources.HRStaffRecord.LastName, HumanResources.HRStaffRecord.FirstName, HumanResources.HRStaffRecord.MiddleInitial
' 
