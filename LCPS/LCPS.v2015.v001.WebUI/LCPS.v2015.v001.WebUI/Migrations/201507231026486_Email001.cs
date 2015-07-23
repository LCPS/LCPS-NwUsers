namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Email001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "LcpsEmail.EmailAccount",
                c => new
                    {
                        RecordId = c.Guid(nullable: false),
                        UserLink = c.Guid(nullable: false),
                        EmailAddress = c.String(),
                        InitialPassword = c.String(),
                    })
                .PrimaryKey(t => t.RecordId);
            
            DropColumn("HumanResources.HRStaff", "StaffEmail");
        }
        
        public override void Down()
        {
            AddColumn("HumanResources.HRStaff", "StaffEmail", c => c.String(maxLength: 256));
            DropTable("LcpsEmail.EmailAccount");
        }
    }
}
