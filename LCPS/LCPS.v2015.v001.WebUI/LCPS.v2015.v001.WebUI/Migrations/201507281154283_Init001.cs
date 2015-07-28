namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "HumanResources.HRRoom",
                c => new
                {
                    RoomKey = c.Guid(nullable: false),
                    BuildingId = c.Guid(nullable: false),
                    RoomNumber = c.String(nullable: false, maxLength: 128),
                    RoomId = c.String(nullable: false, maxLength: 128),
                    RoomType = c.Int(nullable: false),
                    PrimaryOccupant = c.String(maxLength: 128),
                })
                .PrimaryKey(t => t.RoomKey)
                .Index(t => new { t.BuildingId, t.RoomNumber }, unique: true, name: "IX_RoomName");
        }
        
        public override void Down()
        {
            DropTable("HumanResources.HRRoom");
        }
    }
}
