namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LdapAccount001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "LcpsLdap.LdapAccount",
                c => new
                    {
                        AccountId = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        UserKey = c.Guid(nullable: false),
                        InitialPassword = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.AccountId)
                .Index(t => new { t.UserName, t.UserKey }, unique: true, name: "IX_LdapAccount_UserName");
            
        }
        
        public override void Down()
        {
            DropIndex("LcpsLdap.LdapAccount", "IX_LdapAccount_UserName");
            DropTable("LcpsLdap.LdapAccount");
        }
    }
}
