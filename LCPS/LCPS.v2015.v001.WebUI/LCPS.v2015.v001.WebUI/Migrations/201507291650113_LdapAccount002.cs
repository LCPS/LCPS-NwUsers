namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapAccount002 : DbMigration
    {
        public override void Up()
        {
            AddColumn("LcpsLdap.LdapAccount", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("LcpsLdap.LdapAccount", "Active");
        }
    }
}
