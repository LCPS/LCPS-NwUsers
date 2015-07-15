namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ADS001 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Filters.DynamicQueryClause", "QueryId", "Filters.DynamicQuery");
            DropForeignKey("Filters.DynamicQueryClauseField", "ClauseId", "Filters.DynamicQueryClause");
            DropIndex("Filters.DynamicQuery", "IX_QueryName");
            DropIndex("Filters.DynamicQueryClauseField", "IX_QueryClauseField");
            DropIndex("Filters.DynamicQueryClauseField", new[] { "ClauseId" });
            DropIndex("Filters.DynamicQueryClause", "IX_QueryClause");
            DropIndex("dbo.FilterGroups", "IX_FilterName");
            CreateTable(
                "HumanResources.HRStaffFilterClause",
                c => new
                    {
                        StaffFilterClauseId = c.Guid(nullable: false),
                        StaffFilterId = c.Guid(nullable: false),
                        BuildingInclude = c.Boolean(nullable: false),
                        BuildingConjunction = c.Int(nullable: false),
                        BuildingOperator = c.Int(nullable: false),
                        BuildingValue = c.Guid(nullable: false),
                        EmployeeTypeInclude = c.Boolean(nullable: false),
                        EmployeeTypeConjunction = c.Int(nullable: false),
                        EmployeeTypeOperator = c.Int(nullable: false),
                        EmployeeTypeValue = c.Guid(nullable: false),
                        JobTitleInclude = c.Boolean(nullable: false),
                        JobTitleConjunction = c.Int(nullable: false),
                        JobTitleOperator = c.Int(nullable: false),
                        JobTitleValue = c.Guid(nullable: false),
                        StatusInclude = c.Boolean(nullable: false),
                        StatusConjunction = c.Int(nullable: false),
                        StatusOperator = c.Int(nullable: false),
                        StatusValue = c.Int(nullable: false),
                        LastNameInclude = c.Boolean(nullable: false),
                        LastNameConjunction = c.Int(nullable: false),
                        LastNameOperator = c.Int(nullable: false),
                        LastNameValue = c.String(),
                        StaffIdInclude = c.Boolean(nullable: false),
                        StaffIdConjunction = c.Int(nullable: false),
                        StaffIdOperator = c.Int(nullable: false),
                        StaffIdValue = c.String(),
                        FiscalYearInclude = c.Boolean(nullable: false),
                        FiscalYearConjunction = c.Int(nullable: false),
                        FiscalYearOperator = c.Int(nullable: false),
                        FiscalYearValue = c.String(),
                    })
                .PrimaryKey(t => t.StaffFilterClauseId)
                .ForeignKey("HumanResources.StaffFilter", t => t.StaffFilterId, cascadeDelete: true)
                .Index(t => t.StaffFilterId);
            
            CreateTable(
                "HumanResources.StaffFilter",
                c => new
                    {
                        StaffFilterId = c.Guid(nullable: false),
                        AntecedentId = c.Guid(nullable: false),
                        Title = c.String(maxLength: 128),
                        Description = c.String(maxLength: 128),
                        Category = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StaffFilterId)
                .Index(t => new { t.AntecedentId, t.Title }, unique: true, name: "IX_FilterName");
            
            DropTable("Filters.DynamicQuery");
            DropTable("Filters.DynamicQueryClauseField");
            DropTable("Filters.DynamicQueryClause");
            DropTable("dbo.FilterGroups");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FilterGroups",
                c => new
                    {
                        FilterId = c.Guid(nullable: false),
                        AntecedentId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false, maxLength: 2048),
                    })
                .PrimaryKey(t => t.FilterId);
            
            CreateTable(
                "Filters.DynamicQueryClause",
                c => new
                    {
                        ClauseId = c.Guid(nullable: false),
                        QueryId = c.Guid(nullable: false),
                        ClauseConjunction = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ClauseId);
            
            CreateTable(
                "Filters.DynamicQueryClauseField",
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
                .PrimaryKey(t => t.FieldId);
            
            CreateTable(
                "Filters.DynamicQuery",
                c => new
                    {
                        QueryId = c.Guid(nullable: false),
                        AntecedentId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false, maxLength: 2048),
                    })
                .PrimaryKey(t => t.QueryId);
            
            DropForeignKey("HumanResources.HRStaffFilterClause", "StaffFilterId", "HumanResources.StaffFilter");
            DropIndex("HumanResources.StaffFilter", "IX_FilterName");
            DropIndex("HumanResources.HRStaffFilterClause", new[] { "StaffFilterId" });
            DropTable("HumanResources.StaffFilter");
            DropTable("HumanResources.HRStaffFilterClause");
            CreateIndex("dbo.FilterGroups", "Name", unique: true, name: "IX_FilterName");
            CreateIndex("Filters.DynamicQueryClause", new[] { "ClauseId", "QueryId" }, unique: true, name: "IX_QueryClause");
            CreateIndex("Filters.DynamicQueryClauseField", "ClauseId");
            CreateIndex("Filters.DynamicQueryClauseField", new[] { "FieldId", "QueryId", "FieldName" }, unique: true, name: "IX_QueryClauseField");
            CreateIndex("Filters.DynamicQuery", new[] { "AntecedentId", "Name" }, unique: true, name: "IX_QueryName");
            AddForeignKey("Filters.DynamicQueryClauseField", "ClauseId", "Filters.DynamicQueryClause", "ClauseId", cascadeDelete: true);
            AddForeignKey("Filters.DynamicQueryClause", "QueryId", "Filters.DynamicQuery", "QueryId", cascadeDelete: true);
        }
    }
}
