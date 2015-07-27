namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MvcControllerACL006 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Infrastructure.MvcControllerACL", "IX_ControllerAction");
        }
        
        public override void Down()
        {
            CreateIndex("Infrastructure.MvcControllerACL", new[] { "Area", "Controller", "Action", "RoleId" }, unique: true, name: "IX_ControllerAction");
        }
    }
}
