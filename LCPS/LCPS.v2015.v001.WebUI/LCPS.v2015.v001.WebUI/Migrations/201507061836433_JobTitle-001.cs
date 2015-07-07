namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTitle001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "HumanResources.HRJobTitle",
                c => new
                    {
                        JobTitleKey = c.Guid(nullable: false),
                        EmployeeTypeLinkId = c.Guid(nullable: false),
                        JobTitleId = c.String(nullable: false, maxLength: 30),
                        JobTitleName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.JobTitleKey)
                .ForeignKey("HumanResources.HREmployeeType", t => t.EmployeeTypeLinkId, cascadeDelete: true)
                .Index(t => new { t.EmployeeTypeLinkId, t.JobTitleId }, unique: true, name: "IX_JobTitleName");
            
        }
        
        public override void Down()
        {
            DropForeignKey("HumanResources.HRJobTitle", "EmployeeTypeLinkId", "HumanResources.HREmployeeType");
            DropIndex("HumanResources.HRJobTitle", "IX_JobTitleName");
            DropTable("HumanResources.HRJobTitle");
        }
    }
}
