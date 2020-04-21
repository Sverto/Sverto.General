using System.ComponentModel;
using System.Linq;

namespace Sverto.General.Coding
{
    public static class ClassHelper
    {

        /// <summary>
        /// Set all default value's of <!--<DefaultValue(...)>--> on their properties
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        public static void SetDefaultValueOnProperties<T>(T @this)
        {
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(@this))
            {
                DefaultValueAttribute attr = (DefaultValueAttribute)prop.Attributes.OfType<DefaultValueAttribute>();
                if (attr != null)
                {
                    prop.SetValue(@this, attr.Value);
                }
            }
        }

    }
}
