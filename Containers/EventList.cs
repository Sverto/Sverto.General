using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sverto.General.Containers
{
    public class EventList<T> : IList<T>, ICollection<T>, IEnumerable<T>
    {
        //.NET4: IReadOnlyList(Of T), IReadOnlyCollection(Of T) ', IBindingList

        /// <summary>
        /// Initializes a new instance of the ExtendedList(Of T) class that is empty and has the default initial capacity.
        /// </summary>
        public EventList()
        {
            _List = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the ExtendedList(Of T) class that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public EventList(int capacity)
        {
            _List = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the ExtendedList(Of T) class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection"></param>
        public EventList(IEnumerable<T> collection)
        {
            _List = new List<T>(collection);
        }

        /// <summary>
        /// Initializes a new instance of the ExtendedList(Of T) class that uses the specified List(Of T).
        /// </summary>
        /// <param name="list"></param>
        public EventList(List<T> list)
        {
            _List = list;
        }


        protected readonly List<T> _List;
        #region "Events"
        public event AddingEventHandler Adding;
        public delegate void AddingEventHandler(object sender, EventListEventArgs<T> e);
        public event AddedEventHandler Added;
        public delegate void AddedEventHandler(object sender, EventListEventArgs<T> e);
        public event RemovingEventHandler Removing;
        public delegate void RemovingEventHandler(object sender, EventListEventArgs<T> e);
        public event RemovedEventHandler Removed;
        public delegate void RemovedEventHandler(object sender, EventListEventArgs<T> e);
        public event ClearingEventHandler Clearing;
        public delegate void ClearingEventHandler(object sender, EventListEventArgs<T> e);
        public event ClearedEventHandler Cleared;
        public delegate void ClearedEventHandler(object sender, EventArgs e);

        public object EventLock { get; }
        // This will also pauze any method that is waiting for the events' output!

        protected void OnAdding(object sender, EventListEventArgs<T> e)
        {
            lock (EventLock)
            {
                if (Adding != null)
                {
                    Adding(this, e);
                }
            }
        }

        protected void OnAdded(object sender, EventListEventArgs<T> e)
        {
            lock (EventLock)
            {
                if (Added != null)
                {
                    Added(this, e);
                }
            }
        }

        protected void OnRemoving(object sender, EventListEventArgs<T> e)
        {
            lock (EventLock)
            {
                if (Removing != null)
                {
                    Removing(this, e);
                }
            }
        }

        protected void OnRemoved(object sender, EventListEventArgs<T> e)
        {
            lock (EventLock)
            {
                if (Removed != null)
                {
                    Removed(this, e);
                }
            }
        }

        protected void OnClearing(object sender, EventListEventArgs<T> e)
        {
            lock (EventLock)
            {
                if (Clearing != null)
                {
                    Clearing(this, e);
                }
            }
        }

        protected void OnCleared(object sender, EventArgs e)
        {
            lock (EventLock)
            {
                if (Cleared != null)
                {
                    Cleared(this, e);
                }
            }
        }
        #endregion

        #region "Properties"
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            //, IReadOnlyList(Of T).Item
            get { return _List[index]; }
            set { _List[index] = value; }
        }

        /// <summary>
        /// Gets the number of elements contained in the ExtendedList(Of T).
        /// </summary>
        /// <returns></returns>
        public int Count
        {
            //, IReadOnlyList(Of T).Count
            get { return _List.Count; }
        }

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <returns></returns>
        public int Capacity
        {
            get { return _List.Capacity; }
            set { _List.Capacity = value; }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region "Convert"
        /// <summary>
        /// Returns a read-only ReadOnlyCollection(Of T) wrapper for the current collection.
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return _List.AsReadOnly();
        }

        /// <summary>
        /// Copies the entire ExtendedList(Of T) to a compatible one-dimensional array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array"></param>
        public void CopyTo(T[] array)
        {
            _List.CopyTo(array);
        }

        /// <summary>
        /// Copies the entire ExtendedList(Of T) to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _List.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copies a range of elements from the ExtendedList(Of T) to a compatible one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="count"></param>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            _List.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary>
        /// Converts the elements in the current ExtendedList(Of T) to another type, and returns a list containing the converted elements.
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public EventList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            return new EventList<TOutput>(_List.ConvertAll<TOutput>(converter));
        }

        /// <summary>
        /// Copies the elements of the ExtendedList(Of T) to a new array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return _List.ToArray();
        }

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source ExtendedList(Of T).
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public EventList<T> GetRange(int index, int count)
        {
            return new EventList<T>(_List.GetRange(index, count));
        }
        #endregion

        #region "Compare"
        /// <summary>
        /// Determines whether every element in the ExtendedList(Of T) matches the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool TrueForAll(Predicate<T> match)
        {
            return _List.TrueForAll(match);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.(Inherited from Object.)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return _List.Equals(obj);
        }
        #endregion

        #region "Widening"
        public static implicit operator EventList<T>(List<T> list)
        {
            return new EventList<T>(list);
        }

        public static implicit operator List<T>(EventList<T> list)
        {
            return list._List;
        }
        #endregion

        #region "Enumerate"
        /// <summary>
        /// Performs the specified action on each element of the ExtendedList(Of T).
        /// </summary>
        /// <param name="action"></param>
        public void ForEach(Action<T> action)
        {
            _List.ForEach(action);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the ExtendedList(Of T).
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _List.GetEnumerator();
        }

        protected IEnumerator GetObjectEnumerator()
        {
            return _List.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetObjectEnumerator();
        }
        #endregion

        #region "Search"
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the entire ExtendedList(Of T).
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            return _List.IndexOf(item);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the ExtendedList(Of T) that extends from the specified index to the last element.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int IndexOf(T item, int index)
        {
            return _List.IndexOf(item, index);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the ExtendedList(Of T) that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int IndexOf(T item, int index, int count)
        {
            return _List.IndexOf(item, index, count);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the entire ExtendedList(Of T).
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int LastIndexOf(T item)
        {
            return _List.LastIndexOf(item);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the ExtendedList(Of T) that extends from the first element to the specified index.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int LastIndexOf(T item, int index)
        {
            return _List.LastIndexOf(item, index);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the ExtendedList(Of T) that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int LastIndexOf(T item, int index, int count)
        {
            return _List.LastIndexOf(item, index, count);
        }

        /// <summary>
        /// Determines whether an element is in the ExtendedList(Of T).
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            return _List.Contains(item);
        }

        /// <summary>
        /// Searches the entire sorted ExtendedList(Of T) for an element using the default comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int BinarySearch(T item)
        {
            return _List.BinarySearch(item);
        }

        /// <summary>
        /// Searches the entire sorted ExtendedList(Of T) for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return _List.BinarySearch(item, comparer);
        }

        /// <summary>
        /// Searches a range of elements in the sorted ExtendedList(Of T) for an element using the specified comparer and returns the zero-based index of the element.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="item"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return _List.BinarySearch(index, count, item, comparer);
        }

        /// <summary>
        /// Determines whether the ExtendedList(Of T) contains elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool Exists(Predicate<T> match)
        {
            return _List.Exists(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire ExtendedList(Of T).
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public T Find(Predicate<T> match)
        {
            return _List.Find(match);
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public List<T> FindAll(Predicate<T> match)
        {
            return _List.FindAll(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the ExtendedList(Of T) that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            return _List.FindIndex(startIndex, count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the ExtendedList(Of T) that extends from the specified index to the last element.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return _List.FindIndex(startIndex, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire ExtendedList(Of T).
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public int FindIndex(Predicate<T> match)
        {
            return _List.FindIndex(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire ExtendedList(Of T).
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public T FindLast(Predicate<T> match)
        {
            return _List.FindLast(match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the ExtendedList(Of T) that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            return _List.FindLastIndex(startIndex, count, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the ExtendedList(Of T) that extends from the first element to the specified index.
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return _List.FindLastIndex(startIndex, match);
        }

        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire ExtendedList(Of T).
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public int FindLastIndex(Predicate<T> match)
        {
            return _List.FindLastIndex(match);
        }
        #endregion

        #region "Edit"
        /// <summary>
        /// Sets the capacity to the actual number of elements in the ExtendedList(Of T), if that number is less than a threshold value.
        /// </summary>
        public void TrimExcess()
        {
            _List.TrimExcess();
        }

        /// <summary>
        /// Adds an object to the end of the ExtendedList(Of T).
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(T item)
        {
            if (item == null)
                return;
            // Adding event
            EventListEventArgs<T> e = new EventListEventArgs<T>(item);
            OnAdding(this, e);
            if (e.Cancel)
                return;
            // Add
            _List.Add(item);
            // Added event
            OnAdded(this, e);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the ExtendedList(Of T).
        /// </summary>
        /// <param name="items"></param>
        public virtual void AddRange(IEnumerable<T> items)
        {
            if (items == null || !items.Any())
                return;
            EventList<T> itemList = items.ToList();
            // Adding event
            EventListEventArgs<T> e = new EventListEventArgs<T>(itemList);
            OnAdding(this, e);
            if (e.Cancel)
                return;
            // AddRange
            _List.AddRange(itemList);
            // Added event
            OnAdded(this, e);
        }

        /// <summary>
        /// Removes all elements from the ExtendedList(Of T).
        /// </summary>
        public virtual void Clear()
        {
            // Clearing event
            EventListEventArgs<T> e = new EventListEventArgs<T>();
            OnClearing(this, e);
            if (e.Cancel)
                return;
            // Clear
            _List.Clear();
            // Cleared event
            OnCleared(this, EventArgs.Empty);
        }

        /// <summary>
        /// Inserts an element into the ExtendedList(Of T) at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public virtual void Insert(int index, T item)
        {
            if (item == null)
                return;
            // Adding event
            EventListEventArgs<T> e = new EventListEventArgs<T>(item, index);
            OnAdding(this, e);
            if (e.Cancel)
                return;
            // Insert
            _List.Insert(index, item);
            // Added event
            OnAdded(this, e);
        }

        /// <summary>
        /// Inserts the elements of a collection into the ExtendedList(Of T) at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="items"></param>
        public virtual void InsertRange(int index, IEnumerable<T> items)
        {
            if (items == null || !items.Any())
                return;
            EventList<T> itemList = items.ToList();
            // Adding event
            EventListEventArgs<T> e = new EventListEventArgs<T>(itemList, index);
            OnAdding(this, e);
            if (e.Cancel)
                return;
            // AddRange
            _List.InsertRange(index, itemList);
            // Added event
            OnAdded(this, e);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the ExtendedList(Of T).
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual bool Remove(T item)
        {
            bool functionReturnValue = false;
            if (item == null || IndexOf(item) < 0)
                return false;
            // Removing event
            EventListEventArgs<T> e = new EventListEventArgs<T>(item);
            OnRemoving(this, e);
            if (e.Cancel)
                return false;
            // Remove
            functionReturnValue = _List.Remove(item);
            // Removed event
            OnRemoved(this, e);
            return functionReturnValue;
        }

        /// <summary>
        /// Removes a range of elements from the ExtendedList(Of T).
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public virtual void RemoveRange(int index, int count)
        {
            if (count <= 0)
                return;
            EventList<T> itemList = _List.GetRange(index, count);
            // Removing event
            EventListEventArgs<T> e = new EventListEventArgs<T>(itemList);
            OnRemoving(this, e);
            if (e.Cancel)
                return;
            // RemoveRange
            _List.RemoveRange(index, count);
            // Removed event
            OnRemoved(this, e);
        }

        //Public Shadows Sub RemoveRange(items As IEnumerable(Of T)) ' 
        //If items Is Nothing OrElse items.Count = 0 Then Return
        //Dim itemList As ICollection(Of T) = items.ToList()
        //' Removing event
        //Dim e As New ExtendedListEventArgs(Of T)(itemList)
        //RaiseEvent Removing(Me, e)
        //If e.Cancel Then Return
        //' RemoveRange
        //MyBase.RemoveRange(itemList)
        //' Removed event
        //RaiseEvent Removed(Me, e)
        //End Sub

        /// <summary>
        /// Removes the element at the specified index of the ExtendedList(Of T).
        /// </summary>
        /// <param name="index"></param>
        public virtual void RemoveAt(int index)
        {
            T item = this[index];
            // Removing event
            EventListEventArgs<T> e = new EventListEventArgs<T>(item);
            OnRemoving(this, e);
            if (e.Cancel)
                return;
            // RemoveAt
            _List.RemoveAt(index);
            // Removed event
            OnRemoved(this, e);
        }

        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public virtual int RemoveAll(Predicate<T> match)
        {
            EventList<T> items = _List.FindAll(match);
            if (items.Count == 0)
                return 0;
            // Removing event
            EventListEventArgs<T> e = new EventListEventArgs<T>(items);
            OnRemoving(this, e);
            if (e.Cancel)
                return 0;
            // RemoveAll (modified)
            var result = _List.RemoveAll(it => items.Contains(it));
            // Removed event
            OnRemoved(this, e);
            return result;
        }
        #endregion

        #region "Sort"
        /// <summary>
        /// Reverses the order of the elements in the entire ExtendedList(Of T).
        /// </summary>
        public void Reverse()
        {
            _List.Reverse();
        }

        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public void Reverse(int index, int count)
        {
            _List.Reverse(index, count);
        }

        /// <summary>
        /// Sorts the elements in the entire ExtendedList(Of T) using the default comparer.
        /// </summary>
        public void Sort()
        {
            _List.Sort();
        }

        /// <summary>
        /// Sorts the elements in the entire ExtendedList(Of T) using the specified System.Comparison(Of T).
        /// </summary>
        /// <param name="comparison"></param>
        public void Sort(Comparison<T> comparison)
        {
            _List.Sort(comparison);
        }

        /// <summary>
        /// Sorts the elements in the entire ExtendedList(Of T) using the specified comparer.
        /// </summary>
        /// <param name="comparer"></param>
        public void Sort(IComparer<T> comparer)
        {
            _List.Sort(comparer);
        }

        /// <summary>
        /// Sorts the elements in a range of elements in ExtendedList(Of T) using the specified comparer.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="comparer"></param>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            _List.Sort(index, count, comparer);
        }
        #endregion

        /// <summary>
        /// Serves as the default hash function. (Inherited from Object.)
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _List.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.(Inherited from Object.)
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _List.ToString();
        }

    }


    public class EventListEventArgs<T> : EventArgs
    {

        public EventListEventArgs()
        {
        }

        public EventListEventArgs(List<T> items, int index = -1)
        {
            this.Index = index;
            this.Items = items;
        }

        public EventListEventArgs(T item, int index = -1)
        {
            this.Index = index;
            Items = new List<T>();
            Items.Add(item);
        }

        #region "Fields & Properties"
        public List<T> Items { get; }
        public int Index { get; }
        public bool Cancel { get; set; }
        #endregion

    }
}
