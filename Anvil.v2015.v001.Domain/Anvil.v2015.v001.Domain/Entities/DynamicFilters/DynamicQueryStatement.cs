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
    public class DynamicQueryStatement : IList<DynamicQueryClause>
    {
        #region Fields

        private List<DynamicQueryClause> _list = new List<DynamicQueryClause>();

        private DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();

        #endregion

        public DynamicQuery ToDynamicQuery()
        {
            List<string> _elements = new List<string>();
            List<object> _parms = new List<object>();
            
            foreach(DynamicQueryClause c in _list)
            {
                if(c.Include)
                {
                    string q = "";

                    if(_list.IndexOf(c) != 0)
                        q += c.Conjunction.ToString() + " ";

                    q += c.FieldName + lib.GetOperator(c.Operator) + "@" + _parms.Count().ToString();

                    _elements.Add(q);

                    _parms.Add(c.Value);
                }
            }

            DynamicQuery dq = new DynamicQuery()
                {
                    Query = string.Join(" ", _elements.ToArray()),
                    Parms = _parms.ToArray()
                };

            return dq;
        }


        #region List


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
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
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

#endregion
    }
}
