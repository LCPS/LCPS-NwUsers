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
    public abstract class DynamicQueryClause : IDynamicQueryClause
    {
        #region Fields



        private List<string> _elements = new List<string>();

        private List<object> _parms = new List<object>();

        private List<DynamicQueryClauseField> _list = new List<DynamicQueryClauseField>();

        private DynamicQueryOperatorLibrary _lib = new DynamicQueryOperatorLibrary();

        #endregion


        #region Properties

        public Guid FilterId { get; set; }

        public Guid ClauseId { get; set; }

        public List<object> Parms
        {
            get { return _parms; }
        }

        public List<string> Elements
        {
            get { return _elements; }
        }

        public DynamicQueryOperatorLibrary OperatorLibrary
        {
            get { return _lib; }
        }

        public DynamicQueryConjunctions ClauseConjunction { get; set; }

        #endregion

        #region Elements

        public virtual void AddElement(DynamicQueryConjunctions conjunction, string fieldName, DynamicQueryOperators op, object value)
        {
            string element = "";

            if (Parms.Count > 0)
                element = conjunction.ToString();

            if (op == DynamicQueryOperators.Contains)
            {
                element += " " + fieldName + ".Contains(@" + Parms.Count().ToString() + ") ";
            }
            else
            {
                element += " " + fieldName + " " + OperatorLibrary.GetOperator(op) + " @" + Parms.Count().ToString();
            }

            Parms.Add(value);



            Elements.Add(element);

            DynamicQueryClauseField f = new DynamicQueryClauseField()
            {
                Include = true,
                Conjunction = conjunction,
                FieldName = fieldName,
                Operator = op,
                Value = value
            };

            _list.Add(f);
        }

        #endregion

        #region List


        public int IndexOf(DynamicQueryClauseField item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, DynamicQueryClauseField item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public DynamicQueryClauseField Get(string fieldName)
        {
            DynamicQueryClauseField item = _list.FirstOrDefault(x => x.FieldName.ToLower() == fieldName.ToLower());
            return item;
        }

        public DynamicQueryClauseField this[int index]
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

        public void Add(bool include, DynamicQueryConjunctions conjunction,
            string fieldName, DynamicQueryOperators op, object value)
        {
            DynamicQueryClauseField i = new DynamicQueryClauseField()
            {
                Include = include,
                Conjunction = conjunction,
                FieldName = fieldName,
                Operator = op,
                Value = value
            };
            Add(i);
        }

        public void Add(DynamicQueryClauseField item)
        {

            AddElement(item.Conjunction, item.FieldName, item.Operator, item.Value);

            _list.Add(item);
        }

        public void AddRange(DynamicQueryClauseField[] items)
        {
            foreach (DynamicQueryClauseField f in items)
            {
                Add(f);
            }
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(DynamicQueryClauseField item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(DynamicQueryClauseField[] array, int arrayIndex)
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

        public bool Remove(DynamicQueryClauseField item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<DynamicQueryClauseField> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion

        public override string ToString()
        {
            return ToFriendlyString();
        }

        public DynamicQueryStatement ToDynamicQueryStatement()
        {
            Parms.Clear();
            Elements.Clear();
            
            foreach (DynamicQueryClauseField f in _list)
            {
                string q = "";

                if (_list.IndexOf(f) > 0)
                    q += f.Conjunction.ToString() + " ";

                if (f.Operator == DynamicQueryOperators.Contains)
                    q += f.FieldName + ".Contains(@" + Parms.Count().ToString() + ") ";
                else
                    q += f.FieldName + " " + OperatorLibrary.GetOperator(f.Operator) + " @" + Parms.Count().ToString() + " ";

                Elements.Add(q);
                Parms.Add(f.Value);
            }
            

            return new DynamicQueryStatement()
            {
                Parms = this.Parms.ToArray(),
                Query = string.Join(" ", this.Elements.ToArray())
            };
        }

        public abstract string ToFriendlyString();
    }
}
