namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRSchema02 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.HRBuildings", newName: "HRBuilding");
            RenameTable(name: "dbo.ImportItems", newName: "ImportItem");
            RenameTable(name: "dbo.ImportSessions", newName: "ImportStatus");
            MoveTable(name: "dbo.HRBuilding", newSchema: "HumanResources");
            MoveTable(name: "dbo.ImportItem", newSchema: "Importing");
            MoveTable(name: "dbo.ImportStatus", newSchema: "Importing");
        }
        
        public override void Down()
        {
            MoveTable(name: "Importing.ImportStatus", newSchema: "dbo");
            MoveTable(name: "Importing.ImportItem", newSchema: "dbo");
            MoveTable(name: "HumanResources.HRBuilding", newSchema: "dbo");
            RenameTable(name: "dbo.ImportStatus", newName: "ImportSessions");
            RenameTable(name: "dbo.ImportItem", newName: "ImportItems");
            RenameTable(name: "dbo.HRBuilding", newName: "HRBuildings");
        }
    }
}
