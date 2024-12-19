using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using ToDoCR.SharedDomain.Seguridad;

namespace ToDoCR.SharedDomain
{
    public static class ExtensionMethods
    {

        public enum TitleCase
        {
            First,
            All
        }

        public enum TipoFechaFormato
        {
            Default,
            SoloAnio,
            AnioMes,
            FechaMesNombre,
            FechaDiaMesNombre
        }

        #region DateTime
        public static string HaceCuanto(this DateTime yourDate, DateTime? FechaActual = null)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            long TimeTicks = FechaActual?.Ticks ?? DateTime.UtcNow.AddHours(-6).Ticks;

            var ts = new TimeSpan(TimeTicks - yourDate.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "hace un segundo" : "hace " + ts.Seconds + " segundos";

            if (delta < 2 * MINUTE)
                return "hace un minuto";

            if (delta < 45 * MINUTE)
                return "hace " + ts.Minutes + " minutos";

            if (delta < 90 * MINUTE)
                return "hace una hora";

            if (delta < 24 * HOUR)
                return "hace " + ts.Hours + " horas";

            if (delta < 48 * HOUR)
                return "ayer";

            if (delta < 30 * DAY)
                return "hace " + ts.Days + " días";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "hace un mes" : "hace " + months + " meses";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "hace un año" : "hace " + years + " años";
            }
        }
        #endregion DateTime
        
        /// <summary>
        /// Get the Description from the DescriptionAttribute.
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            return enumValue.GetType()
                       .GetMember(enumValue.ToString())
                       .First()
                       .GetCustomAttribute<DescriptionAttribute>()?
                       .Description ?? string.Empty;
        }

        public static string GetMessageException(this Exception ex)
        {
            var message = ex.Message;
            var inner = ex.InnerException;
            while (inner != null)
            {
                message += " | " + inner?.Message;
                inner = inner.InnerException;
            }
            return message;
            //if (ex.InnerException != null)
            //    return getMessageException(ex.InnerException);
            //return ex.Message;
        }

        public static string FormateaCedula(this string item)
        {
            try
            {
                if (item.EsCedulaValida())
                {
                    return string.Format("{0}-{1}-{2}", item.Substring(0, 1), item.Substring(1, 4), item.Substring(5, 4));
                }
                else if (item.EsSociedadValida())
                {
                    return string.Format("{0}-{1}-{2}", item.Substring(0, 1), item.Substring(1, 3), item.Substring(4, 6));
                }
            }
            catch (Exception)
            {
            }
            return item;
        }

        #region Token
        /// <summary>
        /// Encripta un string que pueda ser enviado por medio del GET o POST y que pueda ser descifrado sin problemas
        /// </summary>
        /// <param name="v">Texto a encriptar</param>
        /// <param name="key">Opcional - Llave</param>
        /// <returns></returns>
        public static string Encriptar(this string v)
        {
            return Base64UrlEncoder.Encode(PasswordAES.Encrypt(Keys.PassGeneral, v));

        }

        /// <summary>
        /// Desencripta un string que pueda ser enviado por medio del GET o POST y que pueda ser descifrado sin problemas
        /// </summary>
        /// <param name="v">Texto a desencriptar</param>
        /// <param name="key">Opcional - Llave</param>
        /// <returns></returns>
        public static string Desencriptar(this string v)
        {
            return PasswordAES.Decrypt(Keys.PassGeneral, Base64UrlEncoder.Decode(v));
        }

        #endregion Token

        #region Fechas

        /// <summary>
        /// Obtiene en texto la fecha
        /// </summary>
        /// <param name="Fecha"></param>
        /// <returns>{dia}, {Fecha.Day} de {mes} de {Fecha.Year}</returns>
        public static string OtenerFechaEspannol(this DateTime Fecha, bool AgregarDia = true)
        {
            string dia = Fecha.DayOfWeek.ObtenerDiaSemanaEspannol();
            string mes = Fecha.Month.GetMonthName();
            return (AgregarDia ? $"{dia}, " : "") + $"{Fecha.Day} de {mes} de {Fecha.Year}";
        }

        /// <summary>
        /// Obtiene el nombre del dia de la semana
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string ObtenerDiaSemanaEspannol(this DayOfWeek day)
        {
            CultureInfo dtinfo = new CultureInfo("es-ES", false);
            return dtinfo.DateTimeFormat.GetDayName(day).FirstCap();
        }

        /// <summary>
        /// Obtiene el nombre del mes en español
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string GetMonthName(this int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month).FirstCap();
        }

        #endregion Fechas
        public static string ObtenerStringRestante(this TimeSpan Tiempo, int CantidadDiasPorMes = 30)
        {
            string restante = "";
            int annos = (int)Tiempo.TotalDays / 365;
            if (annos == 1)
            {
                restante = $"{restante} 1 año";
            }
            else
            {
                restante = $"{restante} {annos} años";
            }
            int meses = (int)((Tiempo.TotalDays - annos * 365) / CantidadDiasPorMes);
            if (meses == 1)
            {
                restante = $"{restante} 1 mes";
            }
            else
            {
                restante = $"{restante} {meses} meses";
            }
            int dias = (int)(Tiempo.TotalDays - (annos * 365) - (meses * CantidadDiasPorMes));
            if (meses == 1)
            {
                restante = $"{restante} 1 día";
            }
            else
            {
                restante = $"{restante} y {dias} días";
            }
            return restante;
        }

        public static DateTime FirstDay(this DateTime now)
        {
            return new DateTime(now.Year, now.Month, 1);
        }

        public static string NombrePeriodo(this string Periodo)
        {
            string año = Periodo.Substring(0, 4);
            string mes = Periodo.Substring(4, 2);
            string PeriodoNombre = "";

            if (mes == "01")
            {
                PeriodoNombre = "Ene " + año.Substring(2, 2);
            }
            else if (mes == "02")
            {
                PeriodoNombre = "Feb " + año.Substring(2, 2);
            }
            else if (mes == "03")
            {
                PeriodoNombre = "Mar " + año.Substring(2, 2);
            }
            else if (mes == "04")
            {
                PeriodoNombre = "Abr " + año.Substring(2, 2);
            }
            else if (mes == "05")
            {
                PeriodoNombre = "May " + año.Substring(2, 2);
            }
            else if (mes == "06")
            {
                PeriodoNombre = "Jun " + año.Substring(2, 2);
            }
            else if (mes == "07")
            {
                PeriodoNombre = "Jul " + año.Substring(2, 2);
            }
            else if (mes == "08")
            {
                PeriodoNombre = "Ago " + año.Substring(2, 2);
            }
            else if (mes == "09")
            {
                PeriodoNombre = "Set " + año.Substring(2, 2);
            }
            else if (mes == "10")
            {
                PeriodoNombre = "Oct " + año.Substring(2, 2);
            }
            else if (mes == "11")
            {
                PeriodoNombre = "Nov " + año.Substring(2, 2);
            }
            else if (mes == "12")
            {
                PeriodoNombre = "Dic " + año.Substring(2, 2);
            }

            return PeriodoNombre;
        }

        public static string NombrePeriodoTotal(this string Periodo)
        {
            string año = Periodo.Substring(0, 4);
            string mes = Periodo.Substring(4, 2);
            string PeriodoNombre = "";

            if (mes == "01")
            {
                PeriodoNombre = "Enero " + año;
            }
            else if (mes == "02")
            {
                PeriodoNombre = "Febrero " + año;
            }
            else if (mes == "03")
            {
                PeriodoNombre = "Marzo " + año;
            }
            else if (mes == "04")
            {
                PeriodoNombre = "Abilr " + año;
            }
            else if (mes == "05")
            {
                PeriodoNombre = "Mayo " + año;
            }
            else if (mes == "06")
            {
                PeriodoNombre = "Junio " + año;
            }
            else if (mes == "07")
            {
                PeriodoNombre = "Julio " + año;
            }
            else if (mes == "08")
            {
                PeriodoNombre = "Agosto " + año;
            }
            else if (mes == "09")
            {
                PeriodoNombre = "Setiembre " + año;
            }
            else if (mes == "10")
            {
                PeriodoNombre = "Octubre " + año;
            }
            else if (mes == "11")
            {
                PeriodoNombre = "Noviembre " + año;
            }
            else if (mes == "12")
            {
                PeriodoNombre = "Diciembre " + año;
            }

            return PeriodoNombre;
        }

        /// <summary>
        /// Obtiene DateTime con el ultimo dia del mes
        /// </summary>
        /// <param name="now"></param>
        /// <returns>Datetime con el ultimo dia del mes</returns>
        public static DateTime LastDay(this DateTime now)
        {
            var startDate = new DateTime(now.Year, now.Month, 1);
            return startDate.AddMonths(1).AddDays(-1);
        }

        public static string Formato(this string pTexto, TipoFechaFormato tipoFecha = TipoFechaFormato.Default, string formato = "yyyyMMdd")
        {
            if (string.IsNullOrEmpty(pTexto))
                return "";

            if (tipoFecha != TipoFechaFormato.Default)
            {
                DateTime? fecha = null;
                CultureInfo culture = new CultureInfo("es-Cr");
                switch (tipoFecha)
                {
                    case TipoFechaFormato.SoloAnio:
                        fecha = pTexto.ParsearFecha(formato);

                        if (fecha.HasValue)
                            return fecha.Value.ToString("yyyy", culture);
                        break;

                    case TipoFechaFormato.AnioMes:
                        fecha = pTexto.ParsearFecha(formato);

                        if (fecha.HasValue)
                            return fecha.Value.ToString("yyyy MMMM", culture);
                        break;

                    case TipoFechaFormato.FechaMesNombre:
                        fecha = pTexto.ParsearFecha(formato);

                        if (fecha.HasValue)
                            return fecha.Value.ToString("MMMM yyyy", culture);
                        break;

                    case TipoFechaFormato.FechaDiaMesNombre:
                        fecha = pTexto.ParsearFecha(formato);

                        if (fecha.HasValue)
                            return fecha.Value.ToString("dddd dd 'de' MMMM yyyy", culture);
                        break;
                }
            }
            return Regex.Replace(pTexto.Trim(), @"\s+", " ").Replace("'", "").Replace(" ", "").Trim();
        }

        public static DateTime? ParsearFecha(this string date, string formato)
        {
            DateTime fecha;
            if (DateTime.TryParseExact(date, formato, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out fecha))
            {
                if (fecha.Year < 1900)
                    return null;
                return fecha;
            }
            return null;
        }

        public static DateTime? ParsearFecha(this string date, string formato, string pCultureInfo)
        {
            DateTime fecha;
            if (DateTime.TryParseExact(date, formato, new CultureInfo(pCultureInfo), DateTimeStyles.AssumeLocal, out fecha))
                return fecha;
            return null;
        }
        public static string FirstCap(this string str, TitleCase tcase = TitleCase.All)
        {
            if (str == null)
                return "";
            str = str.Formato().ToLower();
            switch (tcase)
            {
                case TitleCase.First:
                    var strArray = str.Split(' ');
                    if (strArray.Length > 1)
                    {
                        strArray[0] = ci.TextInfo.ToTitleCase(strArray[0]);
                        return string.Join(" ", strArray);
                    }
                    break;

                case TitleCase.All:
                    return ci.TextInfo.ToTitleCase(str);

                default:
                    break;
            }
            return ci.TextInfo.ToTitleCase(str);
        }
        public static string Exec_FirmarXML(this string comprobanteXml, string llave, string pin)
        {
            //* Create your Process
            List<string> response = new List<string>();

            Process process = new Process();
            process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "Models\\Ejecutables\\firmadorxml.exe";
            var guid = Guid.NewGuid();

            try
            {
                //File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + $"Models\\Ejecutables\\Comprobante_{guid}.xml", Convert.FromBase64String(comprobanteXml));
                //File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + $"Models\\Ejecutables\\_Llave{guid}.xml", Convert.FromBase64String(llave));
                //File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + $"Models\\Ejecutables\\Pin_{guid}.xml", Convert.FromBase64String(pin));

                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + $"Models\\Ejecutables\\Comprobante_{guid}.xml", comprobanteXml);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + $"Models\\Ejecutables\\_Llave{guid}.xml", llave);
                File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + $"Models\\Ejecutables\\Pin_{guid}.xml", pin);
            }
            catch (Exception)
            {
                //throw ex;
            }

            process.StartInfo.Arguments = $"{comprobanteXml} {llave} {pin}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            //* Set your output and error (asynchronous) handlers
            process.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                if (e.Data != null)
                    response.Add(e.Data);
            };
            process.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e)
            {
                if (e.Data != null)
                    response.Add(e.Data);
            };
            //* Start process and handlers
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
            if (response.Count > 1)
            {
                string mensajeEx = "";
                foreach (string a in response)
                {
                    mensajeEx = $"{mensajeEx}{Environment.NewLine}{a}";
                }
                throw new Exception(mensajeEx);
            }
            return response.FirstOrDefault();
        }

        public static bool ContieneTexto(this string Data, string Texto)
        {
            if (string.IsNullOrEmpty(Data) || string.IsNullOrEmpty(Texto))
            {
                return false;
            }
            var nuevoData = Data.Replace("-", "").Replace("_", "");
            return nuevoData.Contains(Texto);
        }

        private static CultureInfo ci = new CultureInfo("en-US");

        #region Fechas
        /// <summary>
        /// Obtiene la fecha como Enero-2021
        /// </summary>
        /// <param name="Fecha">Fecha a parsear</param>
        /// <param name="formato">Formato de divisor</param>
        /// <param name="CantidadCaracteresTomarMes">Si es nulo, agarra todo el string del mes, sino, toma la cantidad que se le indique</param>
        /// <returns></returns>
        public static string ObtenerFechaMesAnnioEspannol(this DateTime Fecha, string formato = "-", int? CantidadCaracteresTomarMes = 3)
        {
            string mes = Fecha.Month.GetMonthName();
            return CantidadCaracteresTomarMes == null ? mes : mes.Substring(0, (int)CantidadCaracteresTomarMes) + $"{formato}{Fecha.Year}";
        }
        #endregion Fechas

        /// <summary>
        /// Retorna booleano si es o no un email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>


        #region Cedula y sociedad valida

        public static bool EsCedulaValida(this string item)
        {
            if (string.IsNullOrWhiteSpace(item))
                return false;
            item = item.Formato();
            if (!String.IsNullOrWhiteSpace(item) && item.Count() == 9 && item != "000000000" && item != "NO INDICA" && item != "200000000" && item.EsNumerico() && item.First() != '0')
                return true;
            return false;
        }

        public static bool EsSociedadValida(this string item)
        {
            item = item.Formato();
            Int64 sociedadCed;
            if (!String.IsNullOrWhiteSpace(item) && item.Count() == 10 && item != "000000000" && item != "NO INDICA" && Int64.TryParse(item, out sociedadCed))
                return true;
            return false;
        }

        public static bool EsDIMEX(this string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
                return false;
            try
            {
                var peso = 7317317317;
                valor = valor.Replace("-", "").Trim();
                if (valor.Length == 12 && long.TryParse(valor, out long valornumerico))
                {
                    int sumatoria = 0;
                    for (int i = 0; i < 10; i++)
                    {
                        sumatoria += (Convert.ToInt32(Char.GetNumericValue(valornumerico.ToString()[i])) * Convert.ToInt32(Char.GetNumericValue(peso.ToString()[i])));
                    }
                    var resultado = sumatoria % 37;
                    // Esta expresión rara, por si te da dudas, es lo mismo que hacer: valor[valor.Length - 2]
                    var verificador = valor.Substring(valor.Count() - 2);
                    if (resultado == Convert.ToInt32(verificador))
                        return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        #endregion Cedula y sociedad valida

        public static bool EsNumerico(this string item)
        {
            int n;
            return int.TryParse(item, out n);
        }


        public static string GetQueryString(this object obj)
        {
            var result = new List<string>();
            var props = obj.GetType().GetProperties().Where(p => p.GetValue(obj, null) != null);
            foreach (var p in props)
            {
                var value = p.GetValue(obj, null);
                var enumerable = value as System.Collections.ICollection;
                if (enumerable != null)
                {
                    result.AddRange(from object v in enumerable select string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(v.ToString())));
                }
                else
                {
                    result.Add(string.Format("{0}={1}", p.Name, HttpUtility.UrlEncode(value.ToString())));
                }
            }

            return string.Join("&", result.ToArray());
        }

        public static string RemoveAllNamespaces(this XDocument xmlDoc)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDoc.ToString()));

            return xmlDocumentWithoutNs.ToString();
        }

        public static string RemoveAllNamespaces(this string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            return xmlDocumentWithoutNs.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }

        /// <summary>
        /// Retorna booleano si es o no un email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool EsEmail(this string item)
        {
            if (String.IsNullOrEmpty(item))
                return false;

            item = item.Formato();

            const string pattern = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|'(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*')@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9]))\.){3}(?:(2(5[0-5]|[0-4][0-9])|1[0-9][0-9]|[1-9]?[0-9])|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";
            //const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
            const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

            // Set explicit regex match timeout, sufficient enough for email parsing
            // Unless the global REGEX_DEFAULT_MATCH_TIMEOUT is already set
            TimeSpan matchTimeout = TimeSpan.FromSeconds(2);

            try
            {
                if (AppDomain.CurrentDomain.GetData("REGEX_DEFAULT_MATCH_TIMEOUT") == null)
                {
                    return new Regex(pattern, options, matchTimeout).IsMatch(item);
                }
            }
            catch
            {
                // Fallback on error
            }

            // Legacy fallback (without explicit match timeout)
            return new Regex(pattern, options).IsMatch(item);
        }

    }
}
