using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;



namespace ToDo_CostaRica.Converters
{
    public class MisNumerosTipoLoteriaConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            if (value == parameter)
                return true;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
