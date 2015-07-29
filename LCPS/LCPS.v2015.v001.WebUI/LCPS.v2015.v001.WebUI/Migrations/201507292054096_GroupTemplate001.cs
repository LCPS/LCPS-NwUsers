namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupTemplate001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "LcpsLdap.GroupTemplate",
                c => new
                    {
                        GroupId = c.Guid(nullable: false),
                        TemplateName = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.GroupId)
                .Index(t => t.TemplateName, unique: true, name: "IX_GroupTemplateName");
            
        }
        
        public override void Down()
        {
            DropIndex("LcpsLdap.GroupTemplate", "IX_GroupTemplateName");
            DropTable("LcpsLdap.GroupTemplate");
        }
    }
}
