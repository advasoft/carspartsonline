
using System.Windows;
using System.Windows.Controls;

namespace StoreAppTest.Controls
{
    public partial class AskChildWindow : ChildWindow
    {
        public AskChildWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        public string Message
        {
            get { return MessageTextBlock.Text; }
            set { MessageTextBlock.Text = value; }
        }
    }
}

