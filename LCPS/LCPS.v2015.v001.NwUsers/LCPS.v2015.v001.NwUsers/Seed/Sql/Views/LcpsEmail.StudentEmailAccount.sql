
/****** Object:  View [LcpsEmail].[StudentEmailAccount]    Script Date: 7/31/2015 9:30:01 AM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsEmail].[StudentEmailAccount]'))
DROP VIEW [LcpsEmail].[StudentEmailAccount]

/****** Object:  View [LcpsEmail].[StudentEmailAccount]    Script Date: 7/31/2015 9:30:01 AM ******/
SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsEmail].[StudentEmailAccount]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [LcpsEmail].[StudentEmailAccount]
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

