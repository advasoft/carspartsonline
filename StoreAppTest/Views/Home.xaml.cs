namespace StoreAppTest.Views
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Controls.DataVisualization.Charting;
    using System.Windows.Navigation;
    using Model;

    public partial class Home : Page
	{
		public Home()
		{
			InitializeComponent();
            Loaded += Home_Loaded;
		}

        void Home_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var data = new List<SalesDayChartItem>()
		    {
		        new SalesDayChartItem(){Day = "пн.", Sales = 0},
                new SalesDayChartItem(){Day = "вт.", Sales = 0},
                new SalesDayChartItem(){Day = "ср.", Sales = 10},
                new SalesDayChartItem(){Day = "чт.", Sales = 20},
                new SalesDayChartItem(){Day = "пт.", Sales = 30},
                new SalesDayChartItem(){Day = "сб.", Sales = 10},
                new SalesDayChartItem(){Day = "вс.", Sales = 0}
		    };


            SalesChartName.DataSource = data;
        }

		// Executes when the user navigates to this page.
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
		}
	}
}