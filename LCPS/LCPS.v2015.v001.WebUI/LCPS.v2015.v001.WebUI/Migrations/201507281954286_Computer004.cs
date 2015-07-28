namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Computer004 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Computers.ComputerInfo", "OSServicePack", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("Computers.ComputerInfo", "OSServicePack", c => c.String(nullable: false, maxLength: 256));
        }
    }
}
