namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MvcControllerACL003 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Infrastructure.ControllerActionAccess", "IX_ControllerAction");
            AlterColumn("Infrastructure.ControllerActionAccess", "RoleId", c => c.String(maxLength: 128));
            CreateIndex("Infrastructure.ControllerActionAccess", new[] { "Area", "Controller", "Action", "RoleId" }, unique: true, name: "IX_ControllerAction");
        }
        
        public override void Down()
        {
            DropIndex("Infrastructure.ControllerActionAccess", "IX_ControllerAction");
            AlterColumn("Infrastructure.ControllerActionAccess", "RoleId", c => c.Guid(nullable: false));
            CreateIndex("Infrastructure.ControllerActionAccess", new[] { "Area", "Controller", "Action", "RoleId" }, unique: true, name: "IX_ControllerAction");
        }
    }
}
