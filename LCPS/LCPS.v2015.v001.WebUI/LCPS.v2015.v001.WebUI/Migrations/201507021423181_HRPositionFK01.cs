namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRPositionFK01 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.HRStaffPositions", "StaffLinkId");
            AddForeignKey("dbo.HRStaffPositions", "StaffLinkId", "dbo.HRStaffs", "StaffLinkId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HRStaffPositions", "StaffLinkId", "dbo.HRStaffs");
            DropIndex("dbo.HRStaffPositions", new[] { "StaffLinkId" });
        }
    }
}
