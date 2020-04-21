using System;
using System.Collections.Generic;

namespace Sverto.General.Extensions
{
    public static class EnumerableExtensions
    {

        /// <summary>
        /// Take the last x items from IEnumerable(Of T)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> items, int count)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "n must be 0 or greater");
            // Build list
            //Dim queue As New Queue(Of T)
            //For Each item As T In items
            //    queue.Enqueue(item)
            //    If queue.Count > count Then
            //        queue.Dequeue()
            //    End If
            //Next
            LinkedList<T> linkedList = new LinkedList<T>();
            foreach (T value in items)
            {
                linkedList.AddLast(value);
                if (linkedList.Count > count)
                {
                    linkedList.RemoveFirst();
                }
            }
            return linkedList;
        }

    }
}
