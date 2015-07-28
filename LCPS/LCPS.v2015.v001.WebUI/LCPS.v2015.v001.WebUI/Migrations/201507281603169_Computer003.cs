namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Computer003 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Computers.ArchiveNic",
                c => new
                    {
                        RecordId = c.Guid(nullable: false),
                        ComputerId = c.Guid(nullable: false),
                        Name = c.String(),
                        MacAddress = c.String(),
                        Manufacturer = c.String(),
                    })
                .PrimaryKey(t => t.RecordId);
            
            AddColumn("Computers.ComputerInfo", "UnitNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Computers.ComputerInfo", "UnitNumber");
            DropTable("Computers.ArchiveNic");
        }
    }
}
