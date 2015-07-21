namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filter004 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Filters.StaffFilterClause", "SortIndex", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Filters.StaffFilterClause", "SortIndex");
        }
    }
}
