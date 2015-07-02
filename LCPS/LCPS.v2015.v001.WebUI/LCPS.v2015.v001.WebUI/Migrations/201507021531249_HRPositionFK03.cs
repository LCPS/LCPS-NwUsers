namespace LCPS.v2015.v001.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HRPositionFK03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HRStaffs", "PositionCaptions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HRStaffs", "PositionCaptions");
        }
    }
}
