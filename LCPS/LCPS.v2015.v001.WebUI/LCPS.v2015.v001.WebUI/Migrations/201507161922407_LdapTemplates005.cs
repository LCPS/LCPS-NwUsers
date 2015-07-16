namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapTemplates005 : DbMigration
    {
        public override void Up()
        {
            AddColumn("HumanResources.HRStaffFilterClause", "FilterId", c => c.Guid(nullable: false));
            CreateIndex("HumanResources.HRStaffFilterClause", "FilterId");
            AddForeignKey("HumanResources.HRStaffFilterClause", "FilterId", "Filters.MemberFilter", "FilterId", cascadeDelete: true);
            DropColumn("HumanResources.HRStaffFilterClause", "Antecedentid");
        }
        
        public override void Down()
        {
            AddColumn("HumanResources.HRStaffFilterClause", "Antecedentid", c => c.Guid(nullable: false));
            DropForeignKey("HumanResources.HRStaffFilterClause", "FilterId", "Filters.MemberFilter");
            DropIndex("HumanResources.HRStaffFilterClause", new[] { "FilterId" });
            DropColumn("HumanResources.HRStaffFilterClause", "FilterId");
        }
    }
}
