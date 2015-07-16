namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapTemplates007 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "HumanResources.HRStaffFilterClause", newName: "StaffFilterClause");
            MoveTable(name: "HumanResources.StaffFilterClause", newSchema: "Filters");
        }
        
        public override void Down()
        {
            MoveTable(name: "Filters.StaffFilterClause", newSchema: "HumanResources");
            RenameTable(name: "HumanResources.StaffFilterClause", newName: "HRStaffFilterClause");
        }
    }
}
