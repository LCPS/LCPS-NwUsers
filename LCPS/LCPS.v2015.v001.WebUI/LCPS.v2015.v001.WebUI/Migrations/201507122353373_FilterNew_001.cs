namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FilterNew_001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilterGroups",
                c => new
                    {
                        FilterId = c.Guid(nullable: false),
                        AntecedentId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 75),
                        Description = c.String(nullable: false, maxLength: 2048),
                    })
                .PrimaryKey(t => t.FilterId)
                .Index(t => t.Name, unique: true, name: "IX_FilterName");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.FilterGroups", "IX_FilterName");
            DropTable("dbo.FilterGroups");
        }
    }
}
