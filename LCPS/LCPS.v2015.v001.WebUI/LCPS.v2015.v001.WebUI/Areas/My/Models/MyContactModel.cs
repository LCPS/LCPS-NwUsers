
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Filters;
//using LCPS.v2015.v001.WebUI.Areas.Filters.Models;
using Anvil.v2015.v001.Domain.Entities;

namespace LCPS.v2015.v001.WebUI.Areas.My.Models
{
    public class MyContactModel
    {
        private LcpsDbContext _dbContext;
        private ApplicationUser _user;

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();

                return _dbContext;
            }
        }

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_user == null)
                    _user = DbContext.Users.First(x => x.UserName.ToLower() == HttpContext.Current.User.Identity.Name.ToLower());
                return _user;
            }
        }

        public MemberFilter CurrentFilter { get; set; }

        public List<MemberFilter> GetFilters()
        {
            try
            {
                return DbContext.MemberFilters.Where(x => 
                    x.AntecedentId.Equals(new Guid(CurrentUser.Id)))
                    .OrderBy(x => x.FilterClass.ToString() + x.Title)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting your filters", ex);
            }
        }

         
    }
}