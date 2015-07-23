namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Email002 : DbMigration
    {
        public override void Up()
        {
            DropColumn("LcpsLdap.OUTemplate", "FilterClass");
        }
        
        public override void Down()
        {
            AddColumn("LcpsLdap.OUTemplate", "FilterClass", c => c.Int(nullable: false));
        }
    }
}
