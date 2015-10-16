namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TodayRefundIds_update_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodayRefundsTotals", "PriceListName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TodayRefundsTotals", "PriceListName");
        }
    }
}
