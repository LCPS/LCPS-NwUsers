namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filter008 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Filters.StudentFilterClause", "FilterId", "Filters.MemberFilter");
            DropIndex("Filters.StudentFilterClause", new[] { "FilterId" });
            DropPrimaryKey("Filters.StudentFilterClause");
            AddColumn("Filters.StudentFilterClause", "StudentFilterClauseId", c => c.Guid(nullable: false));
            AddPrimaryKey("Filters.StudentFilterClause", "StudentFilterClauseId");
            DropColumn("Filters.StudentFilterClause", "StaffFilterClauseId");
        }
        
        public override void Down()
        {
            AddColumn("Filters.StudentFilterClause", "StaffFilterClauseId", c => c.Guid(nullable: false));
            DropPrimaryKey("Filters.StudentFilterClause");
            DropColumn("Filters.StudentFilterClause", "StudentFilterClauseId");
            AddPrimaryKey("Filters.StudentFilterClause", "StaffFilterClauseId");
            CreateIndex("Filters.StudentFilterClause", "FilterId");
            AddForeignKey("Filters.StudentFilterClause", "FilterId", "Filters.MemberFilter", "FilterId", cascadeDelete: true);
        }
    }
}
