namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Computer002 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Computers.ComputerInfo", "ComputerType", c => c.Int(nullable: false));
            AddColumn("Computers.ComputerInfo", "Description", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            DropColumn("Computers.ComputerInfo", "Description");
            DropColumn("Computers.ComputerInfo", "ComputerType");
        }
    }
}
