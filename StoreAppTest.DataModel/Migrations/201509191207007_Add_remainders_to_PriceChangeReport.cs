namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_remainders_to_PriceChangeReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PriceChangeReportItems", "Remainders", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PriceChangeReportItems", "Remainders");
        }
    }
}
