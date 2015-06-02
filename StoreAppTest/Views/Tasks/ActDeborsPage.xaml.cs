namespace StoreAppTest.Views
{
    using System;
    using System.Collections;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using DevExpress.Data;

    public partial class ActDeborsPage : Page
    {

        public ActDeborsPage()
        {
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void ReceiptGridControl_OnCustomSummary(object sender, CustomSummaryEventArgs e)
        {
        }
    }
}
