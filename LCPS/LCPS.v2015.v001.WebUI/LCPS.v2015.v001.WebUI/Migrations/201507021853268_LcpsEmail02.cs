namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LcpsEmail02 : DbMigration
    {
        public override void Up()
        {
            DropIndex("LcpsSecurity.LcpsEmail", "IX_LCPSEmailAddress");
            DropTable("LcpsSecurity.LcpsEmail");
        }
        
        public override void Down()
        {
            CreateTable(
                "LcpsSecurity.LcpsEmail",
                c => new
                    {
                        CredentialId = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 50),
                        AntecedentId = c.Guid(nullable: false),
                        InitialPassword = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.CredentialId);
            
            CreateIndex("LcpsSecurity.LcpsEmail", "Email", unique: true, name: "IX_LCPSEmailAddress");
        }
    }
}
