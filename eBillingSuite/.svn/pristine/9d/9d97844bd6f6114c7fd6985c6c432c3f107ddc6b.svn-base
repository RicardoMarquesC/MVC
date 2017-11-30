using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Support
{
    public class StringUtilities
    {
        public static string padString(char padChar, int finalLength, string dataToPad, bool afterDataToPad)
        {
            if (dataToPad == null)
                return (null);

            if (dataToPad.Length >= finalLength)
                return (dataToPad);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (afterDataToPad)
                sb.Append(dataToPad);

            for (int k = 0; k < (finalLength - dataToPad.Length); k++)
                sb.Append(padChar);

            if (!afterDataToPad)
                sb.Append(dataToPad);

            return (sb.ToString());

        }

        public static string RemoveSpecialCharsForFilename(string input, string replaceChar)
        {
            string output = input
                .Replace("\\", replaceChar)
                .Replace("/", replaceChar)
                .Replace(":", replaceChar)
                .Replace("*", replaceChar)
                .Replace("?", replaceChar)
                .Replace("\"", replaceChar)
                .Replace("<", replaceChar)
                .Replace(">", replaceChar)
                .Replace("|", replaceChar);

            return output;
        }
    }
}
