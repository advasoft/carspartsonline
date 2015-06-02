

namespace StoreAppTest.Controls
{
    public partial class PriceItemEditControl
    {
        public PriceItemEditControl()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
