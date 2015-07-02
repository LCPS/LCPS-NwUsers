namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Recreate_02 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.HRJobTitles", new[] { "EmployeeTypeLinkId" });
            DropIndex("dbo.HRJobTitles", "IX_JobTitleName");
            CreateIndex("dbo.HRJobTitles", new[] { "EmployeeTypeLinkId", "JobTitleId" }, unique: true, name: "IX_JobTitleName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.HRJobTitles", "IX_JobTitleName");
            CreateIndex("dbo.HRJobTitles", "JobTitleId", unique: true, name: "IX_JobTitleName");
            CreateIndex("dbo.HRJobTitles", "EmployeeTypeLinkId");
        }
    }
}
