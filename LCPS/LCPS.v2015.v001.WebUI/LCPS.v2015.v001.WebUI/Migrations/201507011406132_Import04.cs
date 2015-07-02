namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Import04 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportSessions", "DetailMode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportSessions", "DetailMode");
        }
    }
}
