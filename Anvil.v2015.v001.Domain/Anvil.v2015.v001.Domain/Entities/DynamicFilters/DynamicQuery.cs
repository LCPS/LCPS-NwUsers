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

using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Anvil.v2015.v001.Domain.Entities.DynamicFilters
{
    public class DynamicQuery : IList<DynamicQueryClause>
    {
        #region Fields

        private List<DynamicQueryClause> _list = new List<DynamicQueryClause>();
        private List<object> _parms = new List<object>();
        private List<string> _elements = new List<string>();
        private DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();

        #endregion


        #region Properties

        public Guid FilterId { get; set; }

        public List<object> Parms
        {
            get { return _parms; }
        }

        public List<String> Elements
        {
            get { return _elements; }
        }

        #endregion

        #region List

        public void Add(IDynamicFilterClause item)
        {
            DynamicQueryClause c = new DynamicQueryClause();
            AnvilEntity e = new AnvilEntity(item);
            e.IgnoreFields.Add("Item");
            e.CopyTo(c);
            c.AddRange(item.ToArray());

            Add(c);
        }

        public void Add(DynamicQueryClause item)
        {


            _list.Add(item);
        }

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

        #endregion

        #region Conversions

        public override string ToString()
        {
            string q = "";

            foreach (DynamicQueryClause c in _list)
            {


                if (_list.IndexOf(c) > 0)
                    q = c.ClauseConjunction.ToString() + " ";

                q += "(" + c.ToString() + ")";

            }

            return q;
        }

        public virtual DynamicQueryStatement ToDynamicQueryStatement()
        {

            foreach (DynamicQueryClause c in _list)
            {
                string q = "(";

                if (_list.IndexOf(c) > 0)
                    q = c.ClauseConjunction.ToString() + " (";

                foreach (DynamicQueryClauseField f in c)
                {
                    if (f.Include)
                    {
                        if (c.IndexOf(f) > 0 & f.Conjunction != DynamicQueryConjunctions.None)
                            q += f.Conjunction.ToString() + " ";

                        if (f.Operator == DynamicQueryOperators.Contains)
                            q += f.FieldName + ".Contains(@" + _parms.Count().ToString() + ")";
                        else
                            q += f.FieldName + " " + lib.GetOperator(f.Operator) + " @" + _parms.Count().ToString() + " ";

                        _parms.Add(f.Value);
                    }
                }

                q += ")";

                _elements.Add(q);
            }

            return new DynamicQueryStatement()
            {
                Parms = _parms.ToArray(),
                Query = string.Join("", _elements.ToArray())
            };

        }

        #endregion
    }
}