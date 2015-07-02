namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRPositionFK04 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.HRStaffs", "PositionCaptions");
        }
        
        public override void Down()
        {
            AddColumn("dbo.HRStaffs", "PositionCaptions", c => c.String());
        }
    }
}
