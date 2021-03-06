IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[LcpsEmail].[LcpsEmailKey]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [LcpsEmail].[LcpsEmailKey]
AS
SELECT        LcpsEmail.EmailAccount.EmailAddress, Students.Student.StudentKey, HumanResources.HRStaff.StaffKey, Students.Student.FirstName AS StuFirstName, 
                         Students.Student.LastName AS StuLastName, HumanResources.HRStaff.FirstName AS StfFirstName, HumanResources.HRStaff.LastName AS StfLastName
FROM            HumanResources.HRStaff INNER JOIN
                         LcpsEmail.EmailAccount ON HumanResources.HRStaff.StaffKey = LcpsEmail.EmailAccount.UserLink INNER JOIN
                         Students.Student ON LcpsEmail.EmailAccount.UserLink = Students.Student.StudentKey
' 
