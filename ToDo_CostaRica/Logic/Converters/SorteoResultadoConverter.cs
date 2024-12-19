using System;

using static ToDoCR.SharedDomain.JPSModels.JPSModels;

namespace ToDo_CostaRica.Converters
{
    public class SorteoResultadoConverter : IValueConverter
    {
        #region IValueConverter implementation

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var premio = (DevuelvePremios)value;
            var parametros = parameter?.ToString().Split(' ');
            if (parameter is Label && int.TryParse(((Label)parameter).Text, out int Fracciones))
            {
                var total = premio.MontoUnitario * Fracciones;
                return $"₡{total.ToString("N0")}";
            }
            else if (parametros[0] == "tipopremio")
            {
                if (premio.TipoPremio == 3 && premio.SubPremio == 0)
                    return "Ha sido ganador del tercer premio.";
                else if (premio.TipoPremio == 2 && premio.SubPremio == 0)
                    return "Ha sido ganador del segundo premio.";
                else if (premio.TipoPremio == 1 && premio.SubPremio == 0)
                    return "Ha sido ganador del primer premio.";
                else
                    return premio.Descripcion;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
