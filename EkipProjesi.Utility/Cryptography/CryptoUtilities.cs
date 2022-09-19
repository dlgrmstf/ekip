using System.Configuration;

namespace EkipProjesi.Utility.Cryptography
{
    public class CryptoUtilities
    {


        public static string Decrypt(string text)
        {

            Encryptor en = new Encryptor(ConfigurationManager.AppSettings["CryptoKey"], ConfigurationManager.AppSettings["CryptoVector"]);
            return en.Decrypt(text);
        }

        public static string Encrypt(string text)
        {
            Encryptor en = new Encryptor(ConfigurationManager.AppSettings["CryptoKey"], ConfigurationManager.AppSettings["CryptoVector"]);
            return en.Encrypt(text);
        }
    }
}