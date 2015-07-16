namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapTemplates006 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OUTemplateFilterLinks", "FilterId", "Filters.MemberFilter");
            DropForeignKey("dbo.OUTemplateFilterLinks", "OUId", "LcpsLdap.OUTemplate");
            DropIndex("dbo.OUTemplateFilterLinks", "IX_OUTemplateFilterLink");
            DropTable("dbo.OUTemplateFilterLinks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OUTemplateFilterLinks",
                c => new
                    {
                        LinkId = c.Guid(nullable: false),
                        OUId = c.Guid(nullable: false),
                        FilterId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.LinkId);
            
            CreateIndex("dbo.OUTemplateFilterLinks", new[] { "OUId", "FilterId" }, unique: true, name: "IX_OUTemplateFilterLink");
            AddForeignKey("dbo.OUTemplateFilterLinks", "OUId", "LcpsLdap.OUTemplate", "OUId", cascadeDelete: true);
            AddForeignKey("dbo.OUTemplateFilterLinks", "FilterId", "Filters.MemberFilter", "FilterId", cascadeDelete: true);
        }
    }
}
