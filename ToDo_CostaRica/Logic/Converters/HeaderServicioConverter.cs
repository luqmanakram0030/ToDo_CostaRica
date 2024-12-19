using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ToDo_CostaRica.Helpers;
using ToDo_CostaRica.Models;


namespace ToDo_CostaRica.Converters
{
    public class HeaderServicioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dato = (HeaderServicioEnum)value;
            var parameters = parameter.ToString()?.Split('|');

            if (parameter?.ToString().EndsWith("Border") == true)
            {
                if (parameters.Contains($"step{(int)dato}Border"))
                {
                    return "ColorOnDarkBackground".ObtenerRecurso<Color>();
                }
                else
                {
                    return "ShadowColor".ObtenerRecurso<Color>();
                }
            }
            else if (parameter?.ToString().EndsWith("Label") == true)
            {
                if (parameters.Contains($"step{(int)dato}Label"))
                {
                    return "ColorOnDarkBackground".ObtenerRecurso<Color>();
                }
                else
                {
                    return "ShadowColor".ObtenerRecurso<Color>();
                }
            }
            else
            {
                if (parameter?.ToString() == $"step{(int)dato}Visible")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
