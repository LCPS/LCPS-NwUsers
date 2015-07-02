namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Building01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HRBuildings",
                c => new
                    {
                        BuildingKey = c.Guid(nullable: false),
                        BuildingId = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.BuildingKey)
                .Index(t => t.BuildingId, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.HRBuildings", new[] { "BuildingId" });
            DropTable("dbo.HRBuildings");
        }
    }
}
