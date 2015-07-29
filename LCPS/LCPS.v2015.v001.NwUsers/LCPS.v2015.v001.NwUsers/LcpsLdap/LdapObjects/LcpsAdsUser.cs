using System;
using System.Collections;
using System.Collections.Generic;

using System.IO;
using System.Reflection;

using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;

using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Principal;
using System.Security.AccessControl;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

using Anvil.v2015.v001.Domain.Entities;
using LCPS.v2015.v001.NwUsers.Infrastructure;


namespace LCPS.v2015.v001.NwUsers.LcpsLdap.LdapObjects
{
    public class LcpsAdsUser : LcpsAdsObject
    {

        #region Constants

        const string pwKey = "D89F1BB1-4345-4EFC-B05C-C4B7DCE26664";

        #endregion

        #region Fields


        private string _userName;
        private string _employeeId;
        private string _firstName;
        private string _lastName;
        private string _displayName;
        private string _description;
        private string _employeeType;
        private string _title;
        private string _email;
        private string _location;
        private byte[] _initialPassword;
        private LcpsAdsUserAccountControl _userAccountControl;
        private bool _isActive;
        private string _distinguishedName;
        private string[] _homeFolders = new string[] { };

        #endregion

        #region Constructors

        public LcpsAdsUser()
        {

        }

        public LcpsAdsUser(Guid id)
            : base(id)
        {
        }

        public LcpsAdsUser(DirectoryEntry e)
            : base(e)
        {
        }

        public override void FillFieldMaps()
        {
            AddFieldMap("EmployeeId", "employeeId");
            AddFieldMap("UserName", "samAccountName");
            AddFieldMap("FirstName", "givenName");
            AddFieldMap("LastName", "sn");
            AddFieldMap("Description", "description");
            AddFieldMap("DisplayName", "displayName");
            AddFieldMap("Location", "company");
            AddFieldMap("EmployeeType", "department");
            AddFieldMap("Title", "title");
            AddFieldMap("Email", "mail");
            AddFieldMap("UserAccountControl", "userAccountControl");
            AddFieldMap("InitialPassword", "unixUserPassword");
            AddFieldMap("DistinguishedName", "distinguishedName");
        }

        #endregion

        #region Properties


        public String UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        public string EmployeeId
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public String FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public string EmployeeType
        {
            get { return _employeeType; }
            set { _employeeType = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public byte[] InitialPassword
        {
            get { return _initialPassword; }
            set { _initialPassword = value; }
        }

        public LcpsAdsUserAccountControl UserAccountControl
        {
            get { return _userAccountControl; }
            set { _userAccountControl = value; }
        }

        public string DistinguishedName
        {
            get { return _distinguishedName; }
            set { _distinguishedName = value; }
        }

        public LcpsAdsOu OrganizationalUnit
        {
            get { return new LcpsAdsOu(DirectoryEntry.Parent); }
        }

        public string OUName
        {
            get { return OrganizationalUnit.Name; }
        }

        public string[] HomeFolders
        {
            get { return _homeFolders; }
        }

        #endregion

        #region Formats

        public string FormatDisplayName()
        {
            return LastName + ", " + FirstName;
        }

        public string FormatDescription()
        {
            return Location + " " + EmployeeType + " " + Title + " Active: " + IsActive.ToString() + " Updated On:" + DateTime.Now.ToString();
        }


        public string FormatDistinguishedName()
        {
            return string.Format(@"CN={0}\, {1} ({2})", LastName, FirstName, UserName);
        }

        public string FormatPrincipalName()
        {
            ApplicationBase a = LcpsDbContext.DefaultApp;
            return string.Format("{0}@{1}", UserName, DefaultApp.LDAPDomainFQN);
        }
        #endregion


        #region Get

        public static LcpsAdsUser Get(string userName)
        {
            string filter = string.Format("(&(samAccountName={0}))", userName);
            DirectoryEntry d = Find(filter, LcpsAdsDomain.Default.DirectoryEntry);
            LcpsAdsUser u = new LcpsAdsUser(d);
            return u;
        }

        #endregion

        #region Fill

        public override void FillFromDirectoryEntry()
        {
            base.FillFromDirectoryEntry();

            if ((_userAccountControl & LcpsAdsUserAccountControl.ACCOUNTDISABLE) == LcpsAdsUserAccountControl.ACCOUNTDISABLE)
                _isActive = false;
            else
                _isActive = true;
        }

        public override void FillDirectoryEntry(bool commitChanges)
        {
            if (_isActive)
                _userAccountControl = LcpsAdsUserAccountControl.NORMAL_ACCOUNT;
            else
                _userAccountControl = LcpsAdsUserAccountControl.NORMAL_ACCOUNT & LcpsAdsUserAccountControl.ACCOUNTDISABLE;

            base.FillDirectoryEntry(commitChanges);
        }


        #endregion

        #region Group Membership

        public bool IsMemberOf(LcpsAdsGroup g)
        {
            foreach (string gdn in DirectoryEntry.Properties["memberOf"])
            {
                if (gdn.ToLower() == g.DistinguishedName.ToLower())
                    return true;
            }
            return false;
        }

        public LcpsAdsGroup[] GetGroupMembership()
        {
            List<LcpsAdsGroup> l = new List<LcpsAdsGroup>();
            var groups = this.DirectoryEntry.Properties["memberOf"].Value;
            if (groups == null)
                return l.ToArray();

            if (groups.GetType() == typeof(string))
            {
                string p = "LDAP://" + DefaultApp.LDAPDomainFQN + "/" + (string)groups;
                LcpsAdsGroup g = new LcpsAdsGroup(p);
                l.Add(g);
            }
            else
            {
                ICollection gc = groups as ICollection;
                foreach (string g in gc)
                {
                    string p = "LDAP://" + DefaultApp.LDAPDomainFQN + "/" + g;
                    LcpsAdsGroup grp = new LcpsAdsGroup(p);
                    l.Add(grp);
                }

            }
            return l.ToArray();
        }

        public void ClearGroupMembership()
        {
            try
            {
                foreach (LcpsAdsGroup g in GetGroupMembership())
                {
                    g.RemoveMember(this);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error clearing gorup membership for {0}", UserName), ex);
            }
        }

        #endregion

       

        #region Reports

        public static LcpsAdsUser[] GetActiveUsersWithNoEmail(DirectoryEntry lookIn, bool recursive)
        {
            string filter = "(&(objectCategory=user)(!mail=*)(!userAccountControl:1.2.840.113556.1.4.803:=2))";
            return GetUsers(filter, lookIn, recursive);
        }

        public static LcpsAdsUser[] GetInactiveUsers(DirectoryEntry lookIn, bool recursive)
        {
            string filter = "(&(objectCategory=user)(userAccountControl:1.2.840.113556.1.4.803:=2))";
            return GetUsers(filter, lookIn, recursive);
        }


        public static LcpsAdsUser[] GetUsers(string filter, DirectoryEntry lookIn, bool recursive)
        {
            List<LcpsAdsUser> l = new List<LcpsAdsUser>();


            using (DirectorySearcher s = new DirectorySearcher(lookIn, filter))
            {
                if (recursive)
                    s.SearchScope = SearchScope.Subtree;
                else
                    s.SearchScope = SearchScope.OneLevel;

                foreach (SearchResult r in s.FindAll())
                {
                    DirectoryEntry e = r.GetDirectoryEntry();
                    LcpsAdsUser u = new LcpsAdsUser(e);
                    l.Add(u);
                }
            }

            return l.ToArray();

        }

        #endregion

       

        #region Password

        public void Resetpassword(string newPassword)
        {
            try
            {
                DirectoryEntry.Invoke("setpassword", newPassword);
                DirectoryEntry.CommitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not reste password", ex);
            }
        }

        public void ForcePasswordReset()
        {
            try
            {
                DirectoryEntry.InvokeSet("PasswordExpired", 1);
                DirectoryEntry.CommitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Error setting password must reset at login", ex);
            }
        }

        public void SetPasswordWillNotExpire(bool expire)
        {
            if(expire)
            {
                if (!UserAccountControl.HasFlag(LcpsAdsUserAccountControl.DONT_EXPIRE_PASSWD))
                {
                    UserAccountControl = UserAccountControl | LcpsAdsUserAccountControl.DONT_EXPIRE_PASSWD;
                }
            }
            else
            {
                if(UserAccountControl.HasFlag(LcpsAdsUserAccountControl.DONT_EXPIRE_PASSWD))
                {
                    UserAccountControl -= LcpsAdsUserAccountControl.DONT_EXPIRE_PASSWD;
                }
            }

            DirectoryEntry.InvokeSet("userAccountControl", UserAccountControl);
            DirectoryEntry.CommitChanges();
        }

        #endregion

        #region Update

        public void Update()
        {
            string[] readOnlyFields = new string[] { "distinguishedname" };

            foreach (string k in AdsToLcpsFieldMaps.Keys)
            {
                try
                {


                    PropertyInfo p = (from PropertyInfo x in this.GetType().GetProperties()
                                      where x.Name.ToLower() == AdsToLcpsFieldMaps[k].ToLower()
                                      & x.CanWrite & x.CanRead
                                      select x).SingleOrDefault();

                    if (p == null)
                        throw new Exception(string.Format("A property named {0} does not exist in {1}", AdsToLcpsFieldMaps[k], this.GetType().ToString()));

                    if (!readOnlyFields.Contains(k.ToLower()))
                    {
                        object v = p.GetValue(this, null);
                        UpdateProperty(k, DirectoryEntry, v);
                    }


                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Error updating user {0}. Could not set property {1}", UserName, k), ex);
                }
            }

            try
            {
                this.DirectoryEntry.CommitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Could not commit changes to user {0}", UserName), ex);
            }
        }

        #endregion

        #region Redirected Folder

        public void GrantfullAccessToFolder(string folderPath, DirectoryEntry de)
        {
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(folderPath);
                DirectorySecurity dSecurity = dInfo.GetAccessControl();

                byte[] sidByte = de.InvokeGet("objectSid") as byte[];
                SecurityIdentifier sid = new SecurityIdentifier(sidByte, 0);


                string id = DefaultApp.LDAPDomain + "\\" + UserName;

                dSecurity.AddAccessRule(new FileSystemAccessRule(sid, FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                dInfo.SetAccessControl(dSecurity);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error granting user {0} full persmissions to folder {1}", UserName, folderPath), ex);
            }
        }

        #endregion
    }

}

