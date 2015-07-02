namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JobTitle01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HRJobTitles",
                c => new
                    {
                        RecordId = c.Guid(nullable: false),
                        JobTitleId = c.String(nullable: false, maxLength: 15),
                        JobTitleName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.RecordId)
                .ForeignKey("dbo.HREmployeeTypes", t => t.RecordId)
                .Index(t => t.RecordId)
                .Index(t => t.JobTitleId, unique: true, name: "IX_JobTitleName");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HRJobTitles", "RecordId", "dbo.HREmployeeTypes");
            DropIndex("dbo.HRJobTitles", "IX_JobTitleName");
            DropIndex("dbo.HRJobTitles", new[] { "RecordId" });
            DropTable("dbo.HRJobTitles");
        }
    }
}
