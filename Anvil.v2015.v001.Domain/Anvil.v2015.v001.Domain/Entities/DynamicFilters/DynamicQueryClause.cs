﻿#region Using

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
    public class DynamicQueryClause : IList<DynamicQueryClauseField>
    {
        #region Fields

        private List<string> _elements = new List<string>();
        
        private List<object> _parms = new List<object>();

        private List<DynamicQueryClauseField> _list = new List<DynamicQueryClauseField>();

        private DynamicQueryOperatorLibrary lib = new DynamicQueryOperatorLibrary();

        #endregion


        #region Properties

        public List<object> Parms
        {
            get { return _parms; }
        }

        public List<string> Elements
        {
            get { return _elements; }
        }

        public DynamicQueryConjunctions ClauseConjunction { get; set; }

        #endregion

        #region Elements

        public void AddElement(DynamicQueryConjunctions conjunction, string fieldName, DynamicQueryOperators op, object value)
        {
            string element = "";

            if(Parms.Count > 0)
                element = conjunction.ToString() ;

            if(op == DynamicQueryOperators.Contains)
            {
                element += " " + fieldName + ".Contains(@" + Parms.Count().ToString() + ") ";
            }
            else
            {
                element += " " + fieldName + " " + lib.GetOperator(op) + " @" + Parms.Count().ToString();
            }

            Parms.Add(value);
            
            

            Elements.Add(element);

            
        }

        #endregion

        public virtual DynamicQueryStatement ToDynamicQuery()
        {
            int count = _list.Where(x => x.Include = true).Count();

            if (count == 0)
                return null;

            DynamicQueryStatement dq = new DynamicQueryStatement()
                {
                    Query = "(" + string.Join(" ", _elements.ToArray()) + ")",
                    Parms = _parms.ToArray()
                };

            return dq;
        }


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
            if(item.Include)
                AddElement(item.Conjunction, item.FieldName, item.Operator, item.Value);

            _list.Add(item);
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


            string q = "(";

            if (ClauseConjunction != DynamicQueryConjunctions.None)
                q = ClauseConjunction.ToString() + " (";

            foreach(DynamicQueryClauseField f in _list)
            {
                if (f.Include)
                {
                    if (_list.IndexOf(f) > 0)
                        if(f.Conjunction != DynamicQueryConjunctions.None)
                            q += f.Conjunction.ToString() + " ";

                    string v = f.Value.ToString();

                    if (f.Operator == DynamicQueryOperators.Contains)
                        q += "[" + f.FieldName + "] Contains \"" + v + "\" ";
                    else
                        q += "[" + f.FieldName + "] " + lib.GetOperator(f.Operator) + " \"" + v + " \" ";
                }
            }

            return q + ")";
        }

        public DynamicQueryStatement ToDynamicQueryStatement()
        {
                        

            foreach (DynamicQueryClauseField f in _list)
            {
                string q = "";

                if (_list.IndexOf(f) > 0)
                    q += f.Conjunction.ToString() + " ";

                if (f.Operator == DynamicQueryOperators.Contains)
                    q += f.FieldName + ".Contains(@" + Parms.Count().ToString() + ")";
                else
                    q += f.FieldName + " " + lib.GetOperator(f.Operator) + " @" + Parms.Count().ToString();

                Parms.Add(f.Value);

                Elements.Add(q);
            }

            return new DynamicQueryStatement()
            {
                Parms = this.Parms.ToArray(),
                Query = string.Join("", this.Elements.ToArray())
            };
        }
    }
}
