namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MvcControllerACL002 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Infrastructure.ControllerActionAccess", "IX_ControllerAction");
            AddColumn("Infrastructure.ControllerActionAccess", "RoleId", c => c.Guid(nullable: false));
            CreateIndex("Infrastructure.ControllerActionAccess", new[] { "Area", "Controller", "Action", "RoleId" }, unique: true, name: "IX_ControllerAction");
            DropColumn("Infrastructure.ControllerActionAccess", "RoleName");
        }
        
        public override void Down()
        {
            AddColumn("Infrastructure.ControllerActionAccess", "RoleName", c => c.String(maxLength: 35));
            DropIndex("Infrastructure.ControllerActionAccess", "IX_ControllerAction");
            DropColumn("Infrastructure.ControllerActionAccess", "RoleId");
            CreateIndex("Infrastructure.ControllerActionAccess", new[] { "Area", "Controller", "Action", "RoleName" }, unique: true, name: "IX_ControllerAction");
        }
    }
}
