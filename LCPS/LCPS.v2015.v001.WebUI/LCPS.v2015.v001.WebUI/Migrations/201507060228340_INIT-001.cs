namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class INIT001 : DbMigration
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
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 256),
                        RoleType = c.Int(),
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
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("Importing.ImportItem", new[] { "SessionId" });
            DropIndex("HumanResources.HREmployeeType", "EmployeeTypeName_IX");
            DropIndex("HumanResources.HREmployeeType", "EmployeeTypeId_IX");
            DropIndex("Anvil.ApplicationBase", "ApplicationName_IX");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("Importing.ImportSession");
            DropTable("Importing.ImportItem");
            DropTable("HumanResources.HREmployeeType");
            DropTable("Anvil.ApplicationBase");
        }
    }
}
