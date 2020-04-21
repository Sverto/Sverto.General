using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Sverto.General.Extensions
{
    public static class StringExtensions
    {

        /// <summary>
        /// Reformat special characters like 'ç' or 'é' to 'c', 'e'
        /// </summary>
        /// <param name="text">String to replace special characters in</param>
        /// <returns></returns>
        public static string RemoveDiacritics(this string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Returns a secure string from the source string
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static SecureString ToSecureString(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;
            else
            {
                SecureString result = new SecureString();
                foreach (char c in source.ToCharArray())
                    result.AppendChar(c);
                return result;
            }
        }

        /// <summary>
        /// Returns a string from the source secure string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String ToStringEx(this SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        /// <summary>
        /// Set source empty or whitespace string to null
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static String EmptyToNull(this string source)
        {
            return String.IsNullOrWhiteSpace(source) ? null : source;
        }

    }
}
