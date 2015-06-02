using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using DevExpress.Data.Utils.ServiceModel;
using DevExpress.Xpf.Printing.Service;
using DevExpress.XtraReports.Service;
using DevExpress.XtraReports.UI;

namespace StoreAppTest.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "StoreAppReportService" in code, svc and config file together.
    [SilverlightFaultBehavior]
    public class StoreAppReportService : DevExpress.XtraReports.Service.ReportService
    {

    }
}
