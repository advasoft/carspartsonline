
using System.Windows;

namespace StoreAppTest.Controls
{
    public partial class InputDebtDischarge
    {
        public InputDebtDischarge()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
