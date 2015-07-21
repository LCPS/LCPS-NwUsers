namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filters004 : DbMigration
    {
        public override void Up()
        {
            DropIndex("Filters.MemberFilter", "IX_FilterName");
            AlterColumn("Filters.MemberFilter", "Title", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("Filters.MemberFilter", "Description", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("Filters.MemberFilter", new[] { "AntecedentId", "Title" }, unique: true, name: "IX_FilterName");
        }
        
        public override void Down()
        {
            DropIndex("Filters.MemberFilter", "IX_FilterName");
            AlterColumn("Filters.MemberFilter", "Description", c => c.String(maxLength: 128));
            AlterColumn("Filters.MemberFilter", "Title", c => c.String(maxLength: 128));
            CreateIndex("Filters.MemberFilter", new[] { "AntecedentId", "Title" }, unique: true, name: "IX_FilterName");
        }
    }
}
