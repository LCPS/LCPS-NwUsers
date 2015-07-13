#region Using

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#endregion


using LCPS.v2015.v001.NwUsers.Infrastructure;

namespace LCPS.v2015.v001.NwUsers.Filters
{
    public class FilterGroupCollection : IList<FilterGroup>
    {

        #region Fields

        List<FilterGroup> _list = new List<FilterGroup>();
        private LcpsDbContext _db = new LcpsDbContext();

        #endregion

        #region Constructors

        public FilterGroupCollection()
        { }

        public FilterGroupCollection(Guid id)
        {
            _list = _db.FilterGrouops.Where(x => x.AntecedentId.Equals(id)).OrderBy(x => x.Name).ToList();
        }

        #endregion

        #region Properties
        #endregion

        public int IndexOf(FilterGroup item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, FilterGroup item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public FilterGroup this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                _list[index] = value;
            }
        }

        public void Add(FilterGroup item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(FilterGroup item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(FilterGroup[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count(); }
        }

        public bool IsReadOnly
        {
            get { return _list.ToArray().IsReadOnly; }
        }

        public bool Remove(FilterGroup item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<FilterGroup> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
