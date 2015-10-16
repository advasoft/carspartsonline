namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TodayRefundTotal_added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TodayRefundsTotals",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        Today = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalByBuy = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.UserName, t.Today });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TodayRefundsTotals");
        }
    }
}
