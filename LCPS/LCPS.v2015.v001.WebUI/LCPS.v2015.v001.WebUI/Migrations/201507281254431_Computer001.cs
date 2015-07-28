namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Computer001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Computers.ComputerInfo",
                c => new
                    {
                        ComputerId = c.Guid(nullable: false),
                        ComputerName = c.String(nullable: false, maxLength: 128),
                        OSName = c.String(nullable: false, maxLength: 256),
                        OSServicePack = c.String(nullable: false, maxLength: 256),
                        SerialNumber = c.String(nullable: false, maxLength: 256),
                        Manufacturer = c.String(nullable: false, maxLength: 256),
                        Model = c.String(nullable: false, maxLength: 256),
                        RecordState = c.Int(nullable: false),
                        AcrchiveDate = c.DateTime(nullable: false),
                        ArchiveAuthor = c.String(),
                        LDAPGuid = c.Guid(nullable: false),
                        LDAPExists = c.Boolean(nullable: false),
                        BuildingId = c.Guid(nullable: false),
                        RoomId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ComputerId);
            
        }
        
        public override void Down()
        {
            DropTable("Computers.ComputerInfo");
        }
    }
}
