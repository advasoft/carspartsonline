
namespace StoreAppTest.Controls
{
    using System.Windows;

    public partial class AcceptIncomeControl
    {
        public AcceptIncomeControl()
        {
            InitializeComponent();
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            OkButton.IsEnabled = false;
            DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
