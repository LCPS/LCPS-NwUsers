namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRSchema03 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationBases", newName: "ApplicationBase");
            MoveTable(name: "dbo.ApplicationBase", newSchema: "Anvil");
        }
        
        public override void Down()
        {
            MoveTable(name: "Anvil.ApplicationBase", newSchema: "dbo");
            RenameTable(name: "dbo.ApplicationBase", newName: "ApplicationBases");
        }
    }
}
