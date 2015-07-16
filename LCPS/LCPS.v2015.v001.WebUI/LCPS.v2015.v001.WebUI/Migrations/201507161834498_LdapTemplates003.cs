namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapTemplates003 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "HumanResources.HRStaffFilterClause",
                c => new
                    {
                        StaffFilterClauseId = c.Guid(nullable: false),
                        Antecedentid = c.Guid(nullable: false),
                        ClauseConjunction = c.Int(nullable: false),
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
                .PrimaryKey(t => t.StaffFilterClauseId);
            
        }
        
        public override void Down()
        {
            DropTable("HumanResources.HRStaffFilterClause");
        }
    }
}
