namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastChanges_20_07_2015_1330 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GearChangeses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChangesDate = c.DateTime(nullable: false),
                        Gear_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gears", t => t.Gear_Id)
                .Index(t => t.Gear_Id);
            
            CreateTable(
                "dbo.RemainderChangeses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChangesDate = c.DateTime(nullable: false),
                        Remainder_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Remainders", t => t.Remainder_Id)
                .Index(t => t.Remainder_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RemainderChangeses", "Remainder_Id", "dbo.Remainders");
            DropForeignKey("dbo.GearChangeses", "Gear_Id", "dbo.Gears");
            DropIndex("dbo.RemainderChangeses", new[] { "Remainder_Id" });
            DropIndex("dbo.GearChangeses", new[] { "Gear_Id" });
            DropTable("dbo.RemainderChangeses");
            DropTable("dbo.GearChangeses");
        }
    }
}
