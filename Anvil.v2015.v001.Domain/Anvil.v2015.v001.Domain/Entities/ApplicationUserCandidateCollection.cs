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
    public class ApplicationUserCandidateCollection : IList<ApplicationUserCandidate>
    {

        #region Fields

        private List<ApplicationUserCandidate> _list = new List<ApplicationUserCandidate>();

        #endregion

        #region Customized Adds

        public void Add(string userName, string password, string emailAddress)
        {
            Add(new ApplicationUserCandidate(userName, emailAddress, password));
        }

        public void Add(string userName, string password, string emailAddress, string companyId)
        {
            Add(new ApplicationUserCandidate(userName, emailAddress, password)
            {
                CompanyId = companyId
            });
        }

        public void Add(string userName, string password, string emailAddress, string companyId, string givenName, string surName)
        {
            Add(new ApplicationUserCandidate(userName, emailAddress, password)
            {
                CompanyId = companyId,
                GivenName = givenName,
                SurName = surName
            });
        }

        public void Add(string userName, string password, string emailAddress, string companyId, string givenName, string surName, string psuedonym)
        {
            Add(new ApplicationUserCandidate(userName, emailAddress, password)
            {
                CompanyId = companyId,
                GivenName = givenName,
                SurName = surName,
                Psuedonym = psuedonym
            });
        }

        public void Add(string userName, string password, string emailAddress, string companyId, string givenName, string surName, string psuedonym, string autobiography)
        {
            Add(new ApplicationUserCandidate(userName, emailAddress, password)
            {
                CompanyId = companyId,
                GivenName = givenName,
                SurName = surName,
                Psuedonym = psuedonym,
                Autobiography = autobiography

            });
        }

        #endregion

        #region List

        public int IndexOf(ApplicationUserCandidate item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, ApplicationUserCandidate item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public ApplicationUserCandidate this[int index]
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

        public void Add(ApplicationUserCandidate item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(ApplicationUserCandidate item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(ApplicationUserCandidate[] array, int arrayIndex)
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

        public bool Remove(ApplicationUserCandidate item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<ApplicationUserCandidate> GetEnumerator()
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
