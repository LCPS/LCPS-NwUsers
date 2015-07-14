namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DynamicQueryClauseField001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Filters.DynamicQuery",
                c => new
                    {
                        QueryId = c.Guid(nullable: false),
                        AntecedentId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false, maxLength: 2048),
                    })
                .PrimaryKey(t => t.QueryId)
                .Index(t => new { t.AntecedentId, t.Name }, unique: true, name: "IX_QueryName");
            
            CreateTable(
                "Filtering.DynamicQueryClauseField",
                c => new
                    {
                        FieldId = c.Guid(nullable: false),
                        QueryId = c.Guid(nullable: false),
                        ClauseId = c.Guid(nullable: false),
                        Include = c.Boolean(nullable: false),
                        Conjunction = c.Int(nullable: false),
                        FieldName = c.String(nullable: false, maxLength: 75),
                        Operator = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FieldId)
                .Index(t => new { t.FieldId, t.QueryId, t.FieldName }, unique: true, name: "IX_QueryClauseField");
            
            CreateTable(
                "Filtering.DynamicQueryClause",
                c => new
                    {
                        ClauseId = c.Guid(nullable: false),
                        QueryId = c.Guid(nullable: false),
                        ClauseConjunction = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClauseId)
                .ForeignKey("Filters.DynamicQuery", t => t.ClauseId)
                .Index(t => new { t.ClauseId, t.QueryId }, unique: true, name: "IX_QueryClause");
            
        }
        
        public override void Down()
        {
            DropForeignKey("Filtering.DynamicQueryClause", "ClauseId", "Filters.DynamicQuery");
            DropIndex("Filtering.DynamicQueryClause", "IX_QueryClause");
            DropIndex("Filtering.DynamicQueryClauseField", "IX_QueryClauseField");
            DropIndex("Filters.DynamicQuery", "IX_QueryName");
            DropTable("Filtering.DynamicQueryClause");
            DropTable("Filtering.DynamicQueryClauseField");
            DropTable("Filters.DynamicQuery");
        }
    }
}
