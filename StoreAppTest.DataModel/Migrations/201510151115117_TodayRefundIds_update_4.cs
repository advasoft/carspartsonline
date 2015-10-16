namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TodayRefundIds_update_4 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TodayRefundsTotals");
            AlterColumn("dbo.TodayRefundsTotals", "PriceListName", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.TodayRefundsTotals", new[] { "UserName", "Today", "PriceListName" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TodayRefundsTotals");
            AlterColumn("dbo.TodayRefundsTotals", "PriceListName", c => c.String());
            AddPrimaryKey("dbo.TodayRefundsTotals", new[] { "UserName", "Today" });
        }
    }
}
