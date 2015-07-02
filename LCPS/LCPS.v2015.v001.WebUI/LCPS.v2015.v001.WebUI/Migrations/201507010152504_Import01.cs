namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Import01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportSessions", "Area", c => c.String());
            AddColumn("dbo.ImportSessions", "Controller", c => c.String());
            AddColumn("dbo.ImportSessions", "Action", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportSessions", "Action");
            DropColumn("dbo.ImportSessions", "Controller");
            DropColumn("dbo.ImportSessions", "Area");
        }
    }
}
