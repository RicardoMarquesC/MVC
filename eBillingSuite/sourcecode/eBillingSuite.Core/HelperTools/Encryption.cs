using eBillingSuite.HelperTools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.HelperTools
{
    public class Encryption : IEncryption
    {
        public string Base64Decode(string data)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();

                byte[] todecode_byte = Convert.FromBase64String(data);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception cse)
            {
                throw;
            }
        }

        public string Sha512Encrypt(string data)
        {
            string rethash = "";
            try
            {
                System.Security.Cryptography.SHA512 hash = System.Security.Cryptography.SHA512.Create();
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                byte[] combined = encoder.GetBytes(data);
                encoder = null;
                hash.ComputeHash(combined);
                rethash = Convert.ToBase64String(hash.Hash);
            }
            catch (Exception)
            {
                throw;
            }
            return rethash;
        }

        public string Base64Encode(string data)
        {
            try
            {
                byte[] encData_byte = new byte[data.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(data);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
