
namespace StoreAppTest.Utilities
{
    using System;

    public static class DateTimeHelper
    {
        public static DateTime GetKzDateTime(DateTime time)
        {
            
            return time.AddHours(6);
        }

        public static DateTime GetNowKz()
        {
            return GetKzDateTime(DateTime.UtcNow);
        }
    }
}
