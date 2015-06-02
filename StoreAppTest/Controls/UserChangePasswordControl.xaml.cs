

namespace StoreAppTest.Controls
{
    public partial class UserChangePasswordControl 
    {
        public UserChangePasswordControl()
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
