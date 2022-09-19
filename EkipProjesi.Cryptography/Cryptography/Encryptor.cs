using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace EkipProjesi.Cryptography.Cryptography
{
    internal class Encryptor
    {
        private string key = "";
        private string vector = "";
        private Rijndael rjn = Rijndael.Create();

        public Encryptor(string key, string vector)
        {
            this.key = key;
            this.vector = vector;
        }

        public string Decrypt(string encryptedText)
        {
            return this.Transform(Encoding.Default.GetString(ConversionUtilities.ConvertHexStringToByteArray(encryptedText)), this.rjn.CreateDecryptor(this.Key, this.Vector));
        }

        public string Encrypt(string Text)
        {
            return ConversionUtilities.ConvertByteArrayToHexString(Encoding.Default.GetBytes(this.Transform(Text, this.rjn.CreateEncryptor(this.Key, this.Vector))));
        }

        private string Transform(string Text, ICryptoTransform CryptoTransform)
        {
            MemoryStream stream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(stream, CryptoTransform, CryptoStreamMode.Write);
            byte[] Input = Encoding.Default.GetBytes(Text);
            cryptoStream.Write(Input, 0, Input.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.Default.GetString(stream.ToArray());
        }

        private byte[] Key
        {
            get
            {
                return ConversionUtilities.ConvertHexStringToByteArray(this.key);
            }
        }

        private byte[] Vector
        {
            get
            {
                return ConversionUtilities.ConvertHexStringToByteArray(this.vector);
            }
        }
    }
}