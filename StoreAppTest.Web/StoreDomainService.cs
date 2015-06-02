
namespace StoreAppTest.Web
{
    extern alias ef;

    using System.Linq;
    using OpenRiaServices.DomainServices.EntityFramework;
    using OpenRiaServices.DomainServices.Hosting;
    using DataModel;
    using EntityState = ef::System.Data.Entity.EntityState;


    // Implements application logic using the StoreDbContext context.
    // TODO: Add your application logic to these methods or in additional methods.
    // TODO: Wire up authentication (Windows/ASP.NET Forms) and uncomment the following to disable anonymous access
    // Also consider adding roles to restrict access as appropriate.
    // [RequiresAuthentication]
    [EnableClientAccess()]
    public class StoreDomainService : DbDomainService<StoreDbContext>
    {

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Customers' query.
        public IQueryable<Customer> GetCustomers()
        {
            return this.DbContext.Customers;
        }

        public void InsertCustomer(Customer customer)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Customer> entityEntry = this.DbContext.Entry(customer);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Customers.Add(customer);
            }
        }

        public void UpdateCustomer(Customer currentCustomer)
        {
            this.DbContext.Customers.AttachAsModified(currentCustomer, this.ChangeSet.GetOriginal(currentCustomer), this.DbContext);
        }

        public void DeleteCustomer(Customer customer)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Customer> entityEntry = this.DbContext.Entry(customer);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Customers.Attach(customer);
                this.DbContext.Customers.Remove(customer);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'DebtDischargeDocuments' query.
        public IQueryable<DebtDischargeDocument> GetDebtDischargeDocuments()
        {
            return this.DbContext.DebtDischargeDocuments;
        }

        public void InsertDebtDischargeDocument(DebtDischargeDocument debtDischargeDocument)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<DebtDischargeDocument> entityEntry = this.DbContext.Entry(debtDischargeDocument);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.DebtDischargeDocuments.Add(debtDischargeDocument);
            }
        }

        public void UpdateDebtDischargeDocument(DebtDischargeDocument currentDebtDischargeDocument)
        {
            this.DbContext.DebtDischargeDocuments.AttachAsModified(currentDebtDischargeDocument, this.ChangeSet.GetOriginal(currentDebtDischargeDocument), this.DbContext);
        }

        public void DeleteDebtDischargeDocument(DebtDischargeDocument debtDischargeDocument)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<DebtDischargeDocument> entityEntry = this.DbContext.Entry(debtDischargeDocument);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.DebtDischargeDocuments.Attach(debtDischargeDocument);
                this.DbContext.DebtDischargeDocuments.Remove(debtDischargeDocument);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Gears' query.
        public IQueryable<Gear> GetGears()
        {
            return this.DbContext.Gears;
        }

        public void InsertGear(Gear gear)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Gear> entityEntry = this.DbContext.Entry(gear);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Gears.Add(gear);
            }
        }

        public void UpdateGear(Gear currentGear)
        {
            this.DbContext.Gears.AttachAsModified(currentGear, this.ChangeSet.GetOriginal(currentGear), this.DbContext);
        }

        public void DeleteGear(Gear gear)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Gear> entityEntry = this.DbContext.Entry(gear);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Gears.Attach(gear);
                this.DbContext.Gears.Remove(gear);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'GearCategories' query.
        public IQueryable<GearCategory> GetGearCategories()
        {
            return this.DbContext.GearCategories;
        }

        public void InsertGearCategory(GearCategory gearCategory)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<GearCategory> entityEntry = this.DbContext.Entry(gearCategory);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.GearCategories.Add(gearCategory);
            }
        }

        public void UpdateGearCategory(GearCategory currentGearCategory)
        {
            this.DbContext.GearCategories.AttachAsModified(currentGearCategory, this.ChangeSet.GetOriginal(currentGearCategory), this.DbContext);
        }

        public void DeleteGearCategory(GearCategory gearCategory)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<GearCategory> entityEntry = this.DbContext.Entry(gearCategory);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.GearCategories.Attach(gearCategory);
                this.DbContext.GearCategories.Remove(gearCategory);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Incomes' query.
        public IQueryable<Income> GetIncomes()
        {
            return this.DbContext.Incomes;
        }

        public void InsertIncome(Income income)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Income> entityEntry = this.DbContext.Entry(income);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Incomes.Add(income);
            }
        }

        public void UpdateIncome(Income currentIncome)
        {
            this.DbContext.Incomes.AttachAsModified(currentIncome, this.ChangeSet.GetOriginal(currentIncome), this.DbContext);
        }

        public void DeleteIncome(Income income)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Income> entityEntry = this.DbContext.Entry(income);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Incomes.Attach(income);
                this.DbContext.Incomes.Remove(income);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'IncomeItems' query.
        public IQueryable<IncomeItem> GetIncomeItems()
        {
            return this.DbContext.IncomeItems;
        }

        public void InsertIncomeItem(IncomeItem incomeItem)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<IncomeItem> entityEntry = this.DbContext.Entry(incomeItem);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.IncomeItems.Add(incomeItem);
            }
        }

        public void UpdateIncomeItem(IncomeItem currentIncomeItem)
        {
            this.DbContext.IncomeItems.AttachAsModified(currentIncomeItem, this.ChangeSet.GetOriginal(currentIncomeItem), this.DbContext);
        }

        public void DeleteIncomeItem(IncomeItem incomeItem)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<IncomeItem> entityEntry = this.DbContext.Entry(incomeItem);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.IncomeItems.Attach(incomeItem);
                this.DbContext.IncomeItems.Remove(incomeItem);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'PriceItems' query.
        public IQueryable<PriceItem> GetPriceItems()
        {
            return this.DbContext.PriceItems;
        }

        public void InsertPriceItem(PriceItem priceItem)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<PriceItem> entityEntry = this.DbContext.Entry(priceItem);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.PriceItems.Add(priceItem);
            }
        }

        public void UpdatePriceItem(PriceItem currentPriceItem)
        {
            this.DbContext.PriceItems.AttachAsModified(currentPriceItem, this.ChangeSet.GetOriginal(currentPriceItem), this.DbContext);
        }

        public void DeletePriceItem(PriceItem priceItem)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<PriceItem> entityEntry = this.DbContext.Entry(priceItem);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.PriceItems.Attach(priceItem);
                this.DbContext.PriceItems.Remove(priceItem);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'PriceLists' query.
        public IQueryable<PriceList> GetPriceLists()
        {
            return this.DbContext.PriceLists;
        }

        public void InsertPriceList(PriceList priceList)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<PriceList> entityEntry = this.DbContext.Entry(priceList);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.PriceLists.Add(priceList);
            }
        }

        public void UpdatePriceList(PriceList currentPriceList)
        {
            this.DbContext.PriceLists.AttachAsModified(currentPriceList, this.ChangeSet.GetOriginal(currentPriceList), this.DbContext);
        }

        public void DeletePriceList(PriceList priceList)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<PriceList> entityEntry = this.DbContext.Entry(priceList);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.PriceLists.Attach(priceList);
                this.DbContext.PriceLists.Remove(priceList);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'RefundDocuments' query.
        public IQueryable<RefundDocument> GetRefundDocuments()
        {
            return this.DbContext.RefundDocuments;
        }

        public void InsertRefundDocument(RefundDocument refundDocument)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<RefundDocument> entityEntry = this.DbContext.Entry(refundDocument);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.RefundDocuments.Add(refundDocument);
            }
        }

        public void UpdateRefundDocument(RefundDocument currentRefundDocument)
        {
            this.DbContext.RefundDocuments.AttachAsModified(currentRefundDocument, this.ChangeSet.GetOriginal(currentRefundDocument), this.DbContext);
        }

        public void DeleteRefundDocument(RefundDocument refundDocument)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<RefundDocument> entityEntry = this.DbContext.Entry(refundDocument);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.RefundDocuments.Attach(refundDocument);
                this.DbContext.RefundDocuments.Remove(refundDocument);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'RefundItems' query.
        public IQueryable<RefundItem> GetRefundItems()
        {
            return this.DbContext.RefundItems;
        }

        public void InsertRefundItem(RefundItem refundItem)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<RefundItem> entityEntry = this.DbContext.Entry(refundItem);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.RefundItems.Add(refundItem);
            }
        }

        public void UpdateRefundItem(RefundItem currentRefundItem)
        {
            this.DbContext.RefundItems.AttachAsModified(currentRefundItem, this.ChangeSet.GetOriginal(currentRefundItem), this.DbContext);
        }

        public void DeleteRefundItem(RefundItem refundItem)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<RefundItem> entityEntry = this.DbContext.Entry(refundItem);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.RefundItems.Attach(refundItem);
                this.DbContext.RefundItems.Remove(refundItem);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Remainders' query.
        public IQueryable<Remainder> GetRemainders()
        {
            return this.DbContext.Remainders;
        }

        public void InsertRemainder(Remainder remainder)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Remainder> entityEntry = this.DbContext.Entry(remainder);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Remainders.Add(remainder);
            }
        }

        public void UpdateRemainder(Remainder currentRemainder)
        {
            this.DbContext.Remainders.AttachAsModified(currentRemainder, this.ChangeSet.GetOriginal(currentRemainder), this.DbContext);
        }

        public void DeleteRemainder(Remainder remainder)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Remainder> entityEntry = this.DbContext.Entry(remainder);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Remainders.Attach(remainder);
                this.DbContext.Remainders.Remove(remainder);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'RetailPrices' query.
        public IQueryable<RetailPrice> GetRetailPrices()
        {
            return this.DbContext.RetailPrices;
        }

        public void InsertRetailPrice(RetailPrice retailPrice)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<RetailPrice> entityEntry = this.DbContext.Entry(retailPrice);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.RetailPrices.Add(retailPrice);
            }
        }

        public void UpdateRetailPrice(RetailPrice currentRetailPrice)
        {
            this.DbContext.RetailPrices.AttachAsModified(currentRetailPrice, this.ChangeSet.GetOriginal(currentRetailPrice), this.DbContext);
        }

        public void DeleteRetailPrice(RetailPrice retailPrice)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<RetailPrice> entityEntry = this.DbContext.Entry(retailPrice);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.RetailPrices.Attach(retailPrice);
                this.DbContext.RetailPrices.Remove(retailPrice);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SaleDocuments' query.
        public IQueryable<SaleDocument> GetSaleDocuments()
        {
            return this.DbContext.SaleDocuments;
        }

        public void InsertSaleDocument(SaleDocument saleDocument)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<SaleDocument> entityEntry = this.DbContext.Entry(saleDocument);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.SaleDocuments.Add(saleDocument);
            }
        }

        public void UpdateSaleDocument(SaleDocument currentSaleDocument)
        {
            this.DbContext.SaleDocuments.AttachAsModified(currentSaleDocument, this.ChangeSet.GetOriginal(currentSaleDocument), this.DbContext);
        }

        public void DeleteSaleDocument(SaleDocument saleDocument)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<SaleDocument> entityEntry = this.DbContext.Entry(saleDocument);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.SaleDocuments.Attach(saleDocument);
                this.DbContext.SaleDocuments.Remove(saleDocument);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'SaleItems' query.
        public IQueryable<SaleItem> GetSaleItems()
        {
            return this.DbContext.SaleItems;
        }

        public void InsertSaleItem(SaleItem saleItem)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<SaleItem> entityEntry = this.DbContext.Entry(saleItem);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.SaleItems.Add(saleItem);
            }
        }

        public void UpdateSaleItem(SaleItem currentSaleItem)
        {
            this.DbContext.SaleItems.AttachAsModified(currentSaleItem, this.ChangeSet.GetOriginal(currentSaleItem), this.DbContext);
        }

        public void DeleteSaleItem(SaleItem saleItem)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<SaleItem> entityEntry = this.DbContext.Entry(saleItem);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.SaleItems.Attach(saleItem);
                this.DbContext.SaleItems.Remove(saleItem);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Suppliers' query.
        public IQueryable<Supplier> GetSuppliers()
        {
            return this.DbContext.Suppliers;
        }

        public void InsertSupplier(Supplier supplier)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Supplier> entityEntry = this.DbContext.Entry(supplier);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Suppliers.Add(supplier);
            }
        }

        public void UpdateSupplier(Supplier currentSupplier)
        {
            this.DbContext.Suppliers.AttachAsModified(currentSupplier, this.ChangeSet.GetOriginal(currentSupplier), this.DbContext);
        }

        public void DeleteSupplier(Supplier supplier)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Supplier> entityEntry = this.DbContext.Entry(supplier);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Suppliers.Attach(supplier);
                this.DbContext.Suppliers.Remove(supplier);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'UnitOfMeasures' query.
        public IQueryable<UnitOfMeasure> GetUnitOfMeasures()
        {
            return this.DbContext.UnitOfMeasures;
        }

        public void InsertUnitOfMeasure(UnitOfMeasure unitOfMeasure)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<UnitOfMeasure> entityEntry = this.DbContext.Entry(unitOfMeasure);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.UnitOfMeasures.Add(unitOfMeasure);
            }
        }

        public void UpdateUnitOfMeasure(UnitOfMeasure currentUnitOfMeasure)
        {
            this.DbContext.UnitOfMeasures.AttachAsModified(currentUnitOfMeasure, this.ChangeSet.GetOriginal(currentUnitOfMeasure), this.DbContext);
        }

        public void DeleteUnitOfMeasure(UnitOfMeasure unitOfMeasure)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<UnitOfMeasure> entityEntry = this.DbContext.Entry(unitOfMeasure);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.UnitOfMeasures.Attach(unitOfMeasure);
                this.DbContext.UnitOfMeasures.Remove(unitOfMeasure);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Users' query.
        public IQueryable<User> GetUsers()
        {
            return this.DbContext.Users;
        }

        public void InsertUser(User user)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<User> entityEntry = this.DbContext.Entry(user);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Users.Add(user);
            }
        }

        public void UpdateUser(User currentUser)
        {
            this.DbContext.Users.AttachAsModified(currentUser, this.ChangeSet.GetOriginal(currentUser), this.DbContext);
        }

        public void DeleteUser(User user)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<User> entityEntry = this.DbContext.Entry(user);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Users.Attach(user);
                this.DbContext.Users.Remove(user);
            }
        }

        // TODO:
        // Consider constraining the results of your query method.  If you need additional input you can
        // add parameters to this method or create additional query methods with different names.
        // To support paging you will need to add ordering to the 'Warehouses' query.
        public IQueryable<Warehouse> GetWarehouses()
        {
            return this.DbContext.Warehouses;
        }

        public void InsertWarehouse(Warehouse warehouse)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Warehouse> entityEntry = this.DbContext.Entry(warehouse);
            if ((entityEntry.State != EntityState.Detached))
            {
                entityEntry.State = EntityState.Added;
            }
            else
            {
                this.DbContext.Warehouses.Add(warehouse);
            }
        }

        public void UpdateWarehouse(Warehouse currentWarehouse)
        {
            this.DbContext.Warehouses.AttachAsModified(currentWarehouse, this.ChangeSet.GetOriginal(currentWarehouse), this.DbContext);
        }

        public void DeleteWarehouse(Warehouse warehouse)
        {
            ef::System.Data.Entity.Infrastructure.DbEntityEntry<Warehouse> entityEntry = this.DbContext.Entry(warehouse);
            if ((entityEntry.State != EntityState.Deleted))
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                this.DbContext.Warehouses.Attach(warehouse);
                this.DbContext.Warehouses.Remove(warehouse);
            }
        }
    }
}


