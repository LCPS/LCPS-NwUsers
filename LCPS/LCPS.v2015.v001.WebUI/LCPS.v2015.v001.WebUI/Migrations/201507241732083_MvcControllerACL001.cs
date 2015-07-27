namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MvcControllerACL001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Infrastructure.ControllerActionAccess",
                c => new
                    {
                        RecordId = c.Guid(nullable: false),
                        Area = c.String(maxLength: 35),
                        Controller = c.String(maxLength: 35),
                        Action = c.String(maxLength: 35),
                        RoleName = c.String(maxLength: 35),
                    })
                .PrimaryKey(t => t.RecordId)
                .Index(t => new { t.Area, t.Controller, t.Action, t.RoleName }, unique: true, name: "IX_ControllerAction");
            
        }
        
        public override void Down()
        {
            DropIndex("Infrastructure.ControllerActionAccess", "IX_ControllerAction");
            DropTable("Infrastructure.ControllerActionAccess");
        }
    }
}
