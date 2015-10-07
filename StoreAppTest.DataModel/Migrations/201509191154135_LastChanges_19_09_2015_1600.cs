namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastChanges_19_09_2015_1600 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PriceItems", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PriceItems", "IsDeleted");
        }
    }
}
