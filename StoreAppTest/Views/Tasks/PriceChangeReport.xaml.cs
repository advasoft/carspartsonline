
using System.Windows.Controls;

namespace StoreAppTest.Views
{
    using DevExpress.Xpf.Grid;

    public partial class PriceChangeReport : UserControl
    {
        public PriceChangeReport()
        {
            InitializeComponent();
        }

        private void PricesGridControl_OnAutoGeneratingColumn(object sender, AutoGeneratingColumnEventArgs e)
        {
            if (e.Column.FieldName == "Number")
                e.Column.Width = 40;
            else if (e.Column.FieldName == "Articul")
            {
                e.Column.Visible = false;
            }
            else if (e.Column.FieldName == "CatalogNumber")
                e.Column.Width = 80;
            else if (e.Column.FieldName == "Gear_Name")
                e.Column.Width = 200;
            else if (e.Column.FieldName == "IsDuplicate")
                e.Column.Width = 20;
            else if (e.Column.FieldName == "Uom")
                e.Column.Width = 60;
            else if (e.Column.FieldName == "WholesalePrice")
                e.Column.Width = 80;
            else if (e.Column.FieldName == "NewPrice")
            {
                e.Column.Width = 80;
                e.Column.DisplayTemplate = (ControlTemplate) Resources["GridColumnPriceTemplate"];
            }
            else if (e.Column.FieldName == "PreviewsPrice")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "PriceItem_Id")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "BuyPriceRur")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "BuyPriceTng")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "Gear_Id")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "RecommendedRemainder")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "LowerLimitRemainder")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "Incomes")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "Income_Id")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "IsAccept")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "Remainders")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "NewPrice_Id")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "PreviewsPrice_Id")
                e.Column.Visible = false;

        }

        private void TableView_OnRowDoubleClick(object sender, RowDoubleClickEventArgs e)
        {
            
        }
    }
}
