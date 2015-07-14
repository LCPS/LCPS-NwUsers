namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicQueryClauseField002 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Filtering.DynamicQueryClauseField", "ClauseId");
            AddForeignKey("Filtering.DynamicQueryClauseField", "QueryId", "Filters.DynamicQuery", "QueryId", cascadeDelete: true);
            AddForeignKey("Filtering.DynamicQueryClauseField", "ClauseId", "Filtering.DynamicQueryClause", "ClauseId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Filtering.DynamicQueryClauseField", "ClauseId", "Filtering.DynamicQueryClause");
            DropForeignKey("Filtering.DynamicQueryClauseField", "QueryId", "Filters.DynamicQuery");
            DropIndex("Filtering.DynamicQueryClauseField", new[] { "ClauseId" });
        }
    }
}
