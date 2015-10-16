namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TodayRefundIds_update_1 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.TodayRefundIds");
            AddColumn("dbo.TodayRefundIds", "SaleItemId", c => c.Int(nullable: false));
            AddColumn("dbo.TodayRefundIds", "Today", c => c.DateTime(nullable: false));
            AddColumn("dbo.TodayRefundIds", "UserName", c => c.String());
            AlterColumn("dbo.TodayRefundIds", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.TodayRefundIds", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.TodayRefundIds");
            AlterColumn("dbo.TodayRefundIds", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.TodayRefundIds", "UserName");
            DropColumn("dbo.TodayRefundIds", "Today");
            DropColumn("dbo.TodayRefundIds", "SaleItemId");
            AddPrimaryKey("dbo.TodayRefundIds", "Id");
        }
    }
}
