namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserAddedCompanyIdField2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Password");
            DropColumn("dbo.AspNetUsers", "ConfirmPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ConfirmPassword", c => c.String());
            AddColumn("dbo.AspNetUsers", "Password", c => c.String());
        }
    }
}
