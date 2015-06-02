using System.Windows;
using System.Windows.Controls;

namespace StoreAppTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Windows.Browser;
    using DevExpress.Data.PLinq.Helpers;
    using GalaSoft.MvvmLight.Threading;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.ServiceLocation;
    using Microsoft.Practices.Unity;
    using StoreAppDataService;
    using Utilities;
    using Views;
    using Receipt = Model.Receipt;
    using Refund = Model.Refund;

    public partial class App : Application
	{
        public static User CurrentUser { get; set; }
        public static string AppBaseUrl { get; set; }


		public App()
		{
			this.Startup += this.Application_Startup;
			this.UnhandledException += this.Application_UnhandledException;
            IUnityContainer container = new UnityContainer();
		    container.RegisterType(typeof (IEventAggregator), typeof (EventAggregator), new ContainerControlledLifetimeManager());

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            DispatcherHelper.Initialize();

            ////Register BrowserHttp for these prefixes 
            //WebRequest.RegisterPrefix("http://",
            //           System.Net.Browser.WebRequestCreator.BrowserHttp);
            WebRequest.RegisterPrefix("https://",
                       System.Net.Browser.WebRequestCreator.BrowserHttp);
            WebRequest.RegisterPrefix("http://", System.Net.Browser.WebRequestCreator.ClientHttp);
			InitializeComponent();
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
            //Init data context
		    AppBaseUrl = string.Concat(
                Current.Host.Source.Scheme, "://",
		        Current.Host.Source.Host, ":",
		        Current.Host.Source.Port);


            //TODO: изменить на авторизацию
            StoreDbContext ctx = new StoreDbContext(
                new Uri(string.Concat(
                    Current.Host.Source.Scheme, "://", Current.Host.Source.Host, ":", Current.Host.Source.Port,
                    "/StoreAppDataService.svc/"), UriKind.Absolute));
            ctx.IgnoreResourceNotFoundException = true;

            new Thread(() =>
            {
                var categories = ctx.ExecuteSyncronous(ctx.GearCategories).ToList();
                //var user = ctx.ExecuteSyncronous(ctx.Users.Where(u => u.UserName == "admin")).FirstOrDefault();
            }).Start();

            //ctx.BeginExecute<User>(new Uri(ctx.Users.Where(u => u.UserName == "dd").ToString()),a =>
            //{
            //    //Current
		        
            //    //CurrentUser = usr.FirstOrDefault(); 
            //}, new object());

            Grid gr = new Grid();
            gr.Children.Add(new LoginPage());
			this.RootVisual = gr;
		}

		private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
		{
			// If the app is running outside of the debugger then report the exception using
			// a ChildWindow control.
			if (!System.Diagnostics.Debugger.IsAttached)
			{
				// NOTE: This will allow the application to continue running after an exception has been thrown
				// but not handled. 
				// For production applications this error handling should be replaced with something that will 
				// report the error to the website and stop the application.
				e.Handled = true;
				ChildWindow errorWin = new ErrorWindow(e.ExceptionObject);
				errorWin.Show();
			}
		}
	}
}