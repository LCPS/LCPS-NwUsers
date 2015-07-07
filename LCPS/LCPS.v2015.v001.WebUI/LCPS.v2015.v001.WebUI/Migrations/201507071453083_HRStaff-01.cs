namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRStaff01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "HumanResources.HRStaff",
                c => new
                    {
                        StaffKey = c.Guid(nullable: false),
                        StaffId = c.String(nullable: false, maxLength: 25),
                        FirstName = c.String(nullable: false, maxLength: 75),
                        MiddleInitial = c.String(maxLength: 3),
                        LastName = c.String(nullable: false, maxLength: 75),
                        StaffEmail = c.String(maxLength: 256),
                        Gender = c.Int(nullable: false),
                        Birthdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StaffKey)
                .Index(t => t.StaffId, unique: true, name: "IX_StaffID");
            
        }
        
        public override void Down()
        {
            DropIndex("HumanResources.HRStaff", "IX_StaffID");
            DropTable("HumanResources.HRStaff");
        }
    }
}
