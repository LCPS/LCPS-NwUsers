using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anvil.v2015.v001.Domain.Entities
{
    public class ApplicationRoleCollection : IList<ApplicationRole>
    {
        #region Fields

        private List<ApplicationRole> _list = new List<ApplicationRole>();

        #endregion

        #region Custom Adds

        public void AddRange(IEnumerable<ApplicationRole> items)
        {
            _list.AddRange(items);
        }

        #endregion

        #region List

        public int IndexOf(ApplicationRole item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, ApplicationRole item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public ApplicationRole this[int index]
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

        public void Add(string roleName, string description)
        {
            ApplicationRole r = new ApplicationRole()
            {
                Name = roleName,
                Description = description,
                Id = Guid.NewGuid().ToString()
            };

            Add(r);
        }

        public void Add(ApplicationRole item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(string roleName, out ApplicationRole r)
        {
            r = this.FirstOrDefault(x => x.Name.ToLower() == roleName.ToLower());
            return (r != null);
        }

        public bool Contains(ApplicationRole item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(ApplicationRole[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return _list.Count();
            }
        }

        public bool IsReadOnly
        {
            get { return _list.ToArray().IsReadOnly; }
        }

        public bool Remove(ApplicationRole item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<ApplicationRole> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}
