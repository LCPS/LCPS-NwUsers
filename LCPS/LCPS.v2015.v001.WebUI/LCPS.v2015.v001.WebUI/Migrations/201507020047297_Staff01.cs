namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Staff01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HRStaffs",
                c => new
                    {
                        StaffLinkId = c.Guid(nullable: false),
                        StaffId = c.String(nullable: false, maxLength: 25),
                        FirstName = c.String(nullable: false, maxLength: 75),
                        MiddleInitial = c.String(maxLength: 3),
                        LastName = c.String(nullable: false, maxLength: 75),
                        Gender = c.Int(nullable: false),
                        Birthdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StaffLinkId)
                .Index(t => t.StaffId, unique: true, name: "IX_StaffID");
            
            CreateTable(
                "dbo.HRStaffPositions",
                c => new
                    {
                        StaffPositionLinkId = c.Guid(nullable: false),
                        StaffLinkId = c.Guid(nullable: false),
                        BuildingId = c.Guid(nullable: false),
                        EmployeeTypeId = c.Guid(nullable: false),
                        JobTitleId = c.Guid(nullable: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StaffPositionLinkId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.HRStaffs", "IX_StaffID");
            DropTable("dbo.HRStaffPositions");
            DropTable("dbo.HRStaffs");
        }
    }
}
