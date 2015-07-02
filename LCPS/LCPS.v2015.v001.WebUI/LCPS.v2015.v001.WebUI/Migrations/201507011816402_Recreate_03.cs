namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Recreate_03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportSessions", "ViewLayoutPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportSessions", "ViewLayoutPath");
        }
    }
}
