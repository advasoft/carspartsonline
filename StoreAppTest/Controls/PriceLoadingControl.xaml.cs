
namespace StoreAppTest.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using GalaSoft.MvvmLight.Threading;
    using Newtonsoft.Json;

    public partial class PriceLoadingControl
    {
        private bool _isBusy = false;
        private Dictionary<string, bool> _uploadedFiles = new Dictionary<string, bool>();
 
        public PriceLoadingControl()
        {
            InitializeComponent();
            UploadControl.WebHandlerUri = new Uri(string.Concat(
                Application.Current.Host.Source.Scheme, "://",
                Application.Current.Host.Source.Host, ":",
                Application.Current.Host.Source.Port,
                "/api/PriceLists/LoadPriceFile"), UriKind.Absolute);
            Closing += PriceLoadingControl_Closing;

        }

        void PriceLoadingControl_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_isBusy)
            {
                e.Cancel = true;
            }
        }

        private void UploadControl_OnTotalUploadCompleted(object sender, EventArgs e)
        {
            AcceptButton.IsEnabled = true;
        }

        private void UploadControl_OnUploadCancelled(object sender, EventArgs e)
        {
            AcceptButton.IsEnabled = false;
            ProgressBarEdit.Visibility = Visibility.Collapsed;
        }

        private void UploadControl_FileUploadCompleted(object sender, DevExpress.Xpf.Controls.UploadItemEventArgs e)
        {
            _uploadedFiles.Add(e.ItemInfo.Name, false);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UploadControl.StopUpload();
            UploadControl.IsEnabled = false;
            AcceptButton.IsEnabled = false;
            ProgressBarEdit.Visibility = Visibility.Visible;
            _isBusy = true;

            var unuploadedFiles = _uploadedFiles.Where(w => !w.Value).ToList();
            Thread[] tasks = new Thread[unuploadedFiles.Count];
            int index = 0;

            foreach (var uploadedFile in unuploadedFiles)
            {
                tasks[index] = new Thread(() =>
                {
                    LoadPrice(uploadedFile.Key, (r) =>
                    {
                        if (r.Status == HttpStatusCode.InternalServerError)
                        {
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                DialogResult = true;
                                MessageChildWindow messageChildWindow = new MessageChildWindow();
                                messageChildWindow.Message = string.Format("Не удалось загрузить прайс лист\r\n {0}", r.ErrorMessage);
                                messageChildWindow.Title = "Ошибка загрузки прайс листа";
                                messageChildWindow.Show();
                            });                            
                        }


                        _uploadedFiles[r.PriceNameList] = true;
                            _isBusy = false;
                            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                DialogResult = true;
                            });
                    });
                })
                {
                    IsBackground = true
                };
                tasks[index].Start();
                index++;
            }
        }


        private void LoadPrice(string priceFileName, Action<RequestCallbackParameter> callback)
        {
            try
            {

                Uri apiUri = default(Uri);
                apiUri = new Uri(
                    string.Concat(App.AppBaseUrl,
                    string.Format("/api/PriceLists/LoadPriceListFile?priceListFileName={0}", priceFileName))
                    , UriKind.Absolute);



                var request =

                  WebRequest.Create(apiUri);
                request.Method = "POST";
                request.Headers[HttpRequestHeader.Authorization] = string.Format("user={0}", App.CurrentUser.UserName);
                request.BeginGetResponse((r) =>
                {

                    WebRequest req =
                    (WebRequest)r.AsyncState;
                    try
                    {

                        HttpWebResponse response =

                            (HttpWebResponse) request.EndGetResponse(r);
                        callback(new RequestCallbackParameter() { Status = response.StatusCode, PriceNameList = priceFileName });

                    }
                    catch (WebException ex)
                    {
                        callback(new RequestCallbackParameter() { Status = HttpStatusCode.InternalServerError, PriceNameList = priceFileName, ErrorMessage = "Ошибка в структуре файла прайс-листа"});
                    }
                },

                request); 

            }
            catch (Exception)
            {

                throw;
            }            
        }
    }

    public class RequestCallbackParameter
    {
        public string PriceNameList { get; set; }
        public HttpStatusCode Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
