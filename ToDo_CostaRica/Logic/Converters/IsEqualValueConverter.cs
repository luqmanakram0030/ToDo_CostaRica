using System;


namespace ToDo_CostaRica.Converters
{
    public class IsEqualValueConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == parameter)
                return true;
            if (int.TryParse(parameter?.ToString(), out int theInt) && int.TryParse(value?.ToString(), out int theValInt))
            {
                if (theValInt == theInt)
                    return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
