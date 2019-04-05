using System;
using System.Collections.Generic;
using System.Text;

namespace BasicAudio.Extensions
{

    /// <summary>
    /// Various extension methods used by BasicAudio.
    /// </summary>
    public static class ExtensionMethods
    {
        //*********************************************************************************************************************
        //
        //             Class:  StringExtensions
        //      Organization:  http://www.blakepell.com
        //     Programmer(s):  Blake Pell, blakepell@hotmail.com
        //
        //*********************************************************************************************************************

        /// <summary>
        /// This function will return the specified amount of characters from the left hand side of the string.  This is the equivalent of the Visual Basic Left function.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Left(this string str, int length)
        {
            return str.Substring(0, length);
        }

        /// <summary>
        /// This function will return the specified amount of characters from the right hand side of the string.  This is the equivalent of the Visual Basic Right function.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string Right(this string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }

        /// <summary>
        /// Returns the specified number of characters from the left hand side of the string.  If the number asked for is longer the
        /// string then the entire string is returned without an exception.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SafeLeft(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }

            if (length >= str.Length)
            {
                return str;
            }
            else if (length < 0)
            {
                return "";
            }

            return Left(str, length);
        }

        /// <summary>
        /// Returns the specified number of characters from the right hand side of the string.  If the number asked for is longer the
        /// string then the entire string is returned without an exception.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string SafeRight(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }

            if (length >= str.Length)
            {
                return str;
            }
            else if (length < 0)
            {
                return "";
            }

            return Right(str, length);
        }

        /// <summary>
        /// Determines whether a string is a numeric value.  This implementation uses Decimal.TryParse to produce it's value.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool IsNumeric(this string str)
        {
            return decimal.TryParse(str, out decimal result);
        }

    }
}
