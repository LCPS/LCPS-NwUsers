namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapTemplates001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "LcpsLdap.OUTemplate",
                c => new
                    {
                        OUId = c.Guid(nullable: false),
                        TemplateName = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.OUId)
                .Index(t => t.TemplateName, unique: true, name: "IX_OUTemplateName");
            
        }
        
        public override void Down()
        {
            DropIndex("LcpsLdap.OUTemplate", "IX_OUTemplateName");
            DropTable("LcpsLdap.OUTemplate");
        }
    }
}
