namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppBaseSmtpDomainAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("Anvil.ApplicationBase", "SMTPDomain", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("Anvil.ApplicationBase", "SMTPDomain");
        }
    }
}
