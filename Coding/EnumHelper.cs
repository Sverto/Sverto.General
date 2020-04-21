using System;
using System.Collections.Generic;
using System.Linq;

namespace Sverto.General.Coding
{
    public static class EnumHelper
    {

        /// <summary>
        /// Get IEnumerable containing Enum values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>IEnumerable containing Enum values</returns>
        public static IEnumerable<T> ToIEnumerable<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

    }
}
