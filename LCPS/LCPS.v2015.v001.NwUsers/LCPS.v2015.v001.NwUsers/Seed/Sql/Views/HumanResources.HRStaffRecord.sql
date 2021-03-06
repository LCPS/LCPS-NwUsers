
/****** Object:  View [HumanResources].[HRStaffRecord]    Script Date: 7/19/2015 9:58:34 PM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[HumanResources].[HRStaffRecord]'))
DROP VIEW [HumanResources].[HRStaffRecord]

/****** Object:  View [HumanResources].[HRStaffRecord]    Script Date: 7/19/2015 9:58:34 PM ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[HumanResources].[HRStaffRecord]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [HumanResources].[HRStaffRecord]
AS
SELECT        TOP (100) PERCENT HumanResources.HRStaffPosition.PositionKey, HumanResources.HRStaffPosition.Status AS StatusVal, 
                         HumanResources.HRStaffPosition.FiscalYear, HumanResources.HRBuilding.BuildingKey, HumanResources.HRBuilding.BuildingId, 
                         HumanResources.HRBuilding.Name AS BuildingName, HumanResources.HRBuilding.Description, 
                         HumanResources.HREmployeeType.EmployeeTypeLinkId AS EmployeeTypeKey, HumanResources.HREmployeeType.EmployeeTypeId, 
                         HumanResources.HREmployeeType.EmployeeTypeName, HumanResources.HREmployeeType.Category, HumanResources.HRStaff.StaffKey, 
                         HumanResources.HRStaff.StaffId, HumanResources.HRStaff.FirstName, HumanResources.HRStaff.MiddleInitial, HumanResources.HRStaff.LastName, 
                         HumanResources.HRStaff.Gender AS GenderVal, HumanResources.HRStaff.Birthdate, HumanResources.HRJobTitle.JobTitleKey, 
                         HumanResources.HRJobTitle.JobTitleId, HumanResources.HRJobTitle.JobTitleName
FROM            HumanResources.HRStaff INNER JOIN
                         HumanResources.HRStaffPosition ON HumanResources.HRStaff.StaffKey = HumanResources.HRStaffPosition.StaffMemberId INNER JOIN
                         HumanResources.HRBuilding ON HumanResources.HRStaffPosition.BuildingKey = HumanResources.HRBuilding.BuildingKey INNER JOIN
                         HumanResources.HREmployeeType ON 
                         HumanResources.HRStaffPosition.EmployeeTypeKey = HumanResources.HREmployeeType.EmployeeTypeLinkId INNER JOIN
                         HumanResources.HRJobTitle ON HumanResources.HRStaffPosition.JobTitleKey = HumanResources.HRJobTitle.JobTitleKey
ORDER BY HumanResources.HRStaff.LastName, HumanResources.HRStaff.FirstName, HumanResources.HRStaff.MiddleInitial
' 

