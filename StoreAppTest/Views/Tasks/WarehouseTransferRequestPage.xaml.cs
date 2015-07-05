namespace StoreAppTest.Views
{
    using System;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using DevExpress.Data;
    using DevExpress.Xpf.Data;
    using DevExpress.Xpf.Grid;
    using DevExpress.XtraEditors.DXErrorProvider;
    using ViewModels;

    public partial class WarehouseTransferRequestPage : Page
    {
        private WarehouseTransferRequestViewModel _viewModel;

        public WarehouseTransferRequestPage()
        {
            InitializeComponent();
            Loaded += WarehouseTransferRequestPage_Loaded;
        }

        void WarehouseTransferRequestPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel = DataContext as WarehouseTransferRequestViewModel;
            BarcodeEditText.Focus();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void WarehouseLookUpEdit_Validate(object sender, DevExpress.Xpf.Editors.ValidationEventArgs e)
        {
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = ErrorType.Default;
                e.ErrorContent = "Нужно выбрать склад.";
            }
            else
            {
                e.IsValid = true;
                e.ErrorType = ErrorType.None;
                e.ErrorContent = "";
            }
        }

        private void GridColumn_Validate(object sender, DevExpress.Xpf.Grid.GridCellValidationEventArgs e)
        {
            //if (_viewModel.SupplierRemainders.Count > 0 && !_viewModel.IsNew)//&& _viewModel.IsNew == false)
            //{
            //    var row = e.Row as WarehouseTransferRequestModelItem;
            //    var rem = _viewModel.SupplierRemainders[row.PriceItem_Id];

            //    if (e.Value != null)
            //    {
            //        if (rem == null || (int.Parse(e.Value.ToString()) > rem.Amount))
            //        {
            //            e.IsValid = false;
            //            e.ErrorType = ErrorType.Critical;
            //            e.ErrorContent = string.Format("У {0} в наличии всего {1}", _viewModel.Warehouse_Id, rem == null ? 0 : rem.Amount);
            //        }
            //        else
            //        {
            //            e.IsValid = true;
            //        }
            //    }

            //}
        }

        private void AcceptButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //IDataProviderOwner provider = (IDataProviderOwner) PricesGridControl;


            //for (int i = 0; i < _viewModel.WarehouseTransferRequestItems.Count; i++)
            //{
                //provider.RaiseValidatingCurrentRow(new ValidateControllerRowEventArgs(PricesGridControl.GetRowHandleByListIndex(0), _viewModel.WarehouseTransferRequestItems[0]));
            //}
        }

        private void TableView_ValidateRow(object sender, DevExpress.Xpf.Grid.GridRowValidationEventArgs e)
        {
        //    if (_viewModel.SupplierRemainders.Count > 0) //&& _viewModel.IsNew == false)
        //    {
        //        var row = e.Row as WarehouseTransferRequestModelItem;
        //        var rem = _viewModel.SupplierRemainders[row.PriceItem_Id];

        //        GridColumn countCol = PricesGridControl.Columns["Count"];
        //        var countValue = PricesGridControl.GetCellValue(e.RowHandle, countCol);

        //        if (countValue != null)
        //        {
        //            int count = (int) countValue;
        //            if (count < rem.Amount)
        //            {
                        
        //                //Set errors with specific descriptions for the columns
        //                PricesGridControl.Columns[""](inStockCol, "The value must be greater than Units On Order");
        //            }
        //        }
        //    }
        }

    }
}
