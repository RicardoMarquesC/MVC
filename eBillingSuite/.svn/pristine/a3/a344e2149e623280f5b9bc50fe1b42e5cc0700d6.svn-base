using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eBillingSuite.Support
{
    public static class Parse
    {
        //isInvariableSum indica se o decimal será usado para fazer uma soma ignorando o sinal
        //e.g. 100 + (-400) = 500
        public static float ToFloat(string str, bool isInvariableSum)
        {
            if (isInvariableSum && !string.IsNullOrEmpty(str))
                str = str.Replace("-", String.Empty).Replace(".", ",");

            // you can throw an exception or return a default value here
            if (string.IsNullOrEmpty(str))
                return 0;

            float f;

            // you could throw an exception or return a default value on failure
            if (!float.TryParse(str, out f))
                return 0;

            return f;
        }

        public static decimal ToDecimal(string str, bool isInvariableSum)
        {
            if (isInvariableSum && !string.IsNullOrEmpty(str))
                str = str.Replace("-", String.Empty).Replace(".", ",");

            // you can throw an exception or return a default value here
            if (string.IsNullOrEmpty(str))
                return 0;

            decimal d;

            // you could throw an exception or return a default value on failure
            if (!decimal.TryParse(str, out d))
                return 0;

            return d;
        }
    }
}
