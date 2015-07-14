namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicQuery003 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Filtering.DynamicQueryClause", "ClauseId", "Filters.DynamicQuery");
            DropIndex("Filtering.DynamicQueryClause", "IX_QueryClause");
            DropColumn("Filtering.DynamicQueryClause", "QueryId");
            RenameColumn(table: "Filtering.DynamicQueryClause", name: "ClauseId", newName: "QueryId");
            CreateIndex("Filtering.DynamicQueryClause", new[] { "ClauseId", "QueryId" }, unique: true, name: "IX_QueryClause");
            AddForeignKey("Filtering.DynamicQueryClause", "QueryId", "Filters.DynamicQuery", "QueryId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Filtering.DynamicQueryClause", "QueryId", "Filters.DynamicQuery");
            DropIndex("Filtering.DynamicQueryClause", "IX_QueryClause");
            RenameColumn(table: "Filtering.DynamicQueryClause", name: "QueryId", newName: "ClauseId");
            AddColumn("Filtering.DynamicQueryClause", "QueryId", c => c.Guid(nullable: false));
            CreateIndex("Filtering.DynamicQueryClause", new[] { "ClauseId", "QueryId" }, unique: true, name: "IX_QueryClause");
            AddForeignKey("Filtering.DynamicQueryClause", "ClauseId", "Filters.DynamicQuery", "QueryId");
        }
    }
}
