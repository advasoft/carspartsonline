namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Something_02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodayRealizationItems", "PriceItem_Id", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TodayRealizationItems", "PriceItem_Id");
        }
    }
}
