namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapTemplates010 : DbMigration
    {
        public override void Up()
        {
            AddColumn("LcpsLdap.OUTemplate", "FilterClass", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("LcpsLdap.OUTemplate", "FilterClass");
        }
    }
}
