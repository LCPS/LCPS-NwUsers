namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRSchema01 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.HRJobTitles", newName: "HRJobTitle");
            RenameTable(name: "dbo.HRStaffs", newName: "HRStaff");
            RenameTable(name: "dbo.HRStaffPositions", newName: "HRStaffPosition");
            MoveTable(name: "dbo.HRJobTitle", newSchema: "HumanResources");
            MoveTable(name: "dbo.HRStaff", newSchema: "HumanResources");
            MoveTable(name: "dbo.HRStaffPosition", newSchema: "HumanResources");
        }
        
        public override void Down()
        {
            MoveTable(name: "HumanResources.HRStaffPosition", newSchema: "dbo");
            MoveTable(name: "HumanResources.HRStaff", newSchema: "dbo");
            MoveTable(name: "HumanResources.HRJobTitle", newSchema: "dbo");
            RenameTable(name: "dbo.HRStaffPosition", newName: "HRStaffPositions");
            RenameTable(name: "dbo.HRStaff", newName: "HRStaffs");
            RenameTable(name: "dbo.HRJobTitle", newName: "HRJobTitles");
        }
    }
}
