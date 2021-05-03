/*
 * @author            : Blake Pell
 * @initial date      : 2007-03-31
 * @last updated      : 2021-05-02
 * @copyright         : Copyright (c) 2003-2021, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

namespace BasicAudio.Extensions
{
    /// <summary>
    /// Various extension methods used by BasicAudio.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// This function will return the specified amount of characters from the right hand side of the string.  This is the equivalent of the Visual Basic Right function.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        public static string Right(this string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }

        /// <summary>
        /// Returns the specified number of characters from the right hand side of the string.  If the number asked for is longer the
        /// string then the entire string is returned without an exception.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        public static string SafeRight(this string str, int length)
        {
            if (string.IsNullOrEmpty(str) || length <= 0)
            {
                return "";
            }

            if (length >= str.Length)
            {
                return str;
            }

            return Right(str, length);
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