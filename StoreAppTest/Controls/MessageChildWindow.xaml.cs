
using System.Windows;
using System.Windows.Controls;

namespace StoreAppTest.Controls
{
    public partial class MessageChildWindow : ChildWindow
    {
        public MessageChildWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string Message
        {
            get { return MessageTextBlock.Text; }
            set { MessageTextBlock.Text = value; }
        }
    }
}

