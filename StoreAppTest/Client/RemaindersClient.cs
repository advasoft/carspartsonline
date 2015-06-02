
namespace StoreAppTest
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;
    using MSPToolkit.Encodings;
    using Newtonsoft.Json;
    using StoreAppDataService;

    public class RemaindersClient
    {
        private string _parameter;
        private Uri _getAllRemaindersUri;
        private Uri _getNewsRemaindersUri;
        private Uri _getSucksRemaindersUri;
        private Uri _getLowLimitRemaindersUri;
        private Uri _getIncomesRemaindersUri;
        private Uri _getTotalIncomesUri;
        private Uri _getAllPriceListItemsUri;

        public RemaindersClient(string parameter)
        {
            _parameter = parameter;

            _getAllRemaindersUri = new Uri(
                        string.Concat(App.AppBaseUrl,
                        string.Format("/api/PriceLists/GetAllPriceItems?priceListName={0}", _parameter))
                        , UriKind.Absolute);

            _getNewsRemaindersUri = new Uri(
                        string.Concat(App.AppBaseUrl,
                        string.Format("/api/PriceLists/GetNewsPriceItems?priceListName={0}", _parameter))
                        , UriKind.Absolute);

            _getSucksRemaindersUri = new Uri(
                        string.Concat(App.AppBaseUrl,
                        string.Format("/api/PriceLists/GetSucksPriceItems?priceListName={0}", _parameter))
                        , UriKind.Absolute);

            _getLowLimitRemaindersUri = new Uri(
                        string.Concat(App.AppBaseUrl,
                        string.Format("/api/PriceLists/GetLowerLimitPriceItems?priceListName={0}", _parameter))
                        , UriKind.Absolute);

            _getIncomesRemaindersUri = new Uri(
                        string.Concat(App.AppBaseUrl,
                        string.Format("/api/PriceLists/GetIncomesPriceItems?priceListName={0}", _parameter))
                        , UriKind.Absolute);

            _getTotalIncomesUri = new Uri(
                        string.Concat(App.AppBaseUrl,
                        string.Format("/api/PriceLists/GetTotalIncomes?priceListName={0}", _parameter))
                        , UriKind.Absolute);

            _getAllPriceListItemsUri = new Uri(
                        string.Concat(App.AppBaseUrl,
                        string.Format("/api/PriceLists/GetAllPriceListItems?warehouse={0}", _parameter))
                        , UriKind.Absolute);

        }



        public IEnumerable<PriceItemRemainderView> GetAllRemainders()
        {
            return GetInternalRemainders<IEnumerable<PriceItemRemainderView>>(_getAllRemaindersUri);
        }

        public IEnumerable<PriceItemRemainderView> GetNewsRemainders()
        {
            return GetInternalRemainders<IEnumerable<PriceItemRemainderView>>(_getNewsRemaindersUri);
        }

        public IEnumerable<PriceItemRemainderView> GetSucksRemainders()
        {
            return GetInternalRemainders<IEnumerable<PriceItemRemainderView>>(_getSucksRemaindersUri);
        }

        public IEnumerable<PriceItemRemainderView> GetLowLimitRemainders()
        {
            return GetInternalRemainders<IEnumerable<PriceItemRemainderView>>(_getLowLimitRemaindersUri);
        }

        public IEnumerable<PriceItemRemainderView> GetIncomesRemainders()
        {
            return GetInternalRemainders<IEnumerable<PriceItemRemainderView>>(_getIncomesRemaindersUri);
        }

        public IEnumerable<PriceIncomeTotalItemView> GetTotalIncomes()
        {
            return GetInternalRemainders<IEnumerable<PriceIncomeTotalItemView>>(_getTotalIncomesUri);
        }

        public IEnumerable<PriceListItemRemainderView> GetAllPriceListItems()
        {
            return GetInternalRemainders<IEnumerable<PriceListItemRemainderView>>(_getAllPriceListItemsUri);
        }

        private T GetInternalRemainders<T>(Uri uri)
        {
            EventWaitHandle handler = new AutoResetEvent(false);

            T result = default(T);

                    new Thread(() =>
        {

            var request =

             WebRequest.Create(uri);
            request.Method = "GET";
            request.BeginGetResponse((r) =>
            {
                HttpWebResponse response;
                response =

                  (HttpWebResponse)request.EndGetResponse(r);

                if (response.StatusCode == HttpStatusCode.OK)
                {

                    Stream stream = response.GetResponseStream();
                    StreamReader sr = new StreamReader(stream);

                    var json = sr.ReadToEnd();
                    result = JsonConvert.DeserializeObject<T>(json);
                    handler.Set();
                }
                else
                {
                    handler.Set();
                }

            },

            request);

        }) { IsBackground = true }.Start();

                    handler.WaitOne();
                    return result;

        }
    }
}
