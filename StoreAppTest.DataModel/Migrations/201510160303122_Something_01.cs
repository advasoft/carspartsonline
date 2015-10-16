namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Something_01 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodayRealizationItems", "WholePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TodayRealizationItems", "SoldCount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TodayRealizationItems", "IsInDebt", c => c.Boolean(nullable: false));
            AddColumn("dbo.TodayRealizationItems", "Customer", c => c.String());
            AddColumn("dbo.TodayRealizationItems", "PriceListName", c => c.String());
            AddColumn("dbo.TodayRealizationItems", "SaledTime", c => c.String());
            AddColumn("dbo.TodayRealizationItems", "SalesNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TodayRealizationItems", "SalesNumber");
            DropColumn("dbo.TodayRealizationItems", "SaledTime");
            DropColumn("dbo.TodayRealizationItems", "PriceListName");
            DropColumn("dbo.TodayRealizationItems", "Customer");
            DropColumn("dbo.TodayRealizationItems", "IsInDebt");
            DropColumn("dbo.TodayRealizationItems", "SoldCount");
            DropColumn("dbo.TodayRealizationItems", "WholePrice");
        }
    }
}
