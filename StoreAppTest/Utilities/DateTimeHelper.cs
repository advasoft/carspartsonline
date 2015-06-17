
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

        public static DateTime GetStartDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
        }
        public static DateTime GetEndDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }
        public static DateTime GetPreviousStartDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0).AddDays(-1);
        }
        public static DateTime GetPreviousEndDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59).AddDays(-1);
        }

        public static DateTime GetStartWeek(DateTime date)
        {
            var monday = DayOfWeek.Monday;
            var sunday = DayOfWeek.Sunday;
            var today = date.DayOfWeek;
            var diff = today - monday;
            if (diff < 0)
            {
                diff += 7;
            }

            return GetStartDay(date).AddDays(-1 * diff);
        }
        public static DateTime GetEndWeek(DateTime date)
        {
            return GetEndDay(GetStartWeek(date).AddDays(6));
        }
        public static DateTime GetPreviousStartWeek(DateTime date)
        {
            return GetStartWeek(date).AddDays(-7);
        }
        public static DateTime GetPreviousEndWeek(DateTime date)
        {
            return GetEndDay(GetPreviousStartWeek(date).AddDays(6));
        }

        public static DateTime GetStartMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        }
        public static DateTime GetEndMonth(DateTime date)
        {
            return GetEndDay(GetStartMonth(date).AddMonths(1).AddDays(-1));
        }
        public static DateTime GetPreviousStartMonth(DateTime date)
        {
            return GetStartMonth(date).AddMonths(-1);
        }
        public static DateTime GetPreviousEndMonth(DateTime date)
        {
            return GetEndDay(GetPreviousStartMonth(date).AddMonths(1).AddDays(-1));
        }

        public static DateTime GetStartHalfYear(DateTime date)
        {
            if (date.Month > 6)
            {
                return new DateTime(date.Year, 6, 1, 0, 0, 0);
            }
            else
            {
                return new DateTime(date.Year, 1, 1, 0, 0, 0);
            }
        }
        public static DateTime GetEndHalfYear(DateTime date)
        {
            if (date.Month > 6)
            {
                return new DateTime(date.Year, 12, new DateTime(date.Year + 1, 1, 1, 0,0,0).AddDays(-1).Day, 23, 59, 59);
            }
            else
            {
                return new DateTime(date.Year, 6, new DateTime(date.Year, 7, 1, 0, 0, 0).AddDays(-1).Day, 23, 59, 59);
            }
        }
        public static DateTime GetPreviousStartHalfYear(DateTime date)
        {
            var currentHalfYear = GetStartHalfYear(date);
            return currentHalfYear.AddMonths(-6);
        }
        public static DateTime GetPreviousEndHalfYear(DateTime date)
        {
            var currentHalfYear = GetEndHalfYear(date);
            return currentHalfYear.AddMonths(-6);
        }

        public static DateTime GetStartYear(DateTime date)
        {
            return new DateTime(date.Year, 1, 1, 0, 0, 0);
        }
        public static DateTime GetEndYear(DateTime date)
        {
            return new DateTime(date.Year, 12, new DateTime(date.Year + 1, 1, 1, 0, 0, 0).AddDays(-1).Day, 23, 59, 59);
        }
        public static DateTime GetPreviousStartYear(DateTime date)
        {
            return GetStartYear(date).AddYears(-1);
        }
        public static DateTime GetPreviousEndYear(DateTime date)
        {
            return GetEndYear(date).AddYears(-1);
        }

        
    }
}
