namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HomeFolderTemplates001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "LcpsLdap.HomeFolderTemplate",
                c => new
                    {
                        HomeFolderId = c.Guid(nullable: false),
                        TemplateName = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false, maxLength: 256),
                        HomeFoldePath = c.String(nullable: false, maxLength: 1024),
                        IdField = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HomeFolderId)
                .Index(t => t.TemplateName, unique: true, name: "IX_HomeFolderTemplateName");
            
        }
        
        public override void Down()
        {
            DropIndex("LcpsLdap.HomeFolderTemplate", "IX_HomeFolderTemplateName");
            DropTable("LcpsLdap.HomeFolderTemplate");
        }
    }
}
