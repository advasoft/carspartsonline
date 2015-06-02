

namespace StoreAppTest.Controls
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class PriceChangesReportControl 
    {
        public PriceChangesReportControl()
        {
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

        }

        private void TableView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DialogResult = true;
            }
        }

        private void TableView_RowDoubleClick(object sender, DevExpress.Xpf.Grid.RowDoubleClickEventArgs e)
        {
            DialogResult = true;
        }
    }
}
