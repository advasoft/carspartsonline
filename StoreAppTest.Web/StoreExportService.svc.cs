using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using DevExpress.Data.Utils.ServiceModel;

namespace StoreAppTest.Web
{
    [SilverlightFaultBehavior]
    public class StoreExportService : DevExpress.Xpf.Printing.Service.ExportService
    {
    }
}
