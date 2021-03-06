

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Students].[StudentRecord]'))
DROP VIEW [Students].[StudentRecord]


SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Students].[StudentRecord]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [Students].[StudentRecord]
AS
SELECT        Students.Student.StudentKey, Students.Student.FirstName, Students.Student.MiddleInitial, Students.Student.LastName, Students.Student.Gender, 
                         Students.Student.Birthdate, Students.Student.StudentId, Students.Student.InstructionalLevelKey, Students.InstructionalLevel.InstructionalLevelId, 
                         Students.InstructionalLevel.InstructionalLevelName, Students.Student.BuildingKey, HumanResources.HRBuilding.BuildingId, HumanResources.HRBuilding.Name, 
                         HumanResources.HRBuilding.Description, Students.Student.Status, Students.Student.SchoolYear
FROM            Students.Student INNER JOIN
                         Students.InstructionalLevel ON Students.Student.InstructionalLevelKey = Students.InstructionalLevel.InstructionalLevelKey INNER JOIN
                         HumanResources.HRBuilding ON Students.Student.BuildingKey = HumanResources.HRBuilding.BuildingKey
' 
