namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filter0041 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Filters.StudentFilterClause",
                c => new
                    {
                        StaffFilterClauseId = c.Guid(nullable: false),
                        FilterId = c.Guid(nullable: false),
                        SortIndex = c.Int(nullable: false),
                        ClauseConjunction = c.Int(nullable: false),
                        BuildingInclude = c.Boolean(nullable: false),
                        BuildingConjunction = c.Int(nullable: false),
                        BuildingOperator = c.Int(nullable: false),
                        BuildingValue = c.Guid(nullable: false),
                        InstructionalLevelInclude = c.Boolean(nullable: false),
                        InstructionalLevelConjunction = c.Int(nullable: false),
                        InstructionalLevelOperator = c.Int(nullable: false),
                        InstructionalLevelValue = c.Guid(nullable: false),
                        StatusInclude = c.Boolean(nullable: false),
                        StatusConjunction = c.Int(nullable: false),
                        StatusOperator = c.Int(nullable: false),
                        StatusValue = c.Int(nullable: false),
                        NameInclude = c.Boolean(nullable: false),
                        NameConjunction = c.Int(nullable: false),
                        NameOperator = c.Int(nullable: false),
                        NameValue = c.String(),
                        StudentIdInclude = c.Boolean(nullable: false),
                        StudentIdConjunction = c.Int(nullable: false),
                        StudentIdOperator = c.Int(nullable: false),
                        StudentIdValue = c.String(),
                    })
                .PrimaryKey(t => t.StaffFilterClauseId)
                .ForeignKey("Filters.MemberFilter", t => t.FilterId, cascadeDelete: true)
                .Index(t => t.FilterId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Filters.StudentFilterClause", "FilterId", "Filters.MemberFilter");
            DropIndex("Filters.StudentFilterClause", new[] { "FilterId" });
            DropTable("Filters.StudentFilterClause");
        }
    }
}
