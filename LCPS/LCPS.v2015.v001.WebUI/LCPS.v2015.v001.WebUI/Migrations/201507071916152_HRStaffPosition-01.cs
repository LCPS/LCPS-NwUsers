namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRStaffPosition01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HRStaffPositions",
                c => new
                    {
                        PositionKey = c.Guid(nullable: false),
                        StaffMemberId = c.Guid(nullable: false),
                        BuildingKey = c.Guid(nullable: false),
                        EmployeeTypeKey = c.Guid(nullable: false),
                        JobTitleKey = c.Guid(nullable: false),
                        Active = c.Boolean(nullable: false),
                        FiscalYear = c.String(),
                    })
                .PrimaryKey(t => t.PositionKey)
                .Index(t => new { t.StaffMemberId, t.BuildingKey, t.EmployeeTypeKey, t.JobTitleKey }, unique: true, name: "IX_Position");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.HRStaffPositions", "IX_Position");
            DropTable("dbo.HRStaffPositions");
        }
    }
}
