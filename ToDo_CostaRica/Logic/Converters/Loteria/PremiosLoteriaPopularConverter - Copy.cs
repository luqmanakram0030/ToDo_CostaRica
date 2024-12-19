using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using static ToDoCR.SharedDomain.JPSModels.JPSModels;

namespace ToDo_CostaRica.Converters
{
    public class PremiosLoteriaPopularConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var lista = (List<PremiosLoteriaPopular>)value;

            lista = lista.Where(p => p.Tipo > 3).OrderBy(p => p.Numero).ThenBy(p => p.Serie).ToList();
            return lista;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
