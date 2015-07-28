namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Anvil.ApplicationBase",
                c => new
                    {
                        RecordId = c.Guid(nullable: false),
                        AppName = c.String(nullable: false, maxLength: 128),
                        Title = c.String(nullable: false, maxLength: 128),
                        MissionStatement = c.String(),
                        SMTPServer = c.String(maxLength: 128),
                        SMTPDomain = c.String(maxLength: 128),
                        SMPTPPort = c.Int(nullable: false),
                        SMTPEnableSSL = c.Boolean(nullable: false),
                        SMTPUserName = c.String(maxLength: 75),
                        SMTPPassword = c.String(),
                        PWDRequiredLength = c.Int(nullable: false),
                        PWDRequireNonLetterOrDigit = c.Boolean(nullable: false),
                        PWDRequireDigit = c.Boolean(nullable: false),
                        PWDRequireLowercase = c.Boolean(nullable: false),
                        PWDRequireUppercase = c.Boolean(nullable: false),
                        PWDUserLockoutEnabledByDefault = c.Boolean(nullable: false),
                        PWDDefaultAccountLockoutTimeSpan = c.Int(nullable: false),
                        PWDMaxFailedAccessAttemptsBeforeLockout = c.Int(nullable: false),
                        LDAPDomainFQN = c.String(maxLength: 128),
                        LDAPDomain = c.String(maxLength: 128),
                        LDAPUserName = c.String(maxLength: 128),
                        LDAPPassword = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RecordId)
                .Index(t => t.AppName, unique: true, name: "ApplicationName_IX");
            
            CreateTable(
                "HumanResources.HRBuilding",
                c => new
                    {
                        BuildingKey = c.Guid(nullable: false),
                        BuildingId = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.BuildingKey)
                .Index(t => t.BuildingId, unique: true);
            
            CreateTable(
                "LcpsEmail.EmailAccount",
                c => new
                    {
                        RecordId = c.Guid(nullable: false),
                        UserLink = c.Guid(nullable: false),
                        EmailAddress = c.String(),
                        InitialPassword = c.String(),
                    })
                .PrimaryKey(t => t.RecordId);
            
            CreateTable(
                "HumanResources.HREmployeeType",
                c => new
                    {
                        EmployeeTypeLinkId = c.Guid(nullable: false),
                        EmployeeTypeId = c.String(nullable: false, maxLength: 15),
                        EmployeeTypeName = c.String(nullable: false, maxLength: 128),
                        Category = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeTypeLinkId)
                .Index(t => t.EmployeeTypeId, unique: true, name: "EmployeeTypeId_IX")
                .Index(t => t.EmployeeTypeName, unique: true, name: "EmployeeTypeName_IX");
            
            CreateTable(
                "Importing.ImportItem",
                c => new
                    {
                        ImportItemId = c.Guid(nullable: false),
                        SessionId = c.Guid(nullable: false),
                        Comment = c.String(),
                        Description = c.String(),
                        EntryDate = c.DateTime(nullable: false),
                        ImportStatus = c.Int(nullable: false),
                        SerializedData = c.Binary(),
                        EntityStatus = c.Int(nullable: false),
                        LineIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImportItemId)
                .ForeignKey("Importing.ImportSession", t => t.SessionId, cascadeDelete: true)
                .Index(t => t.SessionId);
            
            CreateTable(
                "Importing.ImportSession",
                c => new
                    {
                        SessionId = c.Guid(nullable: false),
                        SessionDate = c.DateTime(nullable: false),
                        Author = c.String(),
                        FullAssemblyTypeName = c.String(),
                        AddIfNotExist = c.Boolean(nullable: false),
                        UpdateIfExists = c.Boolean(nullable: false),
                        Delimiter = c.String(),
                        ImportFileContents = c.Binary(),
                        ViewTitle = c.String(),
                        Area = c.String(),
                        Controller = c.String(),
                        ViewLayoutPath = c.String(),
                        Action = c.String(),
                    })
                .PrimaryKey(t => t.SessionId);
            
            CreateTable(
                "Students.InstructionalLevel",
                c => new
                    {
                        InstructionalLevelKey = c.Guid(nullable: false),
                        InstructionalLevelId = c.String(nullable: false, maxLength: 25),
                        InstructionalLevelName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.InstructionalLevelKey)
                .Index(t => t.InstructionalLevelId, unique: true, name: "IX_ILevelId")
                .Index(t => t.InstructionalLevelName, unique: true, name: "IX_ILevelName");
            
            CreateTable(
                "HumanResources.HRJobTitle",
                c => new
                    {
                        JobTitleKey = c.Guid(nullable: false),
                        JobTitleId = c.String(nullable: false, maxLength: 30),
                        JobTitleName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.JobTitleKey)
                .Index(t => t.JobTitleId, unique: true, name: "IX_JobTitleName");
            
            CreateTable(
                "Filters.MemberFilter",
                c => new
                    {
                        FilterId = c.Guid(nullable: false),
                        AntecedentId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 128),
                        Category = c.Int(nullable: false),
                        FilterClass = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FilterId)
                .Index(t => new { t.AntecedentId, t.Title }, unique: true, name: "IX_FilterName");
            
            CreateTable(
                "Infrastructure.MvcControllerACL",
                c => new
                    {
                        RecordId = c.Guid(nullable: false),
                        Area = c.String(maxLength: 35),
                        Controller = c.String(maxLength: 35),
                        Action = c.String(maxLength: 35),
                        RoleId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RecordId);
            
            CreateTable(
                "LcpsLdap.OUTemplate",
                c => new
                    {
                        OUId = c.Guid(nullable: false),
                        TemplateName = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.OUId)
                .Index(t => t.TemplateName, unique: true, name: "IX_OUTemplateName");
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "HumanResources.HRRoom",
                c => new
                    {
                        RoomKey = c.Guid(nullable: false),
                        BuildingId = c.Guid(nullable: false),
                        RoomNumber = c.String(nullable: false, maxLength: 128),
                        RoomId = c.String(nullable: false, maxLength: 128),
                        RoomType = c.Int(nullable: false),
                        PrimaryOccupant = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.RoomKey)
                .Index(t => new { t.BuildingId, t.RoomNumber }, unique: true, name: "IX_RoomName");
            
            CreateTable(
                "Filters.StaffFilterClause",
                c => new
                    {
                        StaffFilterClauseId = c.Guid(nullable: false),
                        FilterId = c.Guid(nullable: false),
                        SortIndex = c.Int(nullable: false),
                        ClauseConjunction = c.Int(nullable: false),
                        BuildingInclude = c.Boolean(nullable: false),
                        BuildingConjunction = c.Int(nullable: false),
                        BuildingOperator = c.Int(nullable: false),
                        BuildingValue = c.Guid(nullable: false),
                        EmployeeTypeInclude = c.Boolean(nullable: false),
                        EmployeeTypeConjunction = c.Int(nullable: false),
                        EmployeeTypeOperator = c.Int(nullable: false),
                        EmployeeTypeValue = c.Guid(nullable: false),
                        JobTitleInclude = c.Boolean(nullable: false),
                        JobTitleConjunction = c.Int(nullable: false),
                        JobTitleOperator = c.Int(nullable: false),
                        JobTitleValue = c.Guid(nullable: false),
                        StatusInclude = c.Boolean(nullable: false),
                        StatusConjunction = c.Int(nullable: false),
                        StatusOperator = c.Int(nullable: false),
                        StatusValue = c.Int(nullable: false),
                        LastNameInclude = c.Boolean(nullable: false),
                        LastNameConjunction = c.Int(nullable: false),
                        LastNameOperator = c.Int(nullable: false),
                        LastNameValue = c.String(),
                        StaffIdInclude = c.Boolean(nullable: false),
                        StaffIdConjunction = c.Int(nullable: false),
                        StaffIdOperator = c.Int(nullable: false),
                        StaffIdValue = c.String(),
                        FiscalYearInclude = c.Boolean(nullable: false),
                        FiscalYearConjunction = c.Int(nullable: false),
                        FiscalYearOperator = c.Int(nullable: false),
                        FiscalYearValue = c.String(),
                    })
                .PrimaryKey(t => t.StaffFilterClauseId);
            
            CreateTable(
                "HumanResources.HRStaff",
                c => new
                    {
                        StaffKey = c.Guid(nullable: false),
                        StaffId = c.String(nullable: false, maxLength: 25),
                        FirstName = c.String(nullable: false, maxLength: 75),
                        MiddleInitial = c.String(maxLength: 3),
                        LastName = c.String(nullable: false, maxLength: 75),
                        Gender = c.Int(nullable: false),
                        Birthdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StaffKey)
                .Index(t => t.StaffId, unique: true, name: "IX_StaffID");
            
            CreateTable(
                "HumanResources.HRStaffPosition",
                c => new
                    {
                        PositionKey = c.Guid(nullable: false),
                        StaffMemberId = c.Guid(nullable: false),
                        BuildingKey = c.Guid(nullable: false),
                        EmployeeTypeKey = c.Guid(nullable: false),
                        JobTitleKey = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        FiscalYear = c.String(),
                    })
                .PrimaryKey(t => t.PositionKey)
                .Index(t => new { t.StaffMemberId, t.BuildingKey, t.EmployeeTypeKey, t.JobTitleKey }, unique: true, name: "IX_Position");
            
            CreateTable(
                "Filters.StudentFilterClause",
                c => new
                    {
                        StudentFilterClauseId = c.Guid(nullable: false),
                        FilterId = c.Guid(nullable: false),
                        SortIndex = c.Int(nullable: false),
                        ClauseConjunction = c.Int(nullable: false),
                        BuildingInclude = c.Boolean(nullable: false),
                        BuildingConjunction = c.Int(nullable: false),
                        BuildingOperator = c.Int(nullable: false),
                        BuildingValue = c.Guid(nullable: false),
                        InstructionalLevelInclude = c.Boolean(nullable: false),
                        InstructionalLevelConjunction = c.Int(nullable: false),
                        InstructionalLevelOperator = c.Int(nullable: false),
                        InstructionalLevelValue = c.Guid(nullable: false),
                        StatusInclude = c.Boolean(nullable: false),
                        StatusConjunction = c.Int(nullable: false),
                        StatusOperator = c.Int(nullable: false),
                        StatusValue = c.Int(nullable: false),
                        NameInclude = c.Boolean(nullable: false),
                        NameConjunction = c.Int(nullable: false),
                        NameOperator = c.Int(nullable: false),
                        NameValue = c.String(),
                        StudentIdInclude = c.Boolean(nullable: false),
                        StudentIdConjunction = c.Int(nullable: false),
                        StudentIdOperator = c.Int(nullable: false),
                        StudentIdValue = c.String(),
                    })
                .PrimaryKey(t => t.StudentFilterClauseId);
            
            CreateTable(
                "Students.Student",
                c => new
                    {
                        StudentKey = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 75),
                        MiddleInitial = c.String(nullable: false, maxLength: 10),
                        LastName = c.String(nullable: false, maxLength: 75),
                        Gender = c.Int(nullable: false),
                        Birthdate = c.DateTime(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 50),
                        InstructionalLevelKey = c.Guid(nullable: false),
                        BuildingKey = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        SchoolYear = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.StudentKey)
                .Index(t => new { t.StudentId, t.InstructionalLevelKey }, unique: true, name: "IX_StudentUnique");
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        AccountStatus = c.Int(nullable: false),
                        AccountVisibility = c.Int(nullable: false),
                        GivenName = c.String(maxLength: 128),
                        SurName = c.String(maxLength: 128),
                        EmailVisibility = c.Int(nullable: false),
                        AvatarContent = c.Binary(),
                        Psuedonym = c.String(maxLength: 128),
                        Birthdate = c.DateTime(nullable: false),
                        Autobiography = c.String(),
                        HomeTownCity = c.String(maxLength: 128),
                        HomeTownState = c.String(maxLength: 2),
                        CompanyId = c.String(maxLength: 25),
                        AvatarData = c.Binary(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("Importing.ImportItem", "SessionId", "Importing.ImportSession");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("Students.Student", "IX_StudentUnique");
            DropIndex("HumanResources.HRStaffPosition", "IX_Position");
            DropIndex("HumanResources.HRStaff", "IX_StaffID");
            DropIndex("HumanResources.HRRoom", "IX_RoomName");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("LcpsLdap.OUTemplate", "IX_OUTemplateName");
            DropIndex("Filters.MemberFilter", "IX_FilterName");
            DropIndex("HumanResources.HRJobTitle", "IX_JobTitleName");
            DropIndex("Students.InstructionalLevel", "IX_ILevelName");
            DropIndex("Students.InstructionalLevel", "IX_ILevelId");
            DropIndex("Importing.ImportItem", new[] { "SessionId" });
            DropIndex("HumanResources.HREmployeeType", "EmployeeTypeName_IX");
            DropIndex("HumanResources.HREmployeeType", "EmployeeTypeId_IX");
            DropIndex("HumanResources.HRBuilding", new[] { "BuildingId" });
            DropIndex("Anvil.ApplicationBase", "ApplicationName_IX");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("Students.Student");
            DropTable("Filters.StudentFilterClause");
            DropTable("HumanResources.HRStaffPosition");
            DropTable("HumanResources.HRStaff");
            DropTable("Filters.StaffFilterClause");
            DropTable("HumanResources.HRRoom");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("LcpsLdap.OUTemplate");
            DropTable("Infrastructure.MvcControllerACL");
            DropTable("Filters.MemberFilter");
            DropTable("HumanResources.HRJobTitle");
            DropTable("Students.InstructionalLevel");
            DropTable("Importing.ImportSession");
            DropTable("Importing.ImportItem");
            DropTable("HumanResources.HREmployeeType");
            DropTable("LcpsEmail.EmailAccount");
            DropTable("HumanResources.HRBuilding");
            DropTable("Anvil.ApplicationBase");
        }
    }
}
