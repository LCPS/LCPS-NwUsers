namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class STU001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        StudentKey = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 75),
                        MiddleInitial = c.String(nullable: false, maxLength: 10),
                        LastName = c.String(nullable: false, maxLength: 75),
                        Gender = c.Int(nullable: false),
                        Birthdate = c.DateTime(nullable: false),
                        StudentId = c.String(nullable: false, maxLength: 50),
                        InstructionalLevelKey = c.Guid(nullable: false),
                        BuildingKey = c.Guid(nullable: false),
                        Status = c.Int(nullable: false),
                        SchoolYear = c.String(nullable: false, maxLength: 4),
                    })
                .PrimaryKey(t => t.StudentKey)
                .Index(t => new { t.StudentId, t.InstructionalLevelKey }, unique: true, name: "IX_StudentUnique");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Students", "IX_StudentUnique");
            DropTable("dbo.Students");
        }
    }
}
