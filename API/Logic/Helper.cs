namespace OnlineStore
{
    public static class Helper
    {
        public static string DateTimeToChar14(DateTime date)
        {
            return date.ToString("yyyyMMddHHmmss");
        }

        public static DateTime Char14ToDateTime(string date, DateTime defaultValue)
        {
            if (string.IsNullOrEmpty(date))
            {
                return defaultValue;
            }
            _ = int.TryParse(date.AsSpan(0, 4), out int year);
            _ = int.TryParse(date.AsSpan(4, 2), out int month);
            _ = int.TryParse(date.AsSpan(6, 2), out int day);
            if (date.Length > 8)
            {
                _ = int.TryParse(date.AsSpan(8, 2), out int hour);
                _ = int.TryParse(date.AsSpan(10, 2), out int minute);
                _ = int.TryParse(date.AsSpan(12, 2), out int second);
                if (year > 0 && month > 0 && day > 0 && minute > 0 && second > 0)
                {
                    return new DateTime(year, month, day, hour, minute, second);
                }
                else
                {
                    return defaultValue;
                }
            }
            else
            {
                if (year > 0 && month > 0 && day > 0)
                {
                    return new DateTime(year, month, day);
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        internal static string ToStandardDate(string birthday)
        {
            var date = Char14ToDateTime(birthday, DateTime.UtcNow);
            return date.ToString("yyyy-MM-dd");
        }

        internal static string ConvertDays(int days)
        {
            const int daysInYear = 365;
            const int daysInMonth = 30;

            if (days >= daysInYear)
            {
                int years = days / daysInYear;
                return FormatArabicPlural(years, "سنة", "سنتان", "سنوات", "سنة");
            }

            if (days >= daysInMonth)
            {
                int months = days / daysInMonth;
                return FormatArabicPlural(months, "شهر", "شهران", "شهور", "شهر");
            }

            return FormatArabicPlural(days, "يوم", "يومان", "أيام", "يوم");
        }

        private static string FormatArabicPlural(int count, string singular, string dual, string few, string many)
        {
            return count switch
            {
                1 => $"{singular} واحد",
                2 => dual,
                _ when count >= 3 && count <= 10 => $"{count} {few}",
                _ => $"{count} {many}"
            };
        }
    }
}
