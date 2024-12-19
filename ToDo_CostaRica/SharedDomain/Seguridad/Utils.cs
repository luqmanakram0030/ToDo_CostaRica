using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoCR.SharedDomain.Seguridad
{
    public class Utils
    {
        /// <summary>
        /// Encripta un string que pueda ser enviado por medio del GET o POST y que pueda ser descifrado sin problemas
        /// </summary>
        /// <param name="v">Texto a encriptar</param>
        /// <param name="key">Opcional - Llave</param>
        /// <returns></returns>
        public static string EncriptarString(string v)
        {
            return PasswordAES.Encrypt(Keys.PassGeneral, v);
        }

        /// <summary>
        /// Desencripta un string que pueda ser enviado por medio del GET o POST y que pueda ser descifrado sin problemas
        /// </summary>
        /// <param name="v">Texto a desencriptar</param>
        /// <param name="key">Opcional - Llave</param>
        /// <returns></returns>
        public static string DesencriptarString(string v)
        {
            try
            {
                return PasswordAES.Decrypt(Keys.PassGeneral, v);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
