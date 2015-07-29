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
        private List<String> _ignoreFields = new List<string>();
        private List<string> _requiredFields = new List<string>();
        private List<string> _compareFields = new List<string>();

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
        public List<string> IgnoreFields
        {
            get { return _ignoreFields;  }
        }

        public List<string> RequiredFields
        {
            get { return _requiredFields; }
        }

        public List<string> CompareFields
        {
            get { return _compareFields; }
        }

        #endregion

        #region Validate

        public List<string> Validate()
        {
            if (RequiredFields.Count() == 0)
                return new List<string>();

            List<String> emptyFields = new List<String>();

            foreach(string f in RequiredFields)
            {
                PropertyInfo p = _source.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == f.ToLower());
                if( p!= null)
                {
                    object v = p.GetValue(_source, null);
                    if (v == null)
                        emptyFields.Add(f);
                    else
                        if (string.IsNullOrEmpty(v.ToString()))
                            emptyFields.Add(f);
                }
            }

            return emptyFields;
        }

        #endregion

        #region Compare

       

        #endregion

        #region Copy

        public Object CopyTo(Object target)
        {
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
                    try
                    {
                        object src = sp.GetValue(_source, null);
                        tp.SetValue(target, src, null);
                    }
                    catch(Exception ex)
                    {
                        throw new Exception(string.Format("Error copying property {0}", tp.Name), ex);
                    }
                }
            }

            r = target;
            return r;
        }

        #endregion

        #region Compare

        public List<string> Compare(object target)
        {
            List<string> different = new List<string>();

            if (CompareFields.Count == 0)
            {
                string[] ff = (from PropertyInfo x in target.GetType().GetProperties()
                               where x.CanRead & x.CanWrite
                               select x.Name).ToArray();
                CompareFields.AddRange(ff);
            }

            foreach (string f in CompareFields)
            {
                PropertyInfo sp = _source.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == f.ToLower());
                PropertyInfo tp = target.GetType().GetProperties().FirstOrDefault(x => x.Name.ToLower() == f.ToLower());
                if (sp == null)
                    throw new Exception(string.Format("The field {0} was not found in the source object", f));

                if (tp == null)
                    throw new Exception(string.Format("The field {0} was not found in the target object", f));

                object sv = sp.GetValue(_source, null);
                object tv = tp.GetValue(target, null);

               

                if (sv != null & tv != null)
                {

                    try 
                    {
                        if (sv == null & tv != null)
                            different.Add(f);
                        else
                        {
                            if (sv != null & tv == null)
                            {
                                different.Add(f);
                            }
                            else if (sv != null & tv != null)
                            {
                                if (!sv.Equals(tv))
                                    different.Add(f);

                            }
                        } 


                    }
                    catch(Exception ex)
                    {
                        throw new Exception(string.Format("Error comparing field {0}", f), ex);
                    }
                }
            }

            return different;
        }

        /*
        public string[] Compare(object target)
        {
            List<string> different = new List<string>();

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
        */

        #endregion


       

    }


}
