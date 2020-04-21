using System;
using System.Collections;
using System.Collections.Generic;

namespace Sverto.General.Containers
{
    public class ArrayBuffer<T> : IEnumerable<T>
    {

        public ArrayBuffer(int bufferSize)
        {
            ValidateBufferSize(bufferSize);
            _Items = new T[bufferSize];
        }

        #region "Fields"
        private int _NextIndex;
        private T[] _Items;
        public T this[int index]
        {
            get
            {
                if (_FilledCount >= _Items.Length)
                {
                    return _Items[(index + _NextIndex) % (_Items.Length)];
                }
                else
                {
                    return _Items[index];
                }
            }
        }

        private int _FilledCount;
        public int Length
        {
            get { return _FilledCount; }
        }

        public int Capacity
        {
            get { return _Items.Length; }
        }
        #endregion

        #region "Buffer Methods"
        public void Add(T item)
        {
            _Items[_NextIndex] = item;
            if (_FilledCount < _Items.Length)
                _FilledCount += 1;
            if (_NextIndex >= _Items.Length - 1)
            {
                _NextIndex = 0;
            }
            else
            {
                _NextIndex += 1;
            }
        }

        public void Clear()
        {
            _Items = new T[_Items.Length];
            _NextIndex = 0;
            _FilledCount = 0;
        }

        public void SetBufferSize(int newBufferSize)
        {
            // Resize the array
            ValidateBufferSize(newBufferSize);

            if (_FilledCount == 0)
            {
                _Items = new T[newBufferSize];

            }
            else if (newBufferSize < _Items.Length)
            {
                // Reformat so the newest message is on the new last index
                int startIndex = _Items.Length - newBufferSize;
                for (int i = newBufferSize - 1; i >= 0; i += -1)
                {
                    _Items[i] = this[i + startIndex];
                }
                _NextIndex = 0;
                if (_FilledCount >= _Items.Length)
                    _FilledCount = newBufferSize;
                Array.Resize(ref _Items, newBufferSize);

            }
            else if (newBufferSize > _Items.Length)
            {
                T[] newArray = new T[newBufferSize];
                Array.Copy(_Items, _NextIndex, newArray, 0, _Items.Length - _NextIndex);
                Array.Copy(_Items, 0, newArray, _Items.Length - _NextIndex, _NextIndex);
                _NextIndex = _FilledCount;
                _Items = newArray;
            }
        }
        #endregion

        #region "Validation"
        private void ValidateBufferSize(int size)
        {
            if (size < 1)
                throw new ArgumentException("The bufferSize must more than zero.");
        }
        #endregion

        public IEnumerator<T> GetEnumerator()
        {
            return new ArrayBufferEnumerator<T>(this);
            //Return _Items.AsEnumerable.GetEnumerator()
        }

        private IEnumerator GetObjectEnumerator()
        {
            return GetEnumerator();
            //Return _Items.GetEnumerator()
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetObjectEnumerator();
        }

    }


    public class ArrayBufferEnumerator<T> : IEnumerator, IEnumerator<T>
    {

        public ArrayBufferEnumerator(ArrayBuffer<T> arrayBuffer)
        {
            if (arrayBuffer == null)
                throw new ArgumentNullException();
            _ArrayBuffer = arrayBuffer;
        }

        private ArrayBuffer<T> _ArrayBuffer;
        private int index = -1;

        private T item;
        public T Current
        {
            get { return item; }
        }

        private object ObjectCurrent
        {
            get
            {
                if (item == null)
                    throw new InvalidOperationException();
                return item;
            }
        }
        object IEnumerator.Current
        {
            get { return ObjectCurrent; }
        }

        public void Reset()
        {
            index = -1;
            item = default(T);
        }

        public bool MoveNext()
        {
            index += 1;
            if (index == _ArrayBuffer.Length)
                return false;
            item = _ArrayBuffer[index];
            return true;
        }

        #region "IDisposable Support"
        // To detect redundant calls
        private bool _IsDisposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_IsDisposed)
            {
                if (disposing)
                {
                    _ArrayBuffer = null;
                    item = default(T);
                }
            }
            _IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
