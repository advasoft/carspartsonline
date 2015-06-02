//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Data.Services;
using System.Data.Services.Common;

namespace StoreAppTest.Web
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Services.Providers;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Remoting.Contexts;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Web;
    using DataModel;

    public class StoreAppDataService : EntityFrameworkDataService<StoreDbContext>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<PriceItemRemainderView> GetLowerLimitPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("PR.WholesalePrice AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("GR.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("GR.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Id AS Remainder_Id, ");
            sb.Append("RM.Warehouse_Id AS Warehouse, ");
            sb.Append("RM.RemainderDate AS RemainderDate, ");
            sb.Append("RM.Amount AS Remainders ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("WHERE RM.Amount <= GR.LowerLimitRemainder ");

            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("AND PR.PriceList_Id = @p0 ");
                parameters[0] = priceListName;
            }

            var query = CurrentDataSource.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query;

        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<PriceItemRemainderView> GetNewsPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("GR.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("GR.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Id AS Remainder_Id, ");
            sb.Append("RM.Warehouse_Id AS Warehouse, ");
            sb.Append("RM.RemainderDate AS RemainderDate, ");
            sb.Append("RM.Amount AS Remainders, ");
            sb.Append("WHPS.Price AS WholesalePrice ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT Gear_Id, MAX(CreatedDate) AS CreatedDate FROM [dbo].[GearNews]  ");
            sb.Append("GROUP BY Gear_Id ");
            sb.Append(") AS GRN ON GR.Id = GRN.Gear_Id ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate ");
            sb.Append("FROM [WholesalePrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id ");
            sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");

            sb.Append(" WHERE YEAR(GRN.CreatedDate) = YEAR(GETDATE()) AND MONTH(GRN.CreatedDate) >= MONTH(DATEADD(MONTH, -1 , GETDATE())) ");

            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("AND PR.PriceList_Id = @p0 ");
                parameters[0] = priceListName;
            }

            var query = CurrentDataSource.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query.AsNoTracking();

        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<PriceItemRemainderView> GetSucksPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("PR.WholesalePrice AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("GR.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("GR.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Id AS Remainder_Id, ");
            sb.Append("RM.Warehouse_Id AS Warehouse, ");
            sb.Append("RM.RemainderDate AS RemainderDate, ");
            sb.Append("RM.Amount AS Remainders ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("WHERE YEAR(RM.RemainderDate) = YEAR(GETDATE()) AND MONTH(RM.RemainderDate) = MONTH(DATEADD(MONTH, -3 , GETDATE())) AND RM.Amount = 0 ");

            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("AND PR.PriceList_Id = @p0 ");
                parameters[0] = priceListName;
            }

            var query = CurrentDataSource.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query;

        }


        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<PriceItemRemainderView> GetIncomesPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("PR.WholesalePrice AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("GR.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("GR.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Id AS Remainder_Id, ");
            sb.Append("RM.Warehouse_Id AS Warehouse, ");
            sb.Append("RM.RemainderDate AS RemainderDate, ");
            sb.Append("INC.Amount AS Remainders ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("JOIN [dbo].[IncomeItems] AS INC ON PR.Id = INC.PriceItem_Id ");
            sb.Append("JOIN [dbo].[Incomes] AS INCO ON INC.Income_Id = INCO.Id ");
            sb.Append("WHERE INCO.IsAccept = 0 ");

            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("AND PR.PriceList_Id = @p0 ");
                parameters[0] = priceListName;
            }

            var query = CurrentDataSource.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query;

        }


        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<PriceItemRemainderView> GetAllPriceItems(string priceListName)
        {
            object[] parameters = new object[1];

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("GR.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("GR.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Id AS Remainder_Id, ");
            sb.Append("RM.Warehouse_Id AS Warehouse, ");
            sb.Append("RM.RemainderDate AS RemainderDate, ");
            sb.Append("RM.Amount AS Remainders, ");
            sb.Append("WHPS.Price AS WholesalePrice ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
            sb.Append("JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("JOIN [dbo].[Remainders] AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate ");
            sb.Append("FROM [WholesalePrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id ");
            sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");
            if (!string.IsNullOrEmpty(priceListName))
            {
                sb.Append("WHERE PRPRL.PriceList_Name = @p0 ");
                parameters[0] = priceListName;
            }

            var query = CurrentDataSource.Set<PriceItemRemainderView>()
                .SqlQuery(sb.ToString(), parameters);


            return query.AsNoTracking();

        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<PriceItemRemainderView> GetLightPriceItemList(string priceListName, string warehouse = "")
        {

            var parameters = new ArrayList() { priceListName };

            StringBuilder sb = new StringBuilder();


            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("WHPS.Price AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("GR.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("GR.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("CAST(0 AS BIGINT) AS Remainder_Id, ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("RM.Warehouse_Id AS Warehouse, ");
            }
            else
            {
                sb.Append("'' AS Warehouse, ");   
            }
            sb.Append("convert(datetime,'01.01.2015') AS RemainderDate, ");
            sb.Append("ISNULL(RM.Amount, 0) AS Remainders, ");
            sb.Append("'' AS Supplier ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("LEFT JOIN ( ");
            sb.Append("SELECT PriceItem_Id, ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("Warehouse_Id, ");
            }
            sb.Append(" SUM(Amount) AS Amount ");

            sb.Append("FROM [dbo].[Remainders] ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("WHERE Warehouse_Id = @p1 ");
                parameters.Add(warehouse);
            }
            sb.Append("GROUP BY PriceItem_Id ");

            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append(",Warehouse_Id ");
            }
            sb.Append(") AS RM ON PR.Id = RM.PriceItem_Id ");


sb.Append("JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
  sb.Append("JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
  sb.Append("LEFT JOIN  ");
  sb.Append("( ");
	sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate ");
	sb.Append("FROM [WholesalePrices] ");
	sb.Append("GROUP BY PriceItem_Id ");
  sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id ");
  sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");

  sb.Append("WHERE PRPRL.PriceList_Name = @p0 ");




            var query = CurrentDataSource.Set<PriceItemRemainderView>()
          .SqlQuery(sb.ToString(), parameters.ToArray());


            return query;

        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<PriceIncomeItemView> GetNotAcceptedIncomes(string priceListName)
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("PR.WholesalePrice AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("GR.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("GR.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Amount AS Remainders, ");
            sb.Append("INCS.Amount AS Incomes ");
            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("LEFT JOIN ( ");
            sb.Append("SELECT PriceItem_Id, SUM(Amount) AS Amount ");
            sb.Append("FROM [dbo].[Remainders] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("LEFT JOIN ( ");

            sb.Append("SELECT INC.PriceItem_Id, SUM(INC.Amount) AS Amount ");
            sb.Append("FROM [dbo].[IncomeItems] AS INC ");
            sb.Append("JOIN [dbo].[Incomes] AS INCO ON INC.Income_Id = INCO.Id ");
            sb.Append("WHERE INCO.IsAccept = 0 ");
            sb.Append("GROUP BY INC.PriceItem_Id ");

            sb.Append(") AS INCS ON PR.Id = INCS.PriceItem_Id ");
            sb.Append("WHERE PR.PriceList_Id = @p0 ");


            var query = CurrentDataSource.Set<PriceIncomeItemView>()
                .SqlQuery(sb.ToString(), priceListName);


            return query;


        }


        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<PriceIncomeTotalItemView> GetTotalIncomes(string priceListName)
        {

            StringBuilder sb = new StringBuilder();


            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("PR.WholesalePrice AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("GR.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("GR.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("RM.Amount AS Remainders ");
            sb.Append(",INCI.Amount AS Incomes ");
            sb.Append(",INCI.Income_Id AS Income_Id ");
            sb.Append(",INC.IsAccept ");
            sb.Append(",PRS.Price AS NewPrice ");
            sb.Append("FROM [IncomeItems] AS INCI ");
            sb.Append("JOIN [Incomes] AS INC ON INCI.Income_Id = INC.Id ");
            sb.Append("JOIN [PriceItems] AS PR ON INCI.PriceItem_Id = PR.Id ");
            sb.Append("LEFT JOIN ( ");
            sb.Append("SELECT PriceItem_Id, SUM(Amount) AS Amount ");
            sb.Append("FROM [Remainders] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS RM ON PR.Id = RM.PriceItem_Id ");
            sb.Append("LEFT JOIN [Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT MAX(PriceDate) AS PriceDate, PriceItem_Id ");
            sb.Append("FROM [RetailPrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS PRSD ON PR.Id = PRSD.PriceItem_Id ");
            sb.Append("LEFT JOIN [RetailPrices] AS PRS ON PRSD.PriceDate = PRS.PriceDate AND PRSD.PriceItem_Id = PRS.PriceItem_Id ");

            sb.Append("WHERE PR.PriceList_Id = @p0 ");

            var query = CurrentDataSource.Set<PriceIncomeTotalItemView>()
                .SqlQuery(sb.ToString(), priceListName);


            return query;


        }

        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        public IEnumerable<PriceItemRemainderView> GetLightAllPriceItemList(string warehouse = "")
        {

            var parameters = new ArrayList() {  };

            StringBuilder sb = new StringBuilder();


            sb.Append("SELECT  ");

            sb.Append("PR.Id AS PriceItem_Id, ");
            sb.Append("PR.BuyPriceRur AS BuyPriceRur, ");
            sb.Append("PR.BuyPriceTng AS BuyPriceTng, ");
            sb.Append("WHPS.Price AS WholesalePrice, ");
            sb.Append("PR.Uom_Id AS Uom, ");
            sb.Append("GR.Id AS Gear_Id, ");
            sb.Append("GR.CatalogNumber AS CatalogNumber, ");
            sb.Append("GR.Articul AS Articul, ");
            sb.Append("GR.IsDuplicate AS IsDuplicate, ");
            sb.Append("GR.Name AS Gear_Name, ");
            sb.Append("GR.RecommendedRemainder AS RecommendedRemainder, ");
            sb.Append("GR.LowerLimitRemainder AS LowerLimitRemainder, ");
            sb.Append("CAST(0 AS BIGINT) AS Remainder_Id, ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("RM.Warehouse_Id AS Warehouse, ");
            }
            else
            {
                sb.Append("'' AS Warehouse, ");
            }
            sb.Append("convert(datetime,'01.01.2015') AS RemainderDate, ");
            sb.Append("ISNULL(RM.Amount, 0) AS Remainders, ");
            sb.Append("'' AS Supplier ");

            sb.Append("FROM [dbo].[PriceItems] AS PR ");
            sb.Append("LEFT JOIN [dbo].[Gears] AS GR ON PR.Gear_Id = GR.Id ");
            sb.Append("LEFT JOIN ( ");
            sb.Append("SELECT PriceItem_Id, ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("Warehouse_Id, ");
            }
            sb.Append(" SUM(Amount) AS Amount ");

            sb.Append("FROM [dbo].[Remainders] ");
            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append("WHERE Warehouse_Id = @p0 ");
                parameters.Add(warehouse);
            }
            sb.Append("GROUP BY PriceItem_Id ");

            if (!string.IsNullOrEmpty(warehouse))
            {
                sb.Append(",Warehouse_Id ");
            }
            sb.Append(") AS RM ON PR.Id = RM.PriceItem_Id ");


            sb.Append("JOIN [PriceListPriceItems] AS PRPRL ON PR.Id = PRPRL.PriceItem_Id ");
            sb.Append("JOIN [PriceLists] AS PRL ON PRPRL.PriceList_Name = PRL.Name ");
            sb.Append("LEFT JOIN  ");
            sb.Append("( ");
            sb.Append("SELECT PriceItem_Id, MAX(PriceDate) AS CreatedDate ");
            sb.Append("FROM [WholesalePrices] ");
            sb.Append("GROUP BY PriceItem_Id ");
            sb.Append(") AS WHPSD ON PR.Id = WHPSD.PriceItem_Id ");
            sb.Append("JOIN [WholesalePrices] AS WHPS ON WHPSD.PriceItem_Id = WHPS.PriceItem_Id AND WHPSD.CreatedDate = WHPS.PriceDate ");

            //sb.Append("WHERE PRPRL.PriceList_Name = @p0 ");




            var query = CurrentDataSource.Set<PriceItemRemainderView>()
          .SqlQuery(sb.ToString(), parameters.ToArray());


            return query;

        }


    }
}
