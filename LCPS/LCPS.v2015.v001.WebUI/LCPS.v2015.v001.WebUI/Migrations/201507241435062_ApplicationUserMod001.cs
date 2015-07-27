namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserMod001 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AvatarData", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AvatarData");
        }
    }
}
