namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapTemplates004 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OUTemplateFilterLinks",
                c => new
                    {
                        LinkId = c.Guid(nullable: false),
                        OUId = c.Guid(nullable: false),
                        FilterId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.LinkId)
                .ForeignKey("Filters.MemberFilter", t => t.FilterId, cascadeDelete: true)
                .ForeignKey("LcpsLdap.OUTemplate", t => t.OUId, cascadeDelete: true)
                .Index(t => new { t.OUId, t.FilterId }, unique: true, name: "IX_OUTemplateFilterLink");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OUTemplateFilterLinks", "OUId", "LcpsLdap.OUTemplate");
            DropForeignKey("dbo.OUTemplateFilterLinks", "FilterId", "Filters.MemberFilter");
            DropIndex("dbo.OUTemplateFilterLinks", "IX_OUTemplateFilterLink");
            DropTable("dbo.OUTemplateFilterLinks");
        }
    }
}
