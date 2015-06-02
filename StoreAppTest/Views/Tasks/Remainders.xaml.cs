
namespace StoreAppTest.Views
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Navigation;
    using DevExpress.Utils;
    using DevExpress.Xpf.Grid;
    using DevExpress.Xpf.Printing;
    using ViewModels;

    public partial class Remainders : Page
    {
        private RemaindersViewModel _viewModel;

        public Remainders()
        {
            InitializeComponent();
            Loaded += Remainders_Loaded;
        }

        void Remainders_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel = DataContext as RemaindersViewModel;
            //_viewModel.LoadView();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DocumentPreviewWindow preview = new DocumentPreviewWindow();
            PrintableControlLink link = new PrintableControlLink((IPrintableControl)PricesGridControl.View);
            link.ExportServiceUri = "/StoreExportService.svc";
            link.PrintingSystem.ExportOptions.PrintPreview.ShowOptionsBeforeExport = false;
            LinkPreviewModel model = new LinkPreviewModel(link);
            preview.Model = model;
            link.CreateDocument(false);
            preview.ShowDialog();
        }


        //private void RemaindersGrid_OnOnEnterKeyPresses(object sender, EventArgs e)
        //{
        //    _viewModel.EditRemaindersCommand.Execute(null);
        //}

        private void PricesGridControl_AutoGeneratingColumn(object sender, DevExpress.Xpf.Grid.AutoGeneratingColumnEventArgs e)
        {
            if (e.Column.FieldName == "Number")
            {
                e.Column.Header = "№";
                e.Column.Width = 40;
                e.Column.VisibleIndex = 1;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.FieldName == "Articul")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "Selected")
            {
                e.Column.Header = " ";
                e.Column.Width = 20;
                e.Column.VisibleIndex = 0;
                e.Column.AllowEditing = DefaultBoolean.True;
            }
            else if (e.Column.FieldName == "CatalogNumber")
            {
                e.Column.Header = "Каталожный номер";
                e.Column.Width = 100;
                e.Column.VisibleIndex = 2;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.FieldName == "Name")
            {
                e.Column.Header = "Название";
                e.Column.Width = 200;
                e.Column.VisibleIndex = 3;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.FieldName == "IsDuplicate")
            {
                e.Column.Header = "*";
                e.Column.Width = 20;
                e.Column.VisibleIndex = 4;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.FieldName == "Uom")
            {
                e.Column.Header = "Единица изм.";
                e.Column.Width = 60;
                e.Column.VisibleIndex = 5;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.FieldName == "WholesalePrice")
            {
                e.Column.Header = "Цена оптовая";
                e.Column.Width = 80;
                e.Column.VisibleIndex = 6;
                e.Column.ReadOnly = true;
            }
            else if (e.Column.FieldName == "BuyPriceRur")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "BuyPriceTng")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "Gear_Id")
                e.Column.Visible = false;
            else if (e.Column.FieldName == "PriceItem_Id")
                e.Column.Visible = false;

            else if (e.Column.FieldName == "Supplier")
            {

                if (_viewModel != null && App.CurrentUser.IsSupplierVisible && (_viewModel.IncomesChecked || _viewModel.LowerLimitChecked))
                {
                    e.Column.Header = "Поставщик";
                    e.Column.Width = 150;
                    e.Column.VisibleIndex = 6;
                    e.Column.ReadOnly = true;
                }
                else
                {
                    e.Column.Visible = false;
                }
            }
            else
            {
                e.Column.Width = 80;
                e.Column.ReadOnly = true;
            }

        }

        private void PricesGridControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                e.Handled = true;
            }
        }

        private void TableView_RowDoubleClick(object sender, DevExpress.Xpf.Grid.RowDoubleClickEventArgs e)
        {
            _viewModel.EditRemaindersCommand.Execute(null);
            e.Handled = true;
        }

        private void TableView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                e.Handled = true;
            }
        }

        private void PricesGridControl_CustomColumnSort(object sender, DevExpress.Xpf.Grid.CustomColumnSortEventArgs e)
        {
            if (e.Column.Header.ToString() == "№")
            {
                GridControl grid = sender as GridControl;
                int r1 = e.ListSourceRowIndex1;
                int r2 = e.ListSourceRowIndex2;
                int dates1 = (int)grid.GetCellValue(r1, grid.Columns[0]);
                int dates2 = (int)grid.GetCellValue(r2, grid.Columns[0]);
                e.Result = Comparer<int>.Default.Compare(dates1, dates2);
                e.Handled = true;
            }
        }
    }
}
