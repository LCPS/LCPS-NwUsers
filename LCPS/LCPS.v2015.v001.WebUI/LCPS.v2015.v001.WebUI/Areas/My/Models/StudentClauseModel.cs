using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

using Anvil.v2015.v001.Domain.Entities;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.Filters;
using LCPS.v2015.v001.NwUsers.Students;


namespace LCPS.v2015.v001.WebUI.Areas.My.Models
{
    public class StudentClauseModel : StudentFilterClause
    {
        private LcpsDbContext _dbContext;

        

        public LcpsDbContext DbContext
        {
            get
            {
                if (_dbContext == null)
                    _dbContext = new LcpsDbContext();
                return _dbContext;
            }
        }

        public List<SelectListItem> GetBuildings()
        {
            try
            {
                List<HRBuilding> bb = DbContext.Buildings.OrderBy(x => x.Name).ToList();
                return bb.Select(x => new SelectListItem() { Text = x.Name, Value = x.BuildingKey.ToString() }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get buildings from database", ex);
            }
        }


        public List<SelectListItem> GetInstructionalLevels(Guid? buildingId)
        {
            try
            {
                List<InstructionalLevel> ll;

                if (buildingId == null)
                    ll = DbContext.InstructionalLevels
                        .OrderBy(x => x.InstructionalLevelName)
                        .ToList();
                else
                    ll = (from InstructionalLevel x in DbContext.InstructionalLevels 
                          join Student s in DbContext.Students on x.InstructionalLevelKey equals s.BuildingKey
                          where s.BuildingKey.Equals(buildingId.Value)
                          orderby x.InstructionalLevelName
                          select x).ToList();

                return ll.Select(x => new SelectListItem()
                    {
                        Text = x.InstructionalLevelName,
                        Value = x.InstructionalLevelId.ToString()
                    }).ToList();
                    
            }
            catch(Exception ex)
            {
                throw new Exception("Could not get Instructional Level", ex);
            }
        }

    }
}