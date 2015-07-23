namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filter009 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Filters.StaffFilterClause", "FilterId", "Filters.MemberFilter");
            DropIndex("Filters.StaffFilterClause", new[] { "FilterId" });
        }
        
        public override void Down()
        {
            CreateIndex("Filters.StaffFilterClause", "FilterId");
            AddForeignKey("Filters.StaffFilterClause", "FilterId", "Filters.MemberFilter", "FilterId", cascadeDelete: true);
        }
    }
}
