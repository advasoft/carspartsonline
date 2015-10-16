namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TodayRefundIds_update_5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TodayRealizationItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Today = c.DateTime(nullable: false),
                        UserName = c.String(),
                        CatalogNumber = c.String(),
                        Name = c.String(),
                        IsDuplicate = c.Boolean(nullable: false),
                        UnitOfMeasure = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remainders = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaleItem_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TodayRealizationItems");
        }
    }
}
