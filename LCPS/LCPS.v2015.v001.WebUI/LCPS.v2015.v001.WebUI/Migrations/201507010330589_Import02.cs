namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Import02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportItems", "SessionId", c => c.Guid(nullable: false));
            AddColumn("dbo.ImportItems", "Description", c => c.String());
            AddColumn("dbo.ImportSessions", "FieldNames", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportSessions", "FieldNames");
            DropColumn("dbo.ImportItems", "Description");
            DropColumn("dbo.ImportItems", "SessionId");
        }
    }
}
