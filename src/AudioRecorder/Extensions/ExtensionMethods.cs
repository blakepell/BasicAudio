/*
 * @author            : Blake Pell
 * @initial date      : 2007-03-31
 * @last updated      : 2021-05-02
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using System;

namespace AudioRecorder.Extensions
{
    public static class ExtensionMethods
    {

        /// <summary>
        /// Formats a number with commas and the specified number of decimal places.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="decimalPlaces"></param>
        public static string FormatIfNumber(this string str, int decimalPlaces)
        {
            if (IsNumeric(str) == true)
            {
                string formatString = "#,0.";

                formatString = formatString.PadRight(formatString.Length + decimalPlaces, '0');

                double x = Convert.ToDouble(str);
                return x.ToString(formatString);
            }

            return str;
        }

        /// <summary>
        /// Determines whether a string is a numeric value.  This implementation uses Decimal.TryParse to produce it's value.
        /// </summary>
        /// <param name="str"></param>
        public static bool IsNumeric(this string str)
        {
            return decimal.TryParse(str, out decimal _);
        }

    }
}
