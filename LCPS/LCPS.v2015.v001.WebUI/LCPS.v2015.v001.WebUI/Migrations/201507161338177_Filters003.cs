namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filters003 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Filters.MemberFilter",
                c => new
                    {
                        FilterId = c.Guid(nullable: false),
                        AntecedentId = c.Guid(nullable: false),
                        Title = c.String(maxLength: 128),
                        Description = c.String(maxLength: 128),
                        Category = c.Int(nullable: false),
                        FilterClass = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FilterId)
                .Index(t => new { t.AntecedentId, t.Title }, unique: true, name: "IX_FilterName");
            
        }
        
        public override void Down()
        {
            DropIndex("Filters.MemberFilter", "IX_FilterName");
            DropTable("Filters.MemberFilter");
        }
    }
}
