using System.Windows.Controls;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using System;

    public partial class RealizationPerDay : Page
    {
        public RealizationPerDay()
        {
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ReceiptGridControl_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            //if (e.Column.FieldName == "DebtDischarge")
            //{
            //    if (Convert.ToInt32(e.Value) == 0)
            //    {
            //        e.DisplayText = string.Empty;
            //    }
            //}
            if (e.Column.FieldName == "SoldCount")
            {
                if (Convert.ToInt32(e.Value) == 0)
                {
                    e.DisplayText = string.Empty;
                }               
            }
            else if (e.Column.FieldName == "Remainders")
            {
                if (Convert.ToInt32(e.Value) == 0)
                {
                    e.DisplayText = string.Empty;
                }
            }
        }

    }
}
