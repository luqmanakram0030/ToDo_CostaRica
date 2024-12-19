using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToDoCR.SharedDomain.Seguridad
{
  public class PasswordAES
  {
    public const int KEY_SIZE = 16;

    public static string Encrypt(string password, string input)
    {
      var sha256CryptoServiceProvider = new SHA256CryptoServiceProvider();
      var hash = sha256CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
      var key = new byte[KEY_SIZE];
      var iv = new byte[KEY_SIZE];

      Buffer.BlockCopy(hash, 0, key, 0, KEY_SIZE);
      Buffer.BlockCopy(hash, KEY_SIZE, iv, 0, KEY_SIZE);

      using (var cipher = new AesCryptoServiceProvider().CreateEncryptor(key, iv))
      using (var output = new MemoryStream())
      {
        using (var cryptoStream = new CryptoStream(output, cipher, CryptoStreamMode.Write))
        {
          var inputBytes = Encoding.UTF8.GetBytes(input);
          cryptoStream.Write(inputBytes, 0, inputBytes.Length);
        }
        return Convert.ToBase64String(output.ToArray());
      }
    }

    public static byte[] EncryptBytes(string password, string input)
    {
      var sha256CryptoServiceProvider = new SHA256CryptoServiceProvider();
      var hash = sha256CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
      var key = new byte[KEY_SIZE];
      var iv = new byte[KEY_SIZE];

      Buffer.BlockCopy(hash, 0, key, 0, KEY_SIZE);
      Buffer.BlockCopy(hash, KEY_SIZE, iv, 0, KEY_SIZE);

      using (var cipher = new AesCryptoServiceProvider().CreateEncryptor(key, iv))
      using (var output = new MemoryStream())
      {
        using (var cryptoStream = new CryptoStream(output, cipher, CryptoStreamMode.Write))
        {
          var inputBytes = Encoding.UTF8.GetBytes(input);
          cryptoStream.Write(inputBytes, 0, inputBytes.Length);
        }
        return output.ToArray();
      }
    }

    public static string Decrypt(string password, string encryptedBytes)
    {
      try
      {
        var sha256CryptoServiceProvider = new SHA256CryptoServiceProvider();
        var hash = sha256CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
        var key = new byte[KEY_SIZE];
        var iv = new byte[KEY_SIZE];

        Buffer.BlockCopy(hash, 0, key, 0, KEY_SIZE);
        Buffer.BlockCopy(hash, KEY_SIZE, iv, 0, KEY_SIZE);

        using (var cipher = new AesCryptoServiceProvider().CreateDecryptor(key, iv))
        using (var source = new MemoryStream(Convert.FromBase64String(encryptedBytes)))
        using (var output = new MemoryStream())
        {
          using (var cryptoStream = new CryptoStream(source, cipher, CryptoStreamMode.Read))
          {
            cryptoStream.CopyTo(output);
          }
          return Encoding.UTF8.GetString(output.ToArray());
        }
      }
      catch (Exception ex)
      {
        Debug.WriteLine("ERROR ===> " + ex);
        return null;
      }
    }
  }
}
