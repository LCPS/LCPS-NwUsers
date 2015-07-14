using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anvil.v2015.v001.Domain.Entities
{

    public class AnvilEntity
    {
        #region Fields

        private Object _source;

        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the entity object
        /// </summary>
        /// <param name="source"></param>
        public AnvilEntity(Object source)
        {
            _source = source;
        }

        #endregion

        #region Properties

        public String[] Fields { get; set; }
        public String[] IgnoreFields { get; set; }

        #endregion

        #region Copy

        public Object CopyTo(Object target)
        {
            if (IgnoreFields == null) { IgnoreFields = new String[] { }; }

            if (Fields == null)
            {
                List<String> names = new List<String>();

                foreach (PropertyInfo p in (from PropertyInfo x in _source.GetType().GetProperties()
                                            where (!IgnoreFields.Contains(x.Name))
                                            & x.CanRead & x.CanWrite 
                                            orderby x.Name
                                            select x))
                {
                    names.Add(p.Name);
                }

                Fields = names.ToArray();
            }

            Object r;

            foreach (String n in Fields)
            {
                PropertyInfo tp = (from PropertyInfo x in target.GetType().GetProperties()
                                   where x.Name.ToLower() == n.ToLower() &
                                   x.CanRead & x.CanWrite
                                   select x).SingleOrDefault();

                PropertyInfo sp = (from PropertyInfo x in _source.GetType().GetProperties()
                                   where x.Name.ToLower() == n.ToLower() &
                                   x.CanRead & x.CanWrite
                                   select x).SingleOrDefault();

                

                if ((tp != null) & (sp != null))
                {
                        object src = sp.GetValue(_source, null);
                        tp.SetValue(target, src, null);
                }
            }

            r = target;
            return r;
        }

        #endregion

        #region Compare

        public string[] Compare(object target)
        {
            List<string> different = new List<string>();

            if (IgnoreFields == null) { IgnoreFields = new String[] { }; }

            if (Fields == null)
            {
                List<String> names = new List<String>();

                foreach (PropertyInfo p in (from PropertyInfo x in _source.GetType().GetProperties()
                                            where (!IgnoreFields.Contains(x.Name))
                                            & x.CanRead & x.CanWrite
                                            orderby x.Name
                                            select x))
                {
                    names.Add(p.Name);
                }

                Fields = names.ToArray();
            }

            foreach (String n in Fields)
            {
                PropertyInfo tp = (from PropertyInfo x in target.GetType().GetProperties()
                                   where x.Name.ToLower() == n.ToLower() &
                                   x.CanRead & x.CanWrite
                                   select x).SingleOrDefault();

                PropertyInfo sp = (from PropertyInfo x in _source.GetType().GetProperties()
                                   where x.Name.ToLower() == n.ToLower() &
                                   x.CanRead & x.CanWrite
                                   select x).SingleOrDefault();



                if ((tp != null) & (sp != null))
                {
                    object src = sp.GetValue(_source, null);

                    object trg = sp.GetValue(target, null);

                    if (!src.Equals(trg))
                        different.Add(n);
                }
            }

            return different.ToArray();

        }

        #endregion


       

    }


}
