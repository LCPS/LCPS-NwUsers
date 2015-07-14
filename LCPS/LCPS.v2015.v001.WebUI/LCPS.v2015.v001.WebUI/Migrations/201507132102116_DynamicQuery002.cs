namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicQuery002 : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("Filtering.DynamicQueryClause", "QueryId", "Filters.DynamicQuery", "QueryId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Filtering.DynamicQueryClause", "QueryId", "Filters.DynamicQuery");
        }
    }
}
