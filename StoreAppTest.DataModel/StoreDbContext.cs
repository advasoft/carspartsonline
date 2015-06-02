namespace StoreAppTest.Web.DataModel
{
    //extern alias ef;
    using System;
    using System.Data.Entity;
    using System.Text;
    //using ef::System.Data.Entity;
    using StoreAppTest.DataModel;
    using Utilities;

    public partial class StoreDbContext : DbContext
    {

        public StoreDbContext()
            : base("name=StoreDbContext")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        static StoreDbContext()
        {
            Database.SetInitializer(new StoreDbInitializer());
            //Database.SetInitializer(new DropCreateDatabaseAlways<StoreDbContext>());
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<DebtDischargeDocument> DebtDischargeDocuments { get; set; }
        public DbSet<GearCategory> GearCategories { get; set; }
        public DbSet<Gear> Gears { get; set; }
        public DbSet<IncomeItem> IncomeItems { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<PriceItem> PriceItems { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<RefundDocument> RefundDocuments { get; set; }
        public DbSet<RefundItem> RefundItems { get; set; }
        public DbSet<Remainder> Remainders { get; set; }
        //public DbSet<RetailPrice> RetailPrices { get; set; }
        public DbSet<WholesalePrice> WholesalePrices { get; set; }

        public DbSet<SaleDocument> SaleDocuments { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<SaleDocumentsPerDay> SaleDocumentsPerDays { get; set; }
        public DbSet<SalesPerDayItem> SalesPerDayItems { get; set; }
        public DbSet<RefundsPerDayItem> RefundsPerDayItems { get; set; }
        public DbSet<RemaindersUserChange> RemaindersUserChanges { get; set; }
        public DbSet<GearNew> GearNews { get; set; }
        public DbSet<WarehouseTransferRequestItem> WarehouseTransferRequestItems { get; set; }
        public DbSet<WarehouseTransferRequest> WarehouseTransferRequests { get; set; }
        public DbSet<PriceChangeReportItem> PriceChangeReportItems { get; set; } 
        public DbSet<PriceChangeReport> PriceChangeReports { get; set; }

        public DbSet<Resource> Resources { get; set; }

        //public DbSet<PriceListPriceItem> PriceListPriceItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SaleDocument>()
                .HasMany(e => e.SaleItems)
                .WithRequired(e => e.SaleDocument)
                .HasForeignKey(e => e.SaleDocument_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SaleDocument>()
                .HasRequired(r => r.Customer)
                .WithMany().HasForeignKey(c => c.Customer_Name).WillCascadeOnDelete(false);
            modelBuilder.Entity<SaleDocument>()
                .HasRequired(r => r.Creator)
                .WithMany().HasForeignKey(c => c.Creator_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<SaleDocument>()
                .HasRequired(r => r.LastChanger)
                .WithMany().HasForeignKey(c => c.LastChanger_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<SaleItem>()
                .HasRequired(r => r.PriceItem)
                .WithMany().HasForeignKey(c => c.PriceItem_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<SaleItem>()
                .HasRequired(r => r.SaleDocument)
                .WithMany().HasForeignKey(c => c.SaleDocument_Id).WillCascadeOnDelete(false);



            //modelBuilder.Entity<DebtDischargeDocument>()
            //    .HasRequired(r => r.SaleDocument)
            //    .WithMany().HasForeignKey(c => c.SaleDocument_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<DebtDischargeDocument>()
                .HasRequired(r => r.Debtor)
                .WithMany().HasForeignKey(c => c.Debtor_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<DebtDischargeDocument>()
                .HasRequired(r => r.Creator)
                .WithMany().HasForeignKey(c => c.Creator_Id).WillCascadeOnDelete(false);


            modelBuilder.Entity<Gear>()
              .HasRequired(r => r.GearCategory)
              .WithMany().HasForeignKey(c => c.Category_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<Gear>()
              .HasOptional(r => r.Image)
              .WithMany().HasForeignKey(c => c.Image_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<GearCategory>()
                .HasMany(e => e.Gears)
                .WithRequired(e => e.GearCategory)
                .HasForeignKey(e => e.Category_Id)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<Income>()
                .HasRequired(r => r.Creator)
                .WithMany().HasForeignKey(c => c.Creator_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<Income>()
                .HasRequired(r => r.Supplier)
                .WithMany().HasForeignKey(c => c.Supplier_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<Income>()
                .HasOptional(r => r.Accepter)
                .WithMany().HasForeignKey(c => c.Accepter_Id).WillCascadeOnDelete(false);


            modelBuilder.Entity<IncomeItem>()
                .HasRequired(r => r.Income)
                .WithMany().HasForeignKey(c => c.Income_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<IncomeItem>()
                .HasRequired(r => r.PriceItem)
                .WithMany().HasForeignKey(c => c.PriceItem_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<Income>()
                .HasMany(e => e.IncomeItems)
                .WithRequired(e => e.Income)
                .HasForeignKey(e => e.Income_Id)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<PriceItem>()
                .HasMany(e => e.Remainders)
                .WithRequired(e => e.PriceItem)
                .HasForeignKey(e => e.PriceItem_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PriceItem>()
                .HasMany(e => e.Prices)
                .WithRequired(e => e.PriceItem)
                .HasForeignKey(e => e.PriceItem_Id)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<PriceItem>()
            //    .HasMany(e => e.RetailPrices)
            //    .WithRequired(e => e.PriceItem)
            //    .HasForeignKey(e => e.PriceItem_Id)
            //    .WillCascadeOnDelete(false);


            modelBuilder.Entity<PriceItem>()
                .HasRequired(r => r.Gear)
                .WithMany().HasForeignKey(c => c.Gear_Id).WillCascadeOnDelete(false);
            //modelBuilder.Entity<PriceItem>()
            //    .HasRequired(r => r.PriceList)
            //    .WithMany().HasForeignKey(c => c.PriceList_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<PriceItem>()
                .HasRequired(r => r.UnitOfMeasure)
                .WithMany().HasForeignKey(c => c.Uom_Id).WillCascadeOnDelete(false);



            modelBuilder.Entity<PriceList>()
                .HasMany(e => e.PriceItems)
                .WithMany(m => m.PriceLists);
                //.HasForeignKey(e => e.PriceList_Id)
                //.WillCascadeOnDelete(false);

            modelBuilder.Entity<PriceList>()
                .HasRequired(r => r.UploadUser)
                .WithMany().HasForeignKey(c => c.UploadUser_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<PriceList>()
                .HasMany(m => m.PriceItems)
                .WithMany(m => m.PriceLists)
                .Map(m => m.ToTable("PriceListPriceItems")
                    .MapLeftKey("PriceList_Name").MapRightKey("PriceItem_Id"));

            modelBuilder.Entity<RefundDocument>()
                .HasMany(e => e.RefundItems)
                .WithRequired(e => e.RefundDocument)
                .HasForeignKey(e => e.RefundDocument_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RefundDocument>()
                .HasRequired(r => r.SaleDocument)
                .WithMany().HasForeignKey(c => c.SaleDocument_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<RefundDocument>()
                .HasRequired(r => r.Creator)
                .WithMany().HasForeignKey(c => c.Creator_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<RefundDocument>()
                .HasRequired(r => r.LastChanger)
                .WithMany().HasForeignKey(c => c.LastChanger_Id).WillCascadeOnDelete(false);


            modelBuilder.Entity<RefundItem>()
                .HasRequired(r => r.PriceItem)
                .WithMany().HasForeignKey(c => c.PriceItem_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<RefundItem>()
                .HasRequired(r => r.RefundDocument)
                .WithMany().HasForeignKey(c => c.RefundDocument_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<RefundItem>()
                .HasRequired(r => r.SaleItem)
                .WithMany().HasForeignKey(c => c.SaleItem_Id).WillCascadeOnDelete(false);



            modelBuilder.Entity<Remainder>()
                .HasRequired(r => r.PriceItem)
                .WithMany().HasForeignKey(c => c.PriceItem_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<Remainder>()
                .HasRequired(r => r.Warehouse)
                .WithMany().HasForeignKey(c => c.Warehouse_Id).WillCascadeOnDelete(false);


            modelBuilder.Entity<WholesalePrice>()
                .HasRequired(r => r.PriceItem)
                .WithMany().HasForeignKey(c => c.PriceItem_Id).WillCascadeOnDelete(false);

            //modelBuilder.Entity<RetailPrice>()
            //    .HasRequired(r => r.PriceItem)
            //    .WithMany().HasForeignKey(c => c.PriceItem_Id).WillCascadeOnDelete(false);
            //modelBuilder.Entity<RetailPrice>()
            //    .HasRequired(r => r.Author)
            //    .WithMany().HasForeignKey(c => c.Author_Id).WillCascadeOnDelete(false);


            modelBuilder.Entity<SaleDocumentsPerDay>()
                .HasMany(e => e.SalesPerDayItems)
                .WithRequired(e => e.SaleDocumentsPerDay)
                .HasForeignKey(e => e.SaleDocumentsPerDay_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SaleDocumentsPerDay>()
                .HasMany(e => e.RefundsPerDayItems)
                .WithRequired(e => e.SaleDocumentsPerDay)
                .HasForeignKey(e => e.SaleDocumentsPerDay_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SaleDocumentsPerDay>()
                .HasRequired(r => r.Creator)
                .WithMany().HasForeignKey(c => c.Creator_Id).WillCascadeOnDelete(false);



            modelBuilder.Entity<SalesPerDayItem>()
                .HasRequired(r => r.SaleItem)
                .WithMany().HasForeignKey(c => c.SaleItem_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<SalesPerDayItem>()
                .HasRequired(r => r.SaleDocumentsPerDay)
                .WithMany().HasForeignKey(c => c.SaleDocumentsPerDay_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<RefundsPerDayItem>()
                .HasRequired(r => r.RefundItem)
                .WithMany().HasForeignKey(c => c.RefundItem_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<RefundsPerDayItem>()
                .HasRequired(r => r.SaleDocumentsPerDay)
                .WithMany().HasForeignKey(c => c.SaleDocumentsPerDay_Id).WillCascadeOnDelete(false);


            modelBuilder.Entity<RemaindersUserChange>()
                .HasRequired(r => r.User)
                .WithMany().HasForeignKey(c => c.User_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<RemaindersUserChange>()
                .HasRequired(r => r.Remainder)
                .WithMany().HasForeignKey(c => c.Remainder_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<Remainder>()
                .HasMany(e => e.RemaindersUserChanges)
                .WithRequired(e => e.Remainder)
                .HasForeignKey(e => e.Remainder_Id)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<GearNew>()
                .HasRequired(r => r.Gear)
                .WithMany().HasForeignKey(c => c.Gear_Id).WillCascadeOnDelete(false);



            modelBuilder.Entity<WarehouseTransferRequest>()
                .HasRequired(r => r.Creator)
                .WithMany().HasForeignKey(c => c.Creator_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<WarehouseTransferRequest>()
                .HasOptional(r => r.LastChanged)
                .WithMany().HasForeignKey(c => c.LastChanged_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<WarehouseTransferRequest>()
                .HasRequired(r => r.Supplier)
                .WithMany().HasForeignKey(c => c.Supplier_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<WarehouseTransferRequest>()
                .HasRequired(r => r.Customer)
                .WithMany().HasForeignKey(c => c.Customer_Id).WillCascadeOnDelete(false);


            modelBuilder.Entity<WarehouseTransferRequestItem>()
                .HasRequired(r => r.WarehouseTransferRequest)
                .WithMany().HasForeignKey(c => c.WarehouseTransferRequest_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<WarehouseTransferRequestItem>()
                .HasRequired(r => r.PriceItem)
                .WithMany().HasForeignKey(c => c.PriceItem_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<WarehouseTransferRequest>()
                .HasMany(e => e.WarehouseTransferRequestItemItems)
                .WithRequired(e => e.WarehouseTransferRequest)
                .HasForeignKey(e => e.WarehouseTransferRequest_Id)
                .WillCascadeOnDelete(false);



            modelBuilder.Entity<PriceChangeReport>()
                .HasRequired(r => r.Creator)
                .WithMany().HasForeignKey(c => c.Creator_Id).WillCascadeOnDelete(false);


            modelBuilder.Entity<PriceChangeReportItem>()
                .HasRequired(r => r.PriceChangeReport)
                .WithMany().HasForeignKey(c => c.PriceChangeReport_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<PriceChangeReportItem>()
                .HasRequired(r => r.PriceItem)
                .WithMany().HasForeignKey(c => c.PriceItem_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<PriceChangeReportItem>()
                .HasRequired(r => r.PreviousPrice)
                .WithMany().HasForeignKey(c => c.PreviousPrice_Id).WillCascadeOnDelete(false);
            modelBuilder.Entity<PriceChangeReportItem>()
                .HasRequired(r => r.NewPrice)
                .WithMany().HasForeignKey(c => c.NewPrice_Id).WillCascadeOnDelete(false);

            modelBuilder.Entity<PriceChangeReport>()
                .HasMany(e => e.PriceChangeReportItems)
                .WithRequired(e => e.PriceChangeReport)
                .HasForeignKey(e => e.PriceChangeReport_Id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasRequired(r => r.Creator)
                .WithMany().HasForeignKey(c => c.Creator_Id).WillCascadeOnDelete(false);


            modelBuilder.Entity<PriceItemRemainderView>()
                .ToTable("PriceItemRemainder_VIEW");

            modelBuilder.Entity<PriceIncomeItemView>()
                .ToTable("PriceIncomeItem_VIEW");

            modelBuilder.Entity<PriceIncomeTotalItemView>()
                .ToTable("PriceTotalIncomeItem_VIEW");

            modelBuilder.Entity<PriceItemView>()
                .ToTable("PriceItemView_VIEW");

            modelBuilder.Entity<PriceListItemRemainderView>()
                .ToTable("PriceListItemRemainder_VIEW");

            //modelBuilder.Entity<PriceListPriceItem>()
            //    .ToTable("PriceListPriceItems")
            //    .HasKey(k => k.PriceList_Name)
            //    .HasKey(k => k.PriceItem_Id);

            modelBuilder.Entity<Resource>()
                .HasKey(k => k.Id);

                //.HasRequired(r => r.PriceList_Name).WithMany()
                //.WithMany()
            modelBuilder.Entity<User>()
                .HasOptional(o => o.Warehouse)
                .WithMany()
                .HasForeignKey(f => f.Warehouse_Id);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithMany(e => e.Users);

        }
    }

    //public class StoreDbInitializer : DropCreateDatabaseAlways<StoreDbContext>
    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<StoreDbContext>
    {
        #region Overrides of DropCreateDatabaseIfModelChanges<StoreDbContext>

        /// <summary>
        /// A method that should be overridden to actually add data to the context for seeding.
        ///             The default implementation does nothing.
        /// </summary>
        /// <param name="context">The context to seed. </param>
        protected override void Seed(StoreDbContext context)
        {

            Warehouse wh = new Warehouse();
            wh.Name = "Основной";
            context.Warehouses.Add(wh);

            User usr = new User();
            usr.DisplayName = "Администратор";
            usr.UserName = "admin";
            usr.Warehouse_Id = wh.Name;
            usr.Warehouse = wh;
            usr.IsSupplierVisible = true;

            var securePwd = SecUtility.Encrypt("store_app_test", "master");
            var pwdBytes = Encoding.Unicode.GetBytes(securePwd);
            usr.PasswordHash = pwdBytes;
            context.Users.Add(usr);

            Customer cs = new Customer();
            cs.Name = "Розничный покупатель";
            cs.Creator_Id = "admin";
            context.Customers.Add(cs);

            Customer cse = new Customer();
            cse.Name = "Клиент интернет-магазина";
            cse.Creator_Id = "admin";
            context.Customers.Add(cse);

            GearCategory gc1 = new GearCategory();
            gc1.Name = "Новинки";
            context.GearCategories.Add(gc1);

            GearCategory gc2 = new GearCategory();
            gc2.Name = "Отстой";
            context.GearCategories.Add(gc2);

            GearCategory gc3 = new GearCategory();
            gc3.Name = "Обычные";
            context.GearCategories.Add(gc3);

            base.Seed(context);
        }

        #endregion
    }
}
