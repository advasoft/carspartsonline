using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using System;
    using DevExpress.Data;
    using DevExpress.Xpf.Grid;

    public partial class RealizationPerDay : Page
    {
        public RealizationPerDay()
        {
            InitializeComponent();

            ReceiptGridControl.GroupSummary.Add(new GridSummaryItem()
            {
                FieldName = "SoldCount",
                SummaryType = SummaryItemType.Sum,
                ShowInColumn = "SoldCount",
                DisplayFormat = "Количество: {0}"
            });
            ReceiptGridControl.GroupSummary.Add(new GridSummaryItem()
            {
                FieldName = "AmountWithoutDebt",
                SummaryType = SummaryItemType.Sum,
                ShowInColumn = "Amount",
                DisplayFormat = "Итого: {0}"
            });
            ReceiptGridControl.GroupSummary.Add(new GridSummaryItem()
            {
                FieldName = "AmountWithoutDebtProfit",
                SummaryType = SummaryItemType.Sum,
                ShowInColumn = "Amount",
                DisplayFormat = "Доп прибыль: {0}"
            });
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ReceiptGridControl_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            //if (e.Column.FieldName == "DebtDischarge")
            //{
            //    if (Convert.ToInt32(e.Value) == 0)
            //    {
            //        e.DisplayText = string.Empty;
            //    }
            //}
            if (e.Column.FieldName == "SoldCount")
            {
                if (Convert.ToInt32(e.Value) == 0)
                {
                    e.DisplayText = string.Empty;
                }               
            }
            else if (e.Column.FieldName == "Remainders")
            {
                if (Convert.ToInt32(e.Value) == 0)
                {
                    e.DisplayText = string.Empty;
                }
            }
        }

        private void ReceiptGridControl_OnCustomSummaryExists(object sender, CustomSummaryExistEventArgs e)
        {
            e.Exists = e.GroupLevel == 0;


        }
    }
}
