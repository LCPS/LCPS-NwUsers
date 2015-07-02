namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HREmplioyeeTypeSchema01 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.HREmployeeTypes", newName: "HREmployeeType");
            MoveTable(name: "dbo.HREmployeeType", newSchema: "HumanResources");
        }
        
        public override void Down()
        {
            MoveTable(name: "HumanResources.HREmployeeType", newSchema: "dbo");
            RenameTable(name: "dbo.HREmployeeType", newName: "HREmployeeTypes");
        }
    }
}
