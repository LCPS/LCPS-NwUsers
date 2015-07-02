namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Import03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportItems", "EntryDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportItems", "EntryDate");
        }
    }
}
