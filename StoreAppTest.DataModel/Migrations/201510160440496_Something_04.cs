namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Something_04 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TodayRealizationsTotals",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        Today = c.DateTime(nullable: false),
                        PriceListName = c.String(nullable: false, maxLength: 128),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalByBuy = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.UserName, t.Today, t.PriceListName });
            
            CreateTable(
                "dbo.TodayRefundInDebtsIds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SaleItemId = c.Int(nullable: false),
                        Today = c.DateTime(nullable: false),
                        UserName = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TodayRefundInDebtsIds");
            DropTable("dbo.TodayRealizationsTotals");
        }
    }
}
