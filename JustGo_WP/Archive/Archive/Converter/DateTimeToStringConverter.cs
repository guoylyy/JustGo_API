using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Archive.Converter
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var time = value is DateTime ? (DateTime) value : new DateTime();
            string amorpm = time.Hour < 12 ? "AM" : "PM";
            int hour = time.Hour > 12 ? time.Hour - 12 : time.Hour;
            //if (time.Hour > 12)
            //{
            //    amorpm = "PM";
            //    hour = time.Hour - 12;
            //}
            //else
            //{
            //    amorpm = "AM";
            //    hour = time.Hour;
            //}
            string returnString = string.Format("{0}-{1}-{2} {3}:{4} {5}",
                time.Year,
                time.Month,
                time.Day,
                hour,
                time.Minute,
                amorpm);

            return returnString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
