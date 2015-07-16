namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapTemplates009 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Filters.StaffFilterClause", "OUTemplateId", c => c.Guid(nullable: false));
            CreateIndex("Filters.StaffFilterClause", "OUTemplateId");
            AddForeignKey("Filters.StaffFilterClause", "OUTemplateId", "LcpsLdap.OUTemplate", "OUId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Filters.StaffFilterClause", "OUTemplateId", "LcpsLdap.OUTemplate");
            DropIndex("Filters.StaffFilterClause", new[] { "OUTemplateId" });
            DropColumn("Filters.StaffFilterClause", "OUTemplateId");
        }
    }
}
