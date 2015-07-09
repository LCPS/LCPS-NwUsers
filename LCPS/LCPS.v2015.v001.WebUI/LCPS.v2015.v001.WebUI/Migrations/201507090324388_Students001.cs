namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Students001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "HumanResources.InstructionalLevel",
                c => new
                    {
                        InstructionalLevelKey = c.Guid(nullable: false),
                        InstructionalLevelId = c.String(nullable: false, maxLength: 25),
                        InstructionalLevelName = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.InstructionalLevelKey)
                .Index(t => t.InstructionalLevelId, unique: true, name: "IX_ILevelId")
                .Index(t => t.InstructionalLevelName, unique: true, name: "IX_ILevelName");
            
        }
        
        public override void Down()
        {
            DropIndex("HumanResources.InstructionalLevel", "IX_ILevelName");
            DropIndex("HumanResources.InstructionalLevel", "IX_ILevelId");
            DropTable("HumanResources.InstructionalLevel");
        }
    }
}
