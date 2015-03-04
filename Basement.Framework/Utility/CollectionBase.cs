using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace Basement.Framework.Utility
{
    /// <summary>Provides the abstract base class for a strongly typed collection.</summary>
    /// <filterpriority>2</filterpriority>
    [Serializable, ComVisible(true)]
    public class CollectionBase<T> : IList<T>, IEnumerable<T>, ICollection<T>
    {
        #region = Constructor =
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.CollectionBase"></see> class with the default initial capacity.</summary>
        public CollectionBase()
        {
            this.InnerList = new List<T>();
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.CollectionBase"></see> class with the specified capacity.</summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public CollectionBase(int capacity)
        {
            this.InnerList = new List<T>(capacity);

        }
        #endregion

        #region = Prop =
        #region = Capacity =
        /// <summary>Gets or sets the number of elements that the <see cref="T:System.Collections.CollectionBase"></see> can contain.</summary>
        /// <returns>The number of elements that the <see cref="T:System.Collections.CollectionBase"></see> can contain.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><see cref="P:System.Collections.CollectionBase.Capacity"></see> is set to a value that is less than <see cref="P:System.Collections.CollectionBase.Count"></see>.</exception>
        /// <filterpriority>2</filterpriority>
        [ComVisible(false)]
        public int Capacity
        {
            get
            {
                return this.InnerList.Capacity;
            }
            set
            {
                this.InnerList.Capacity = value;
            }
        }
        #endregion

        #region = Count =
        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.CollectionBase"></see> instance.Retrieving the value of this property is an O(1) operation.</returns>
        /// <filterpriority>2</filterpriority>
        public int Count
        {
            get
            {
                if (this.InnerList != null)
                {
                    return this.InnerList.Count;
                }
                return 0;
            }
        }
        #endregion

        #region = InnerList =
        private List<T> _innerlist = null;
        /// <summary>Gets an <see cref="T:System.Collections.ArrayList"></see> containing the list of elements in the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <returns>An <see cref="T:System.Collections.ArrayList"></see> representing the <see cref="T:System.Collections.CollectionBase"></see> instance itself.Retrieving the value of this property is an O(1) operation.</returns>
        protected List<T> InnerList
        {
            get { return _innerlist; }
            set { _innerlist = value; }
        }
        #endregion

        #region = List =
        /// <summary>Gets an <see cref="T:System.Collections.IList"></see> containing the list of elements in the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <returns>An <see cref="T:System.Collections.IList"></see> representing the <see cref="T:System.Collections.CollectionBase"></see> instance itself.</returns>
        protected IList<T> List
        {
            get
            {
                return this;
            }
        }

        #endregion
        #endregion

        #region = Public Method =

        #region = Add =
        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public virtual void Add(T item)
        {
            this.InnerList.Add(item);
        }
        #endregion

        #region = Clear =
        /// <summary>Removes all objects from the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <filterpriority>2</filterpriority>
        public virtual void Clear()
        {
            this.OnClear();
            this.InnerList.Clear();
            this.OnClearComplete();
        }
        #endregion

        #region = RemoveAt =
        /// <summary>Removes the element at the specified index of the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is equal to or greater than <see cref="P:System.Collections.CollectionBase.Count"></see>.</exception>
        /// <filterpriority>2</filterpriority>
        public virtual void RemoveAt(int index)
        {
            this.InnerList.RemoveAt(index);
        }
        #endregion

        #region = Remove =
        public virtual bool Remove(T item)
        {
            return this.InnerList.Remove(item);
        }
        #endregion

        #region = GetEnumerator =
        /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"></see> for the <see cref="T:System.Collections.CollectionBase"></see> instance.</returns>
        /// <filterpriority>2</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }
        #endregion

        #region = Sort =
        public virtual void Sort()
        {
            this.InnerList.Sort();
        }

        public virtual void Sort(Comparison<T> comparison)
        {
            this.InnerList.Sort(comparison);
        }

        public virtual void Sort(IComparer<T> comparer)
        {
            this.InnerList.Sort(comparer);
        }
        #endregion

        #region = Contains =
        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
        /// <returns>
        /// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return this.List.Contains(item);
        }
        #endregion

        #endregion

        #region  = Protected Methods =

        #region = OnClear =
        /// <summary>Performs additional custom processes when clearing the contents of the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        protected virtual void OnClear()
        {
        }
        #endregion

        #region = OnClearComplete =
        /// <summary>Performs additional custom processes after clearing the contents of the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        protected virtual void OnClearComplete()
        {
        }
        #endregion

        #region = OnInsert =
        /// <summary>Performs additional custom processes before inserting a new element into the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <param name="value">The new value of the element at index.</param>
        /// <param name="index">The zero-based index at which to insert value.</param>
        protected virtual void OnInsert(int index, T value)
        {
        }
        #endregion

        #region = OnInsertComplete =
        /// <summary>Performs additional custom processes after inserting a new element into the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <param name="value">The new value of the element at index.</param>
        /// <param name="index">The zero-based index at which to insert value.</param>
        protected virtual void OnInsertComplete(int index, T value)
        {
        }
        #endregion

        #region = OnRemove =
        /// <summary>Performs additional custom processes when removing an element from the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <param name="value">The value of the element to remove from index.</param>
        /// <param name="index">The zero-based index at which value can be found.</param>
        protected virtual void OnRemove(int index, T value)
        {
        }
        #endregion

        #region = OnRemoveComplete =
        /// <summary>Performs additional custom processes after removing an element from the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <param name="value">The value of the element to remove from index.</param>
        /// <param name="index">The zero-based index at which value can be found.</param>
        protected virtual void OnRemoveComplete(int index, T value)
        {
        }
        #endregion

        #region = OnSet =
        /// <summary>Performs additional custom processes before setting a value in the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <param name="oldValue">The value to replace with newValue.</param>
        /// <param name="newValue">The new value of the element at index.</param>
        /// <param name="index">The zero-based index at which oldValue can be found.</param>
        protected virtual void OnSet(int index, T oldValue, T newValue)
        {
        }
        #endregion

        #region = OnSetComplete =
        /// <summary>Performs additional custom processes after setting a value in the <see cref="T:System.Collections.CollectionBase"></see> instance.</summary>
        /// <param name="oldValue">The value to replace with newValue.</param>
        /// <param name="newValue">The new value of the element at index.</param>
        /// <param name="index">The zero-based index at which oldValue can be found.</param>
        protected virtual void OnSetComplete(int index, T oldValue, T newValue)
        {
        }
        #endregion

        #region = OnValidate =
        /// <summary>Performs additional custom processes when validating a value.</summary>
        /// <param name="value">The object to validate.</param>
        protected virtual void OnValidate(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
        }
        #endregion
        #endregion

        #region = Index =
        public T this[int index]
        {
            get
            {
                if ((index < 0) || (index >= this.InnerList.Count))
                {
                    throw new ArgumentOutOfRangeException("index", "ArgumentOutOfRange_Index");
                }

                return this.InnerList[index];
            }
            set
            {
                if ((index < 0) || (index >= this.InnerList.Count))
                {
                    throw new ArgumentOutOfRangeException("index", "ArgumentOutOfRange_Index");
                }
                //this.OnValidate(value);
                //object obj1 = this.InnerList[index];
                //this.OnSet(index, obj1, value);
                this.InnerList[index] = value;
                //try
                //{
                //    this.OnSetComplete(index, obj1, value);
                //}
                //catch
                //{
                //    this.InnerList[index] = obj1;
                //    throw;
                //}
            }
        }
        #endregion

        #region Interface
        #region IList<T> Members
        #region = IList.IndexOf =
        int IList<T>.IndexOf(T item)
        {
            return this.InnerList.IndexOf(item);
        }
        #endregion

        #region = IList.Insert =
        void IList<T>.Insert(int index, T item)
        {
            if ((index < 0) || (index > this.InnerList.Count))
            {
                throw new ArgumentOutOfRangeException("index", "ArgumentOutOfRange_Index");
            }

            this.OnValidate(item);
            this.OnInsert(index, item);
            this.InnerList.Insert(index, item);

            try
            {
                this.OnInsertComplete(index, item);
            }
            catch
            {
                this.InnerList.RemoveAt(index);
                throw;
            }
        }
        #endregion

        #region = IList<T>.RemoveAt =
        void IList<T>.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }
        #endregion

        #region = IList<T>.this[int index] =
        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value;
            }
        }
        #endregion

        #endregion

        #region ICollection<T> Members

        #region = Add =
        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }
        #endregion

        #region = Clear =
        void ICollection<T>.Clear()
        {
            this.Clear();
        }
        #endregion

        #region = Contains =
        bool ICollection<T>.Contains(T item)
        {
            return this.InnerList.Contains(item);
        }
        #endregion

        #region = CopyTo =
        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            this.InnerList.CopyTo(array, arrayIndex);
        }
        #endregion

        #region = Count =
        int ICollection<T>.Count
        {
            get { return this.Count; }
        }
        #endregion

        #region = IsReadOnly =
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region = Remove =
        bool ICollection<T>.Remove(T item)
        {
            return this.Remove(item);
        }
        #endregion

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }

        #endregion
        #endregion
    }
}
