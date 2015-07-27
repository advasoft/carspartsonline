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
    using System.Text.RegularExpressions;
    using DevExpress.Xpf.Editors;
    using DevExpress.Xpf.Printing;
    using DevExpress.XtraEditors.DXErrorProvider;

    public partial class IncomesPage : Page
    {
        public IncomesPage()
        {
            InitializeComponent();
        }

        private Regex _numberRegex = new Regex("Оприходование № (?<number>\\d+) от (?<date>[\\d\\.])");

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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentPreviewWindow preview = new DocumentPreviewWindow();
            PrintableControlLink link = new PrintableControlLink((IPrintableControl)IncomesGridControl.View);
            link.ExportServiceUri = "/StoreExportService.svc";
            link.PrintingSystem.ExportOptions.PrintPreview.ShowOptionsBeforeExport = false;
            LinkPreviewModel model = new LinkPreviewModel(link);
            preview.Model = model;
            link.CreateDocument(false);
            preview.ShowDialog();
        }

        private void IncomesGridControl_CustomColumnSort(object sender, DevExpress.Xpf.Grid.CustomColumnSortEventArgs e)
        {
            if (e.Column.FieldName == "Income")
            {
                //Оприходование № 34 от 23.07.2015
                var val1 = GetNumberValue((string) e.Value1);
                var val2 = GetNumberValue((string) e.Value2);

                e.Result = Comparer<long>.Default.Compare(val1, val2);

                e.Handled = true;
            }
        }

        private long GetNumberValue(string incomeString)
        {
            long result = 0;
            var valMatch = _numberRegex.Match(incomeString);
            if (valMatch != null && valMatch.Success)
            {
                result = Convert.ToInt64(valMatch.Groups["number"].Value);
            }
            return result;
        }
    }
}
