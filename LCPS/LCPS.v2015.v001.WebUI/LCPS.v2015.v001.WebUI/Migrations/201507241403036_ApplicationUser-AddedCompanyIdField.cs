namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserAddedCompanyIdField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CompanyId", c => c.String(maxLength: 25));
            AddColumn("dbo.AspNetUsers", "Password", c => c.String(maxLength: 25));
            AddColumn("dbo.AspNetUsers", "ConfirmPassword", c => c.String(maxLength: 25));
            AddColumn("dbo.AspNetUsers", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.AspNetRoles", "RoleType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "RoleType", c => c.Int());
            DropColumn("dbo.AspNetUsers", "Discriminator");
            DropColumn("dbo.AspNetUsers", "ConfirmPassword");
            DropColumn("dbo.AspNetUsers", "Password");
            DropColumn("dbo.AspNetUsers", "CompanyId");
        }
    }
}
