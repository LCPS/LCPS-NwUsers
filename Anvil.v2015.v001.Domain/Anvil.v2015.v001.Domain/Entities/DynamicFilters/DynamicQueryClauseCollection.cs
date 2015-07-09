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
    public class DynamicQueryClauseCollection : IList<DynamicQueryClause>
    {

        DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();

        List<DynamicQueryClause> _list = new List<DynamicQueryClause>();

        string query = "";
        List<object> parms;

        public int IndexOf(DynamicQueryClause item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, DynamicQueryClause item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public DynamicQueryClause this[int index]
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

        public void Add(DynamicQueryClause item)
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

        public bool Contains(DynamicQueryClause item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(DynamicQueryClause[] array, int arrayIndex)
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

        public bool Remove(DynamicQueryClause item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<DynamicQueryClause> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
