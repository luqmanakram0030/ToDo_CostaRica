using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ToDo_CostaRica.Helpers;
using ToDo_CostaRica.Models;
using ToDoCR.SharedDomain.Response;


namespace ToDo_CostaRica.Converters
{
    public class PersonaCivilResponseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            var dato = (RpTSE)value;
            if (parameter?.ToString() == "Civil" && dato.Civil != null)
            {
                return true;
            }
            else if (parameter?.ToString() == "Sociedad" && dato.Sociedad != null)
            {
                return true;
            }
            else if (parameter?.ToString() == "Dimex" && dato.Dimex != null)
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
