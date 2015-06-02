
using System.Windows;
using System.Windows.Controls;

namespace StoreAppTest
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using Controls;
    using GalaSoft.MvvmLight.Threading;
    using MSPToolkit.Encodings;
    using Newtonsoft.Json;
    using StoreAppDataService;
    using Utilities;

    public partial class LoginPage : UserControl
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ////TODO: это топор, нужно переделать на нормальную авторизацию
            //if (LoginTextEdit.EditValue != "admin" && PasswordBoxEdit.Password != "admin")
            //{
                
            //    ErrorMessageTextBlock.Visibility = Visibility.Visible;
                
            //}
            //else
            //{
            //    ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
                var grid = Application.Current.RootVisual as Grid;


                ////TODO: изменить на авторизацию
                //StoreDbContext ctx = new StoreDbContext(
                //    new Uri(string.Concat(
                //        Application.Current.Host.Source.Scheme, "://",
                //        Application.Current.Host.Source.Host, ":",
                //        Application.Current.Host.Source.Port,
                //        "/StoreAppDataService.svc/"), UriKind.Absolute));
                //ctx.IgnoreResourceNotFoundException = true;
                Uri apiUri = new Uri(
                    string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/LogIn?userName={0}&passwordHash={1}", LoginTextEdit.Text, PasswordBoxEdit.Password))
                    , UriKind.Absolute);

                LoginButton.IsEnabled = false;
                new Thread(() =>
                {
                    try
                    {


                        var request =

                          WebRequest.Create(apiUri);
                        request.Method = "POST";
                        //request.Headers[HttpRequestHeader.Authorization] = string.Format("user={0}", App.CurrentUser.UserName);

                        request.BeginGetResponse((r) =>
                        {
                            //WebRequest req =

                            //     (WebRequest)r.AsyncState;
                            HttpWebResponse response;
                            try
                            {
                                response =

                                  (HttpWebResponse)request.EndGetResponse(r);
                            }
                            catch (WebException)
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    ErrorMessageTextBlock.Visibility = Visibility.Visible;
                                    LoginButton.IsEnabled = true;
                                });
                                return;
                                
                            }

                            if (response.StatusCode != HttpStatusCode.Accepted)
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    ErrorMessageTextBlock.Visibility = Visibility.Visible;
                                    LoginButton.IsEnabled = true;
                                });
                            }
                            else
                            {
                                var sb = new StringBuilder();
                                using (Stream resStream = response.GetResponseStream())
                                {

                                    string parseString = null;
                                    int count = 0;
                                    byte[] buf = new byte[4096];
                                    var enc = new Windows1251Encoding();
                                    
                                    do
                                    {
                                        // Read a chunk of data
                                        count = resStream.Read(buf, 0, buf.Length);

                                        if (count != 0)
                                        {
                                            // Convert to ASCII

                                            parseString = Encoding.UTF8.GetString(buf, 0, count);

                                            // Append string to results
                                            sb.Append(parseString);
                                        }
                                    }
                                    while (count > 0);

                                    
                                }

                                App.CurrentUser = JsonConvert.DeserializeObject<User>(sb.ToString());
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
                                    LoginButton.IsEnabled = true;
                                    grid.Children.Clear();
                                    grid.Children.Add(new MainPage());
                                });
                            }

                            //callback(new RequestCallbackParameter() { Status = response.StatusCode, PriceNameList = priceFileName });
                        },

                        request); 

                        //App.CurrentUser = ctx.ExecuteSyncronous(ctx.Users.Expand("Warehouse").Where(u => u.UserName == "admin")).FirstOrDefault();
                    }
                    catch (Exception exception)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            LoginButton.IsEnabled = true;
                            ErrorMessageTextBlock.Visibility = Visibility.Visible;
                            //MessageBox.Show(exception.Message);
                        });
                    }

                    
                }).Start();

            //}
        }
    }
}
