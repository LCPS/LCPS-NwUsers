namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicQuery001 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Filtering.DynamicQueryClauseField", "QueryId", "Filters.DynamicQuery");
            DropForeignKey("Filtering.DynamicQueryClause", "ClauseId", "Filters.DynamicQuery");
            DropForeignKey("Filtering.DynamicQueryClauseField", "ClauseId", "Filtering.DynamicQueryClause");
            DropIndex("Filtering.DynamicQueryClauseField", new[] { "ClauseId" });
        }
        
        public override void Down()
        {
            CreateIndex("Filtering.DynamicQueryClauseField", "ClauseId");
            AddForeignKey("Filtering.DynamicQueryClauseField", "ClauseId", "Filtering.DynamicQueryClause", "ClauseId", cascadeDelete: true);
            AddForeignKey("Filtering.DynamicQueryClause", "ClauseId", "Filters.DynamicQuery", "QueryId");
            AddForeignKey("Filtering.DynamicQueryClauseField", "QueryId", "Filters.DynamicQuery", "QueryId", cascadeDelete: true);
        }
    }
}
