
namespace StoreAppTest
{
    using StoreAppDataService;
    using Views;
    using System;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using Controls;
    using GalaSoft.MvvmLight.Threading;
    using Utilities;

    public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();
            Loaded += MainPage_Loaded;
		}

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoginTextBlock.Text = App.CurrentUser.DisplayName;
            if (App.CurrentUser.UserName != "admin")
            {
                AdminLink.Visibility = Visibility.Collapsed;
            }
            else
            {
                AdminLink.Visibility = Visibility.Visible;
            }
        }

		// After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
		private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
		{
			foreach (UIElement child in LinksStackPanel.Children)
			{
				HyperlinkButton hb = child as HyperlinkButton;
				if (hb != null && hb.NavigateUri != null)
				{
					if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
					{
						VisualStateManager.GoToState(hb, "ActiveLink", true);
					}
					else
					{
						VisualStateManager.GoToState(hb, "InactiveLink", true);
					}
				}
			}
		}

		// If an error occurs during navigation, show an error window
		private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			e.Handled = true;
			ChildWindow errorWin = new ErrorWindow(e.Uri);
			errorWin.Show();
		}

        private void LoadPriceLink_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new PriceLoadingControl();
            dialog.Show();


        }

        private void ExitLink_Click(object sender, RoutedEventArgs e)
        {
            var grid = Application.Current.RootVisual as Grid;

            new Thread(() =>
            {
                try
                {
                    App.CurrentUser = null;
                }
                catch (Exception exception)
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        MessageBox.Show(exception.Message);
                    });
                }

                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    grid.Children.Clear();
                    grid.Children.Add(new LoginPage());
                });
            }).Start();
        }
	}
}