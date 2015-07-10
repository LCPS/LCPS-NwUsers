namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class INSLVL001 : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "HumanResources.InstructionalLevel", newSchema: "Students");
        }
        
        public override void Down()
        {
            MoveTable(name: "Students.InstructionalLevel", newSchema: "HumanResources");
        }
    }
}
