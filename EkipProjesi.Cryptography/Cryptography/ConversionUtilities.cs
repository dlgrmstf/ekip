using System.Globalization;
using System.Text;

namespace EkipProjesi.Cryptography.Cryptography
{
    public sealed class ConversionUtilities
    {
        public static string ConvertByteArrayToHexString(byte[] array)
        {
            StringBuilder sb = new StringBuilder(array.Length * 2);
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        public static byte[] ConvertHexStringToByteArray(string str)
        {
            int len = str.Length / 2;
            string str2convert = str;
            byte[] retval = new byte[len];
            for (int i = 0; i < str2convert.Length; i += 2)
            {
                retval[i / 2] = byte.Parse(str2convert.Substring(i, 2), NumberStyles.AllowHexSpecifier);
            }
            return retval;
        }
    }
}