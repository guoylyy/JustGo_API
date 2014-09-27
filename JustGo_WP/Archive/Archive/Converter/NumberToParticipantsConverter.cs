using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Archive.Converter
{
    public class NumberToParticipantsConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return null;

            var number = (int) value;
            string numStr = string.Empty;
            
            while (number > 1000)
            {
                var remain = number % 1000;
                number = number/1000;
                string remainStr = remain.ToString();
                if (remain < 10)
                {
                    remainStr = "00" + remainStr;
                }
                else if(remain <100)
                {
                    remainStr = "0" + remainStr;
                }
                numStr = "," + remainStr + numStr;
            }
            numStr = number + numStr + " Participants";
            return numStr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
