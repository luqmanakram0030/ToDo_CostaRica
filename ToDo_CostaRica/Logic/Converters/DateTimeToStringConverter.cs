using System;
using System.Collections.Generic;
using System.Globalization;


namespace ToDo_CostaRica.Converters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            if (value is DateTimeOffset)
            {
                var datetime = (DateTimeOffset)value;
                return datetime.ToString("dddd, dd 'de' MMMM yyyy", new CultureInfo("ES-es"));
            }
            else
            {
                var datetime = (DateTime)value;
                return datetime.ToString("dddd, dd 'de' MMMM yyyy", new CultureInfo("ES-es"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
