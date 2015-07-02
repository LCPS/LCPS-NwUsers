namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LcpsEmail04 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "LcpsSecurity.LcpsEmail", newName: "LcpsStaffEmail");
            MoveTable(name: "LcpsSecurity.LcpsStaffEmail", newSchema: "Security");
        }
        
        public override void Down()
        {
            MoveTable(name: "Security.LcpsStaffEmail", newSchema: "LcpsSecurity");
            RenameTable(name: "LcpsSecurity.LcpsStaffEmail", newName: "LcpsEmail");
        }
    }
}
