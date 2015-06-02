using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace StoreAppTest.Views
{
    using DevExpress.Xpf.Editors;
    using DevExpress.XtraEditors.DXErrorProvider;

    public partial class IncomPage : Page
    {
        public IncomPage()
        {
            InitializeComponent();
        }

        // Выполняется, когда пользователь переходит на эту страницу.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void SuppliersLookUpEdit_OnValidate(object sender, ValidationEventArgs e)
        {
            if (e.Value == null)
            {
                e.IsValid = false;
                e.ErrorType = ErrorType.Default;
                e.ErrorContent = "Нужно выбрать поставщика.";
            }
            else
            {
                e.IsValid = true;
                e.ErrorType = ErrorType.None;
                e.ErrorContent = "";
            }
        }

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
