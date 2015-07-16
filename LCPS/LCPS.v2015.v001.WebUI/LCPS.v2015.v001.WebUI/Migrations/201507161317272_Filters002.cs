namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filters002 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("HumanResources.StaffClauseGroup", "StaffGroupId", "HumanResources.DynamicStaffGroup");
            DropIndex("HumanResources.StaffClauseGroup", "IX_StaffClause");
            DropIndex("HumanResources.DynamicStaffGroup", "IX_DynamicGroupName");
            DropTable("HumanResources.StaffClauseGroup");
            DropTable("HumanResources.DynamicStaffGroup");
        }
        
        public override void Down()
        {
            CreateTable(
                "HumanResources.DynamicStaffGroup",
                c => new
                    {
                        DynamicGroupId = c.Guid(nullable: false),
                        GroupName = c.String(nullable: false, maxLength: 128),
                        Description = c.String(maxLength: 2048),
                    })
                .PrimaryKey(t => t.DynamicGroupId);
            
            CreateTable(
                "HumanResources.StaffClauseGroup",
                c => new
                    {
                        RecordId = c.Guid(nullable: false),
                        StaffGroupId = c.Guid(nullable: false),
                        SortIndex = c.Int(nullable: false),
                        GroupConjunction = c.Int(nullable: false),
                        BuildingConjunction = c.Int(nullable: false),
                        BuildingOperator = c.Int(nullable: false),
                        Building = c.Guid(nullable: false),
                        EmployeeTypeConjunction = c.Int(nullable: false),
                        EmployeeTypeOperator = c.Int(nullable: false),
                        EmployeeType = c.Guid(nullable: false),
                        JobTitleConjunction = c.Int(nullable: false),
                        JobTitleOperator = c.Int(nullable: false),
                        JobTitle = c.Guid(nullable: false),
                        StaffConjunction = c.Int(nullable: false),
                        StaffOperator = c.Int(nullable: false),
                        Staff = c.Guid(nullable: false),
                        StatusConjunction = c.Int(nullable: false),
                        StatusOperator = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        YearConjunction = c.Int(nullable: false),
                        YearOperator = c.Int(nullable: false),
                        Year = c.String(),
                    })
                .PrimaryKey(t => t.RecordId);
            
            CreateIndex("HumanResources.DynamicStaffGroup", "GroupName", unique: true, name: "IX_DynamicGroupName");
            CreateIndex("HumanResources.StaffClauseGroup", new[] { "StaffGroupId", "Building", "EmployeeType", "JobTitle" }, unique: true, name: "IX_StaffClause");
            AddForeignKey("HumanResources.StaffClauseGroup", "StaffGroupId", "HumanResources.DynamicStaffGroup", "DynamicGroupId", cascadeDelete: true);
        }
    }
}
