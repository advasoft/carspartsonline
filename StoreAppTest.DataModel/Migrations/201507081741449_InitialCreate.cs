namespace StoreAppTest.DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 255),
                        Details = c.String(),
                        BankDetails = c.String(),
                        ShipmentAddress = c.String(),
                        Creator_Id = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .Index(t => t.Creator_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 255),
                        DisplayName = c.String(maxLength: 255),
                        Warehouse_Id = c.String(maxLength: 255),
                        PasswordHash = c.Binary(),
                        IsSupplierVisible = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserName)
                .ForeignKey("dbo.Warehouses", t => t.Warehouse_Id)
                .Index(t => t.Warehouse_Id);
            
            CreateTable(
                "dbo.PriceLists",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 255),
                        UploadUser_Id = c.String(nullable: false, maxLength: 255),
                        UploadDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Users", t => t.UploadUser_Id)
                .Index(t => t.UploadUser_Id);
            
            CreateTable(
                "dbo.PriceItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Gear_Id = c.Long(nullable: false),
                        BuyPriceRur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyPriceTng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Uom_Id = c.String(nullable: false, maxLength: 255),
                        Barcode1 = c.String(maxLength: 50),
                        Barcode2 = c.String(maxLength: 50),
                        Barcode3 = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gears", t => t.Gear_Id)
                .ForeignKey("dbo.UnitOfMeasures", t => t.Uom_Id)
                .Index(t => t.Gear_Id)
                .Index(t => t.Uom_Id);
            
            CreateTable(
                "dbo.Gears",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CatalogNumber = c.String(maxLength: 255),
                        Name = c.String(nullable: false, maxLength: 500),
                        Articul = c.String(maxLength: 255),
                        IsDuplicate = c.Boolean(nullable: false),
                        Category_Id = c.String(nullable: false, maxLength: 255),
                        Image_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GearCategories", t => t.Category_Id)
                .ForeignKey("dbo.Resources", t => t.Image_Id)
                .Index(t => t.Category_Id)
                .Index(t => t.Image_Id);
            
            CreateTable(
                "dbo.GearCategories",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ResourceData = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WholesalePrices",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PriceDate = c.DateTime(nullable: false),
                        PriceItem_Id = c.Long(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceItems", t => t.PriceItem_Id)
                .Index(t => t.PriceItem_Id);
            
            CreateTable(
                "dbo.Remainders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RemainderDate = c.DateTime(nullable: false),
                        PriceItem_Id = c.Long(nullable: false),
                        Warehouse_Id = c.String(nullable: false, maxLength: 255),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RecommendedRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LowerLimitRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Warehouses", t => t.Warehouse_Id)
                .ForeignKey("dbo.PriceItems", t => t.PriceItem_Id)
                .Index(t => t.PriceItem_Id)
                .Index(t => t.Warehouse_Id);
            
            CreateTable(
                "dbo.RemaindersUserChanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        User_Id = c.String(nullable: false, maxLength: 255),
                        Remainder_Id = c.Long(nullable: false),
                        Amound = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .ForeignKey("dbo.Remainders", t => t.Remainder_Id)
                .Index(t => t.User_Id)
                .Index(t => t.Remainder_Id);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.UnitOfMeasures",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.RoleName);
            
            CreateTable(
                "dbo.DebtDischargeDocuments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DischargeDate = c.DateTime(nullable: false),
                        Debtor_Id = c.String(nullable: false, maxLength: 255),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Creator_Id = c.String(nullable: false, maxLength: 255),
                        IsDischarge = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .ForeignKey("dbo.Customers", t => t.Debtor_Id)
                .Index(t => t.Debtor_Id)
                .Index(t => t.Creator_Id);
            
            CreateTable(
                "dbo.GearNews",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreatedDate = c.DateTime(nullable: false),
                        Gear_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gears", t => t.Gear_Id)
                .Index(t => t.Gear_Id);
            
            CreateTable(
                "dbo.IncomeItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PriceItem_Id = c.Long(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NewPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyPriceRur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyPriceTng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Income_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Incomes", t => t.Income_Id)
                .ForeignKey("dbo.PriceItems", t => t.PriceItem_Id)
                .Index(t => t.PriceItem_Id)
                .Index(t => t.Income_Id);
            
            CreateTable(
                "dbo.Incomes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IncomeNumber = c.String(nullable: false, maxLength: 255),
                        IncomeDate = c.DateTime(nullable: false),
                        IsAccept = c.Boolean(nullable: false),
                        AcceptedDate = c.DateTime(),
                        Supplier_Id = c.String(nullable: false, maxLength: 255),
                        Creator_Id = c.String(nullable: false, maxLength: 255),
                        Accepter_Id = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Accepter_Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .ForeignKey("dbo.Suppliers", t => t.Supplier_Id)
                .Index(t => t.Supplier_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.Accepter_Id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 255),
                        City = c.String(maxLength: 255),
                        Phone = c.String(maxLength: 255),
                        Email = c.String(maxLength: 255),
                        Manager = c.String(maxLength: 255),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.PriceChangeReportItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PriceItem_Id = c.Long(nullable: false),
                        PreviousPrice_Id = c.Long(nullable: false),
                        NewPrice_Id = c.Long(nullable: false),
                        PriceChangeReport_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.WholesalePrices", t => t.NewPrice_Id)
                .ForeignKey("dbo.WholesalePrices", t => t.PreviousPrice_Id)
                .ForeignKey("dbo.PriceChangeReports", t => t.PriceChangeReport_Id)
                .ForeignKey("dbo.PriceItems", t => t.PriceItem_Id)
                .Index(t => t.PriceItem_Id)
                .Index(t => t.PreviousPrice_Id)
                .Index(t => t.NewPrice_Id)
                .Index(t => t.PriceChangeReport_Id);
            
            CreateTable(
                "dbo.PriceChangeReports",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReportNumber = c.String(nullable: false, maxLength: 255),
                        ReportDate = c.DateTime(nullable: false),
                        Creator_Id = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .Index(t => t.Creator_Id);
            
            CreateTable(
                "dbo.RefundDocuments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RefundNumber = c.String(nullable: false, maxLength: 255),
                        RefundDate = c.DateTime(nullable: false),
                        SaleDocument_Id = c.Long(nullable: false),
                        Creator_Id = c.String(nullable: false, maxLength: 255),
                        LastChanger_Id = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .ForeignKey("dbo.Users", t => t.LastChanger_Id)
                .ForeignKey("dbo.SaleDocuments", t => t.SaleDocument_Id)
                .Index(t => t.SaleDocument_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.LastChanger_Id);
            
            CreateTable(
                "dbo.RefundItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PriceItem_Id = c.Long(nullable: false),
                        SaleItem_Id = c.Long(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RefundDocument_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceItems", t => t.PriceItem_Id)
                .ForeignKey("dbo.SaleItems", t => t.SaleItem_Id)
                .ForeignKey("dbo.RefundDocuments", t => t.RefundDocument_Id)
                .Index(t => t.PriceItem_Id)
                .Index(t => t.SaleItem_Id)
                .Index(t => t.RefundDocument_Id);
            
            CreateTable(
                "dbo.SaleItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PriceItem_Id = c.Long(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CatalogNumber = c.String(maxLength: 255),
                        Name = c.String(maxLength: 500),
                        Articul = c.String(maxLength: 255),
                        IsDuplicate = c.Boolean(nullable: false),
                        SaleDocument_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceItems", t => t.PriceItem_Id)
                .ForeignKey("dbo.SaleDocuments", t => t.SaleDocument_Id)
                .Index(t => t.PriceItem_Id)
                .Index(t => t.SaleDocument_Id);
            
            CreateTable(
                "dbo.SaleDocuments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaleDate = c.DateTime(nullable: false),
                        Customer_Name = c.String(nullable: false, maxLength: 255),
                        Number = c.String(nullable: false, maxLength: 255),
                        Creator_Id = c.String(nullable: false, maxLength: 255),
                        LastChanger_Id = c.String(nullable: false, maxLength: 255),
                        IsReceiptPrinted = c.Boolean(nullable: false),
                        IsOrder = c.Boolean(nullable: false),
                        IsInDebt = c.Boolean(nullable: false),
                        IsInvoice = c.Boolean(nullable: false),
                        Barcode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Name)
                .ForeignKey("dbo.Users", t => t.LastChanger_Id)
                .Index(t => t.Customer_Name)
                .Index(t => t.Creator_Id)
                .Index(t => t.LastChanger_Id);
            
            CreateTable(
                "dbo.RefundsPerDayItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RefundItem_Id = c.Long(nullable: false),
                        SaleDocumentsPerDay_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RefundItems", t => t.RefundItem_Id)
                .ForeignKey("dbo.SaleDocumentsPerDays", t => t.SaleDocumentsPerDay_Id)
                .Index(t => t.RefundItem_Id)
                .Index(t => t.SaleDocumentsPerDay_Id);
            
            CreateTable(
                "dbo.SaleDocumentsPerDays",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SaleDocumentsDate = c.DateTime(nullable: false),
                        Barcode = c.String(),
                        Number = c.String(nullable: false, maxLength: 255),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalRefund = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmountProfit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalRefundProfit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SubTotalProfit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Creator_Id = c.String(nullable: false, maxLength: 255),
                        IsClosed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .Index(t => t.Creator_Id);
            
            CreateTable(
                "dbo.SalesPerDayItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CatalogNumber = c.String(maxLength: 255),
                        Name = c.String(nullable: false, maxLength: 500),
                        IsDuplicate = c.Boolean(nullable: false),
                        UnitOfMeasure = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remainders = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaleItem_Id = c.Long(nullable: false),
                        SaleDocumentsPerDay_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SaleItems", t => t.SaleItem_Id)
                .ForeignKey("dbo.SaleDocumentsPerDays", t => t.SaleDocumentsPerDay_Id)
                .Index(t => t.SaleItem_Id)
                .Index(t => t.SaleDocumentsPerDay_Id);
            
            CreateTable(
                "dbo.WarehouseTransferRequestItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PriceItem_Id = c.Long(nullable: false),
                        Count = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CountAccepted = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WholesalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AcceptedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WarehouseTransferRequest_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PriceItems", t => t.PriceItem_Id)
                .ForeignKey("dbo.WarehouseTransferRequests", t => t.WarehouseTransferRequest_Id)
                .Index(t => t.PriceItem_Id)
                .Index(t => t.WarehouseTransferRequest_Id);
            
            CreateTable(
                "dbo.WarehouseTransferRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestNumber = c.String(nullable: false, maxLength: 255),
                        RequestDate = c.DateTime(nullable: false),
                        IsAccept = c.Boolean(nullable: false),
                        IsReserve = c.Boolean(nullable: false),
                        StateChangedDate = c.DateTime(),
                        Status = c.String(maxLength: 255),
                        Customer_Id = c.String(nullable: false, maxLength: 255),
                        Supplier_Id = c.String(nullable: false, maxLength: 255),
                        Creator_Id = c.String(nullable: false, maxLength: 255),
                        LastChanged_Id = c.String(maxLength: 255),
                        Description = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Creator_Id)
                .ForeignKey("dbo.Warehouses", t => t.Customer_Id)
                .ForeignKey("dbo.Users", t => t.LastChanged_Id)
                .ForeignKey("dbo.Warehouses", t => t.Supplier_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Supplier_Id)
                .Index(t => t.Creator_Id)
                .Index(t => t.LastChanged_Id);
            
            CreateTable(
                "dbo.PriceItemRemainder_VIEW",
                c => new
                    {
                        PriceItem_Id = c.Long(nullable: false),
                        BuyPriceRur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyPriceTng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WholesalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Supplier = c.String(),
                        Uom = c.String(),
                        Gear_Id = c.Long(nullable: false),
                        CatalogNumber = c.String(),
                        Articul = c.String(),
                        IsDuplicate = c.Boolean(nullable: false),
                        Gear_Name = c.String(),
                        RecommendedRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LowerLimitRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remainder_Id = c.Long(nullable: false),
                        Warehouse = c.String(),
                        RemainderDate = c.DateTime(nullable: false),
                        Remainders = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PriceItem_Id);
            
            CreateTable(
                "dbo.PriceIncomeItem_VIEW",
                c => new
                    {
                        PriceItem_Id = c.Long(nullable: false),
                        BuyPriceRur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyPriceTng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WholesalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Uom = c.String(),
                        Gear_Id = c.Long(nullable: false),
                        CatalogNumber = c.String(),
                        Articul = c.String(),
                        IsDuplicate = c.Boolean(nullable: false),
                        Gear_Name = c.String(),
                        RecommendedRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LowerLimitRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remainders = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Incomes = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PriceItem_Id);
            
            CreateTable(
                "dbo.PriceTotalIncomeItem_VIEW",
                c => new
                    {
                        PriceItem_Id = c.Long(nullable: false),
                        BuyPriceRur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyPriceTng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WholesalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Uom = c.String(),
                        Gear_Id = c.Long(nullable: false),
                        CatalogNumber = c.String(),
                        Articul = c.String(),
                        IsDuplicate = c.Boolean(nullable: false),
                        Gear_Name = c.String(),
                        RecommendedRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LowerLimitRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remainders = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Incomes = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Income_Id = c.Long(nullable: false),
                        IsAccept = c.Boolean(nullable: false),
                        NewPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PriceItem_Id);
            
            CreateTable(
                "dbo.PriceItemView_VIEW",
                c => new
                    {
                        PriceItem_Id = c.Long(nullable: false),
                        BuyPriceRur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyPriceTng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Uom = c.String(),
                        Gear_Id = c.Long(nullable: false),
                        CatalogNumber = c.String(),
                        Articul = c.String(),
                        IsDuplicate = c.Boolean(nullable: false),
                        Gear_Name = c.String(),
                        WholesalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WholesalePriceDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PriceItem_Id);
            
            CreateTable(
                "dbo.PriceListItemRemainder_VIEW",
                c => new
                    {
                        PriceItem_Id = c.Long(nullable: false),
                        BuyPriceRur = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BuyPriceTng = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WholesalePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceList_Name = c.String(),
                        Supplier = c.String(),
                        Uom = c.String(),
                        Gear_Id = c.Long(nullable: false),
                        CatalogNumber = c.String(),
                        Articul = c.String(),
                        IsDuplicate = c.Boolean(nullable: false),
                        Gear_Name = c.String(),
                        RecommendedRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LowerLimitRemainder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remainder_Id = c.Long(nullable: false),
                        Warehouse = c.String(),
                        RemainderDate = c.DateTime(nullable: false),
                        Remainders = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PriceItem_Id);
            
            CreateTable(
                "dbo.PriceListPriceItems",
                c => new
                    {
                        PriceList_Name = c.String(nullable: false, maxLength: 255),
                        PriceItem_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.PriceList_Name, t.PriceItem_Id })
                .ForeignKey("dbo.PriceLists", t => t.PriceList_Name, cascadeDelete: true)
                .ForeignKey("dbo.PriceItems", t => t.PriceItem_Id, cascadeDelete: true)
                .Index(t => t.PriceList_Name)
                .Index(t => t.PriceItem_Id);
            
            CreateTable(
                "dbo.UsersPriceLists",
                c => new
                    {
                        User_UserName = c.String(nullable: false, maxLength: 255),
                        PriceList_Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => new { t.User_UserName, t.PriceList_Name })
                .ForeignKey("dbo.Users", t => t.User_UserName, cascadeDelete: true)
                .ForeignKey("dbo.PriceLists", t => t.PriceList_Name, cascadeDelete: true)
                .Index(t => t.User_UserName)
                .Index(t => t.PriceList_Name);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        User_UserName = c.String(nullable: false, maxLength: 255),
                        Role_RoleName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => new { t.User_UserName, t.Role_RoleName })
                .ForeignKey("dbo.Users", t => t.User_UserName, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_RoleName, cascadeDelete: true)
                .Index(t => t.User_UserName)
                .Index(t => t.Role_RoleName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WarehouseTransferRequestItems", "WarehouseTransferRequest_Id", "dbo.WarehouseTransferRequests");
            DropForeignKey("dbo.WarehouseTransferRequests", "Supplier_Id", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseTransferRequests", "LastChanged_Id", "dbo.Users");
            DropForeignKey("dbo.WarehouseTransferRequests", "Customer_Id", "dbo.Warehouses");
            DropForeignKey("dbo.WarehouseTransferRequests", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.WarehouseTransferRequestItems", "PriceItem_Id", "dbo.PriceItems");
            DropForeignKey("dbo.RefundsPerDayItems", "SaleDocumentsPerDay_Id", "dbo.SaleDocumentsPerDays");
            DropForeignKey("dbo.SalesPerDayItems", "SaleDocumentsPerDay_Id", "dbo.SaleDocumentsPerDays");
            DropForeignKey("dbo.SalesPerDayItems", "SaleItem_Id", "dbo.SaleItems");
            DropForeignKey("dbo.SaleDocumentsPerDays", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.RefundsPerDayItems", "RefundItem_Id", "dbo.RefundItems");
            DropForeignKey("dbo.RefundDocuments", "SaleDocument_Id", "dbo.SaleDocuments");
            DropForeignKey("dbo.RefundItems", "RefundDocument_Id", "dbo.RefundDocuments");
            DropForeignKey("dbo.RefundItems", "SaleItem_Id", "dbo.SaleItems");
            DropForeignKey("dbo.SaleItems", "SaleDocument_Id", "dbo.SaleDocuments");
            DropForeignKey("dbo.SaleDocuments", "LastChanger_Id", "dbo.Users");
            DropForeignKey("dbo.SaleDocuments", "Customer_Name", "dbo.Customers");
            DropForeignKey("dbo.SaleDocuments", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.SaleItems", "PriceItem_Id", "dbo.PriceItems");
            DropForeignKey("dbo.RefundItems", "PriceItem_Id", "dbo.PriceItems");
            DropForeignKey("dbo.RefundDocuments", "LastChanger_Id", "dbo.Users");
            DropForeignKey("dbo.RefundDocuments", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.PriceChangeReportItems", "PriceItem_Id", "dbo.PriceItems");
            DropForeignKey("dbo.PriceChangeReportItems", "PriceChangeReport_Id", "dbo.PriceChangeReports");
            DropForeignKey("dbo.PriceChangeReports", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.PriceChangeReportItems", "PreviousPrice_Id", "dbo.WholesalePrices");
            DropForeignKey("dbo.PriceChangeReportItems", "NewPrice_Id", "dbo.WholesalePrices");
            DropForeignKey("dbo.IncomeItems", "PriceItem_Id", "dbo.PriceItems");
            DropForeignKey("dbo.Incomes", "Supplier_Id", "dbo.Suppliers");
            DropForeignKey("dbo.IncomeItems", "Income_Id", "dbo.Incomes");
            DropForeignKey("dbo.Incomes", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.Incomes", "Accepter_Id", "dbo.Users");
            DropForeignKey("dbo.GearNews", "Gear_Id", "dbo.Gears");
            DropForeignKey("dbo.DebtDischargeDocuments", "Debtor_Id", "dbo.Customers");
            DropForeignKey("dbo.DebtDischargeDocuments", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.Customers", "Creator_Id", "dbo.Users");
            DropForeignKey("dbo.Users", "Warehouse_Id", "dbo.Warehouses");
            DropForeignKey("dbo.UserRoles", "Role_RoleName", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "User_UserName", "dbo.Users");
            DropForeignKey("dbo.UsersPriceLists", "PriceList_Name", "dbo.PriceLists");
            DropForeignKey("dbo.UsersPriceLists", "User_UserName", "dbo.Users");
            DropForeignKey("dbo.PriceLists", "UploadUser_Id", "dbo.Users");
            DropForeignKey("dbo.PriceListPriceItems", "PriceItem_Id", "dbo.PriceItems");
            DropForeignKey("dbo.PriceListPriceItems", "PriceList_Name", "dbo.PriceLists");
            DropForeignKey("dbo.PriceItems", "Uom_Id", "dbo.UnitOfMeasures");
            DropForeignKey("dbo.Remainders", "PriceItem_Id", "dbo.PriceItems");
            DropForeignKey("dbo.Remainders", "Warehouse_Id", "dbo.Warehouses");
            DropForeignKey("dbo.RemaindersUserChanges", "Remainder_Id", "dbo.Remainders");
            DropForeignKey("dbo.RemaindersUserChanges", "User_Id", "dbo.Users");
            DropForeignKey("dbo.WholesalePrices", "PriceItem_Id", "dbo.PriceItems");
            DropForeignKey("dbo.PriceItems", "Gear_Id", "dbo.Gears");
            DropForeignKey("dbo.Gears", "Image_Id", "dbo.Resources");
            DropForeignKey("dbo.Gears", "Category_Id", "dbo.GearCategories");
            DropIndex("dbo.UserRoles", new[] { "Role_RoleName" });
            DropIndex("dbo.UserRoles", new[] { "User_UserName" });
            DropIndex("dbo.UsersPriceLists", new[] { "PriceList_Name" });
            DropIndex("dbo.UsersPriceLists", new[] { "User_UserName" });
            DropIndex("dbo.PriceListPriceItems", new[] { "PriceItem_Id" });
            DropIndex("dbo.PriceListPriceItems", new[] { "PriceList_Name" });
            DropIndex("dbo.WarehouseTransferRequests", new[] { "LastChanged_Id" });
            DropIndex("dbo.WarehouseTransferRequests", new[] { "Creator_Id" });
            DropIndex("dbo.WarehouseTransferRequests", new[] { "Supplier_Id" });
            DropIndex("dbo.WarehouseTransferRequests", new[] { "Customer_Id" });
            DropIndex("dbo.WarehouseTransferRequestItems", new[] { "WarehouseTransferRequest_Id" });
            DropIndex("dbo.WarehouseTransferRequestItems", new[] { "PriceItem_Id" });
            DropIndex("dbo.SalesPerDayItems", new[] { "SaleDocumentsPerDay_Id" });
            DropIndex("dbo.SalesPerDayItems", new[] { "SaleItem_Id" });
            DropIndex("dbo.SaleDocumentsPerDays", new[] { "Creator_Id" });
            DropIndex("dbo.RefundsPerDayItems", new[] { "SaleDocumentsPerDay_Id" });
            DropIndex("dbo.RefundsPerDayItems", new[] { "RefundItem_Id" });
            DropIndex("dbo.SaleDocuments", new[] { "LastChanger_Id" });
            DropIndex("dbo.SaleDocuments", new[] { "Creator_Id" });
            DropIndex("dbo.SaleDocuments", new[] { "Customer_Name" });
            DropIndex("dbo.SaleItems", new[] { "SaleDocument_Id" });
            DropIndex("dbo.SaleItems", new[] { "PriceItem_Id" });
            DropIndex("dbo.RefundItems", new[] { "RefundDocument_Id" });
            DropIndex("dbo.RefundItems", new[] { "SaleItem_Id" });
            DropIndex("dbo.RefundItems", new[] { "PriceItem_Id" });
            DropIndex("dbo.RefundDocuments", new[] { "LastChanger_Id" });
            DropIndex("dbo.RefundDocuments", new[] { "Creator_Id" });
            DropIndex("dbo.RefundDocuments", new[] { "SaleDocument_Id" });
            DropIndex("dbo.PriceChangeReports", new[] { "Creator_Id" });
            DropIndex("dbo.PriceChangeReportItems", new[] { "PriceChangeReport_Id" });
            DropIndex("dbo.PriceChangeReportItems", new[] { "NewPrice_Id" });
            DropIndex("dbo.PriceChangeReportItems", new[] { "PreviousPrice_Id" });
            DropIndex("dbo.PriceChangeReportItems", new[] { "PriceItem_Id" });
            DropIndex("dbo.Incomes", new[] { "Accepter_Id" });
            DropIndex("dbo.Incomes", new[] { "Creator_Id" });
            DropIndex("dbo.Incomes", new[] { "Supplier_Id" });
            DropIndex("dbo.IncomeItems", new[] { "Income_Id" });
            DropIndex("dbo.IncomeItems", new[] { "PriceItem_Id" });
            DropIndex("dbo.GearNews", new[] { "Gear_Id" });
            DropIndex("dbo.DebtDischargeDocuments", new[] { "Creator_Id" });
            DropIndex("dbo.DebtDischargeDocuments", new[] { "Debtor_Id" });
            DropIndex("dbo.RemaindersUserChanges", new[] { "Remainder_Id" });
            DropIndex("dbo.RemaindersUserChanges", new[] { "User_Id" });
            DropIndex("dbo.Remainders", new[] { "Warehouse_Id" });
            DropIndex("dbo.Remainders", new[] { "PriceItem_Id" });
            DropIndex("dbo.WholesalePrices", new[] { "PriceItem_Id" });
            DropIndex("dbo.Gears", new[] { "Image_Id" });
            DropIndex("dbo.Gears", new[] { "Category_Id" });
            DropIndex("dbo.PriceItems", new[] { "Uom_Id" });
            DropIndex("dbo.PriceItems", new[] { "Gear_Id" });
            DropIndex("dbo.PriceLists", new[] { "UploadUser_Id" });
            DropIndex("dbo.Users", new[] { "Warehouse_Id" });
            DropIndex("dbo.Customers", new[] { "Creator_Id" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.UsersPriceLists");
            DropTable("dbo.PriceListPriceItems");
            DropTable("dbo.PriceListItemRemainder_VIEW");
            DropTable("dbo.PriceItemView_VIEW");
            DropTable("dbo.PriceTotalIncomeItem_VIEW");
            DropTable("dbo.PriceIncomeItem_VIEW");
            DropTable("dbo.PriceItemRemainder_VIEW");
            DropTable("dbo.WarehouseTransferRequests");
            DropTable("dbo.WarehouseTransferRequestItems");
            DropTable("dbo.SalesPerDayItems");
            DropTable("dbo.SaleDocumentsPerDays");
            DropTable("dbo.RefundsPerDayItems");
            DropTable("dbo.SaleDocuments");
            DropTable("dbo.SaleItems");
            DropTable("dbo.RefundItems");
            DropTable("dbo.RefundDocuments");
            DropTable("dbo.PriceChangeReports");
            DropTable("dbo.PriceChangeReportItems");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Incomes");
            DropTable("dbo.IncomeItems");
            DropTable("dbo.GearNews");
            DropTable("dbo.DebtDischargeDocuments");
            DropTable("dbo.Roles");
            DropTable("dbo.UnitOfMeasures");
            DropTable("dbo.Warehouses");
            DropTable("dbo.RemaindersUserChanges");
            DropTable("dbo.Remainders");
            DropTable("dbo.WholesalePrices");
            DropTable("dbo.Resources");
            DropTable("dbo.GearCategories");
            DropTable("dbo.Gears");
            DropTable("dbo.PriceItems");
            DropTable("dbo.PriceLists");
            DropTable("dbo.Users");
            DropTable("dbo.Customers");
        }
    }
}
