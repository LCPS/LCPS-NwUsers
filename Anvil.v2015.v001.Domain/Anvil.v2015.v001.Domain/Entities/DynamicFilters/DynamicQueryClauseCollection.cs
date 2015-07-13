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

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    public class DynamicQueryClauseCollection : IList<IDynamicQueryClause>
    {

        DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();

        List<IDynamicQueryClause> _list = new List<IDynamicQueryClause>();

        string query = "";
        List<object> parms;

        public int IndexOf(IDynamicQueryClause item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, IDynamicQueryClause item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public IDynamicQueryClause this[int index]
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

        public void Add(IDynamicQueryClause item)
        {
            _list.Add(item);
            if (parms.Count() == 0)
            {
                query = item.FieldName + " " + lib.GetOperator(item.Operator) + " @" + parms.Count().ToString();
                parms.Add(item.Value);
            }
            else
            {
                query = item.Conjunction.ToString() + " " +  item.FieldName + " " + lib.GetOperator(item.Operator) + " @" + parms.Count().ToString();
                parms.Add(item.Value);
            }
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(IDynamicQueryClause item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(IDynamicQueryClause[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _list.Count(); }
        }

        public bool IsReadOnly
        {
            get { return _list.ToArray().IsReadOnly; }
        }

        public bool Remove(IDynamicQueryClause item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<IDynamicQueryClause> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
