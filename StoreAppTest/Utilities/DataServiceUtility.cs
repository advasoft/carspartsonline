
namespace StoreAppTest.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Data.Services.Client;
    using System.Linq;
    using System.Threading;

    public static class DataServiceUtility
    {
        public static IEnumerable<TE> ExecuteSyncronous<TE>(this DataServiceQuery<TE> query)
        {
            ManualResetEvent m1 = new ManualResetEvent(false);
            IEnumerable<TE> result = default(IEnumerable<TE>);

            query.BeginExecute(r =>
            {
                try { result = query.EndExecute(r).AsEnumerable(); }
                finally { m1.Set(); }
            }, null);

            //ThreadPool.QueueUserWorkItem((o) =>
            //{

            //    // do your thing..
            //});
            WaitHandle.WaitAll(new WaitHandle[] { m1 });

            return result;
        }


        public static DataServiceResponse SaveChangesSynchronous(this DataServiceContext context)
        {

            DataServiceResponse result = default(DataServiceResponse);

            var th = new Thread(() =>
{

    ManualResetEvent m1 = new ManualResetEvent(false);

            context.BeginSaveChanges(r =>
            {
                try { result = context.EndSaveChanges(r); }
                finally { m1.Set(); }
            }, null);

            WaitHandle.WaitAll(new WaitHandle[] { m1 });
        }) { IsBackground = true };
            th.Start();
            th.Join();

            return result;
        }


        public static IEnumerable<TE> ExecuteSyncronous<TE>(this DataServiceContext context, IQueryable<TE> query)
        {
            IEnumerable<TE> result = default(IEnumerable<TE>);
            var th = new Thread(() =>
            {
                ManualResetEvent m1 = new ManualResetEvent(false);

                context.IgnoreResourceNotFoundException = true;
                context.BeginExecute<TE>(new Uri(query.ToString()), r =>
                {
                    try { result = context.EndExecute<TE>(r); }
                    finally { m1.Set(); }
                }, null);
                WaitHandle.WaitAll(new WaitHandle[] { m1 });
            }) { IsBackground = true };
            th.Start();
            th.Join();

            return result;
        }

        public static IEnumerable<TE> ExecuteSyncronous<TE>(this DataServiceContext context, Uri uri)
        {
            IEnumerable<TE> result = default(IEnumerable<TE>);
            var th = new Thread(() =>
            {
                ManualResetEvent m1 = new ManualResetEvent(false);

                context.IgnoreResourceNotFoundException = true;
                context.BeginExecute<TE>(uri, r =>
                {
                    try { result = context.EndExecute<TE>(r); }
                    finally { m1.Set(); }
                }, null);
                WaitHandle.WaitAll(new WaitHandle[] { m1 });
            }) { IsBackground = true };
            th.Start();
            th.Join();

            return result;
        }
    }
}
