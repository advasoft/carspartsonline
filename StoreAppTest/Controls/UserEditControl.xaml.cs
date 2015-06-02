

namespace StoreAppTest.Controls
{
    public partial class UserEditControl 
    {
        public UserEditControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
