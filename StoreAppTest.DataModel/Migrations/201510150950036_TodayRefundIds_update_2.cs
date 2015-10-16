namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TodayRefundIds_update_2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodayRefundIds", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TodayRefundIds", "Amount");
        }
    }
}
