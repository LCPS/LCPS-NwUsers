namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRPositionFK02 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.HRStaffPositions", "BuildingId");
            CreateIndex("dbo.HRStaffPositions", "EmployeeTypeId");
            CreateIndex("dbo.HRStaffPositions", "JobTitleId");
            AddForeignKey("dbo.HRStaffPositions", "BuildingId", "dbo.HRBuildings", "BuildingKey", cascadeDelete: false);
            AddForeignKey("dbo.HRStaffPositions", "EmployeeTypeId", "dbo.HREmployeeTypes", "EmployeeTypeLinkId", cascadeDelete: false);
            AddForeignKey("dbo.HRStaffPositions", "JobTitleId", "dbo.HRJobTitles", "RecordId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HRStaffPositions", "JobTitleId", "dbo.HRJobTitles");
            DropForeignKey("dbo.HRStaffPositions", "EmployeeTypeId", "dbo.HREmployeeTypes");
            DropForeignKey("dbo.HRStaffPositions", "BuildingId", "dbo.HRBuildings");
            DropIndex("dbo.HRStaffPositions", new[] { "JobTitleId" });
            DropIndex("dbo.HRStaffPositions", new[] { "EmployeeTypeId" });
            DropIndex("dbo.HRStaffPositions", new[] { "BuildingId" });
        }
    }
}
