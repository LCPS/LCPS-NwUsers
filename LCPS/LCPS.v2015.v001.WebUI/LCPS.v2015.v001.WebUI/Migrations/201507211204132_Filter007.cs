namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Filter007 : DbMigration
    {
        public override void Up()
        {
            DropColumn("Filters.MemberFilter", "Description");
        }
        
        public override void Down()
        {
            AddColumn("Filters.MemberFilter", "Description", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
