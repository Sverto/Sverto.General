using System;
using System.Reflection;
using System.Windows.Forms;

namespace Sverto.General.Extensions
{
    public static class ControlExtensions
    {

        /// <summary>
        /// Enable double buffering without inheriting
        /// </summary>
        /// <param name="control"></param>
        /// <param name="doubleBuffered"></param>
        public static void SetDoubleBuffered(this Control control, bool doubleBuffered)
        {
            Type controlType = control.GetType();
            PropertyInfo pi = controlType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(control, doubleBuffered, null);
        }

    }
}
