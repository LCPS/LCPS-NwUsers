using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Collections.Specialized;

using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;
using System.Threading.Tasks;

using Anvil.v2015.v001.Domain.Infrastructure;

namespace Anvil.v2015.v001.Domain.Entities
{
    public class EntityNotFound : Exception
    {
        public EntityNotFound(string name)
            : base(string.Format("{0} was not found in the database", name))
        {

        }
    }

    public class NotInstantiatedException : Exception
    {
        public NotInstantiatedException(string actionName, string name)
            : base(string.Format("{0} can not be performed until {1} has been instantiated", actionName, name))
        {

        }
    }

    public class ApplicationUserRoleValidator : IDictionary<ApplicationUserCandidate, ApplicationRoleCollection>
    {
        #region Fields

        private Dictionary<ApplicationUserCandidate, ApplicationRoleCollection> _dic = new Dictionary<ApplicationUserCandidate, ApplicationRoleCollection>();
        private ApplicationDbContext _context;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        #endregion

        #region Constructors

            
        public ApplicationUserRoleValidator(ApplicationDbContext context, ApplicationUserManager userManer, ApplicationRoleManager roleManager)
        {
            _context = context;
            _userManager = userManer;
            _roleManager = roleManager;
        }

        #endregion


        #region Custom Adds

        public void Add(ApplicationUserCandidate c, ApplicationRole r)
        {
            try
            {
                if (!this.ContainsKey(c.UserName))
                {
                    Add(c, new ApplicationRoleCollection());
                }


                int count = this[c].Where(x => x.Name.ToLower() == r.Name.ToLower()).Count();
                if (count == 0)
                    this[c].Add(r);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not add user role pair to validation list", ex);
            }

        }

        #endregion

        #region Dictionary

        public void Add(ApplicationUserCandidate key, ApplicationRoleCollection value)
        {
            _dic.Add(key, value);
        }

        public bool ContainsKey(string userName)
        {
            int count = this.Keys.Where(x => x.UserName.Equals(userName)).Count();
            return (count > 0);
        }

        public bool ContainsKey(ApplicationUserCandidate key)
        {
            return _dic.ContainsKey(key);
        }

        public ICollection<ApplicationUserCandidate> Keys
        {
            get { return _dic.Keys; }
        }

        public bool Remove(ApplicationUserCandidate key)
        {
            return _dic.Remove(key);
        }

        public bool TryGetValue(ApplicationUserCandidate key, out ApplicationRoleCollection value)
        {
            return _dic.TryGetValue(key, out value);
        }

        public ICollection<ApplicationRoleCollection> Values
        {
            get { return _dic.Values ; }
        }

        public ApplicationRoleCollection this[ApplicationUserCandidate key]
        {
            get
            {
                return _dic[key];
            }
            set
            {
                _dic[key] = value;
            }
        }

        public void Add(KeyValuePair<ApplicationUserCandidate, ApplicationRoleCollection> item)
        {
            _dic.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dic.Clear();
        }

        public bool Contains(KeyValuePair<ApplicationUserCandidate, ApplicationRoleCollection> item)
        {
            return _dic.Contains(item);
        }

        public void CopyTo(KeyValuePair<ApplicationUserCandidate, ApplicationRoleCollection>[] array, int arrayIndex)
        {
            _dic.ToArray().CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _dic.Count(); }
        }

        public bool IsReadOnly
        {
            get { return _dic.ToArray().IsReadOnly; }
        }

        public bool Remove(KeyValuePair<ApplicationUserCandidate, ApplicationRoleCollection> item)
        {
            return _dic.Remove(item.Key);
        }

        public IEnumerator<KeyValuePair<ApplicationUserCandidate, ApplicationRoleCollection>> GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dic.GetEnumerator();
        }

        #endregion

        #region Validation

        public void Validate()
        {
            if (_context == null)
                throw new NotInstantiatedException("Validation", "the database connection");

            if (_userManager == null)
                throw new NotInstantiatedException("Validation", "the user manager");

            if (_roleManager == null)
                throw new NotInstantiatedException("Validation", "the role manager");

            foreach(ApplicationUserCandidate u in this.Keys)
            {
                if (u.Birthdate == null)
                    u.Birthdate = DateTime.MaxValue;

                if(u.Birthdate == DateTime.MinValue)
                    u.Birthdate = DateTime.MaxValue;

                ApplicationUser dbU = _context.Users.FirstOrDefault(x => x.UserName.ToLower() == u.UserName.ToLower());
                
                if (dbU == null)
                {
                    dbU = new ApplicationUser();
                    AnvilEntity e = new AnvilEntity(u);
                    e.Fields = new string[] { "UserName", "GivenName", "SurName", "Birthdate", "Autobiography", "Email", "CompanyId", "Psuedonym" };
                    e.CopyTo(dbU);

                    if (string.IsNullOrEmpty(dbU.Email))
                        throw new Exception("Email address is required for user");

                    if (string.IsNullOrEmpty(u.Password))
                        throw new Exception("Password is required for user");

                    
                        

                    
                    dbU.Id = Guid.NewGuid().ToString();

                    dbU.AccountStatus = ApplicationUser.AccountStatusQualifier.Active;

                    _userManager.Create(dbU, u.Password);
                }

                dbU = _userManager.FindByName(u.UserName);

                foreach (ApplicationRole r in this[u])
                {
                    ApplicationRole dbR = _roleManager.FindByName(r.Name);
                    if (dbR == null)
                    {
                        r.Id = Guid.NewGuid().ToString();
                        _roleManager.Create(r);
                    }

                    if (!_userManager.IsInRole(dbU.Id, r.Name))
                    {
                        try
                        {
                            _userManager.AddToRole(dbU.Id, r.Name);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Could not add {0} to {1}", u.UserName, r.Name), ex);
                        }
                    }

                }
            }
        }

        #endregion
    }
}
