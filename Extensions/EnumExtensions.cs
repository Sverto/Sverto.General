using System;
using System.ComponentModel;
using System.Reflection;

namespace Sverto.General.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Checks if the value contains the provided type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Has<T>(this Enum type, T value)
        {
            try
            {
                return ((Convert.ToInt32((object)type) & Convert.ToInt32((object)value)) == Convert.ToInt32((object)value));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the value is only the provided type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Is<T>(this Enum type, T value)
        {
            try
            {
                return Convert.ToInt32((object)type) == Convert.ToInt32((object)value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Appends a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Set<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(Convert.ToInt32((object)type) | Convert.ToInt32((object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not append value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }

        /// <summary>
        /// Completely removes the value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Unset<T>(this Enum type, T value)
        {
            try
            {
                return (T)(object)(Convert.ToInt32((object)type) & ~Convert.ToInt32((object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), ex);
            }
        }

        public static string GetDescription(this Enum type)
        {
            FieldInfo fi = type.GetType().GetField(type.ToString());
            if (fi == null)
                throw new ArgumentException("Could not retrieve the enum item description. Does the enum has multiple values or an invalid value?");
            DescriptionAttribute[] attr = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attr.Length > 0)
            {
                return attr[0].Description;
            }
            else
            {
                return type.ToString();
            }
        }

    }
}
