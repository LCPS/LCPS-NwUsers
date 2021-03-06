
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsEmail].[StuadentEmailAccount]'))
DROP VIEW [LcpsEmail].[StuadentEmailAccount]

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsEmail].[StaffEmailAccount]'))
DROP VIEW [LcpsEmail].[StaffEmailAccount]

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsEmail].[StaffEmailAccount]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [LcpsEmail].[StaffEmailAccount]
AS
SELECT        HumanResources.HRStaff.StaffId, HumanResources.HRStaff.FirstName, HumanResources.HRStaff.MiddleInitial, HumanResources.HRStaff.LastName, 
                         HumanResources.HRStaff.Gender AS GenderVal, HumanResources.HRStaff.Birthdate, LcpsEmail.EmailAccount.EmailAddress, 
                         LcpsEmail.EmailAccount.InitialPassword, LcpsEmail.EmailAccount.RecordId, HumanResources.HRStaff.StaffKey, HumanResources.HRStaffPosition.BuildingKey, 
                         HumanResources.HRStaffPosition.EmployeeTypeKey, HumanResources.HRStaffPosition.JobTitleKey, HumanResources.HRStaffPosition.Status AS StatusVal, 
                         HumanResources.HRStaffPosition.FiscalYear, HumanResources.HRBuilding.Name AS BuildingName, HumanResources.HREmployeeType.EmployeeTypeName, 
                         HumanResources.HRJobTitle.JobTitleName
FROM            LcpsEmail.EmailAccount INNER JOIN
                         HumanResources.HRStaff ON LcpsEmail.EmailAccount.UserLink = HumanResources.HRStaff.StaffKey INNER JOIN
                         HumanResources.HRStaffPosition ON HumanResources.HRStaff.StaffKey = HumanResources.HRStaffPosition.StaffMemberId INNER JOIN
                         HumanResources.HRBuilding ON HumanResources.HRStaffPosition.BuildingKey = HumanResources.HRBuilding.BuildingKey INNER JOIN
                         HumanResources.HREmployeeType ON 
                         HumanResources.HRStaffPosition.EmployeeTypeKey = HumanResources.HREmployeeType.EmployeeTypeLinkId INNER JOIN
                         HumanResources.HRJobTitle ON HumanResources.HRStaffPosition.JobTitleKey = HumanResources.HRJobTitle.JobTitleKey
' 

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsEmail].[StuadentEmailAccount]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [LcpsEmail].[StuadentEmailAccount]
AS
SELECT        Students.Student.StudentKey, Students.Student.FirstName, Students.Student.MiddleInitial, Students.Student.LastName, Students.Student.Gender AS GenderVal, 
                         Students.Student.Birthdate, Students.Student.StudentId, Students.Student.Status AS StatusVal, Students.Student.SchoolYear, LcpsEmail.EmailAccount.EmailAddress, 
                         LcpsEmail.EmailAccount.InitialPassword, LcpsEmail.EmailAccount.RecordId, Students.Student.InstructionalLevelKey, Students.Student.BuildingKey, 
                         HumanResources.HRBuilding.Name AS BuildingName, Students.InstructionalLevel.InstructionalLevelName
FROM            LcpsEmail.EmailAccount INNER JOIN
                         Students.Student ON LcpsEmail.EmailAccount.UserLink = Students.Student.StudentKey INNER JOIN
                         HumanResources.HRBuilding ON Students.Student.BuildingKey = HumanResources.HRBuilding.BuildingKey INNER JOIN
                         Students.InstructionalLevel ON Students.Student.InstructionalLevelKey = Students.InstructionalLevel.InstructionalLevelKey
' 
