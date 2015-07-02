namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LcpsEmail03 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "LcpsSecurity.LcpsEmail",
                c => new
                    {
                        EmailId = c.Guid(nullable: false),
                        StaffLinkId = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50),
                        InitialPassword = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.EmailId)
                .ForeignKey("HumanResources.HRStaff", t => t.StaffLinkId, cascadeDelete: true)
                .Index(t => t.StaffLinkId)
                .Index(t => t.Email, unique: true, name: "IX_LCPSEmailAddress");
            
        }
        
        public override void Down()
        {
            DropForeignKey("LcpsSecurity.LcpsEmail", "StaffLinkId", "HumanResources.HRStaff");
            DropIndex("LcpsSecurity.LcpsEmail", "IX_LCPSEmailAddress");
            DropIndex("LcpsSecurity.LcpsEmail", new[] { "StaffLinkId" });
            DropTable("LcpsSecurity.LcpsEmail");
        }
    }
}
