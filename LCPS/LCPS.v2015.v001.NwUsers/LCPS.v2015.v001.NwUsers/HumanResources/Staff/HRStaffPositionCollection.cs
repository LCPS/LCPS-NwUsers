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

using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;


#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public class HRStaffPositionCollection :IList<HRStaffPosition>
    {
        private Guid _staffLinkId;
        private LcpsDbContext db = new LcpsDbContext();
        private List<HRStaffPosition> _list = new List<HRStaffPosition>();

        public HRStaffPositionCollection()
        { }

        public HRStaffPositionCollection(Guid staffLinkId)
        {
            _staffLinkId = staffLinkId;
        }

        
        public void Add(string staffId, string buildingId, string employeeTypeId, string jobTitleId, bool active)
        {
            _list.Add(new HRStaffPosition(staffId, buildingId, employeeTypeId, jobTitleId, active));
        }

        public void Add(HRStaffPosition item)
        {
            _list.Add(item);
          //  db.StaffPositions.Add(item);
            db.SaveChanges();
        }

        public void AddRange(HRStaffPosition[] items)
        {
            _list.AddRange(items);
        }

        public int IndexOf(string buildingId, string employeeTypeId, string jobTitleId)
        {
            HRStaffPosition p = _list.FirstOrDefault(x => x.Building.BuildingId == buildingId &
                                x.EmployeeType.EmployeeTypeId == employeeTypeId &
                                x.JobTitle.JobTitleId == jobTitleId);
            if (p == null)
                return -1;
            else
                return IndexOf(p);
        }



        public int IndexOf(HRStaffPosition item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, HRStaffPosition item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public HRStaffPosition this[int index]
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

        public void Clear()
        {
            _list.Clear();
        }


        public bool Contains(string buildingId, string employeeTypeId, string jobTitleId)
        {
            int count = _list.Where(x => x.Building.BuildingId == buildingId &
                                            x.EmployeeType.EmployeeTypeId == employeeTypeId &
                                            x.JobTitle.JobTitleId == jobTitleId).Count();
            return (count > 0);
        }

        public void Refresh()
        {
            Refresh(_staffLinkId);
        }

        private void Refresh(string staffId)
        {
           // _list = db.StaffPositions.Where(x => x.StaffId == staffId).ToList();
        }

        private void Refresh(Guid staffLinkId)
        {
           // _list = db.StaffPositions.Where(x => x.StaffLinkId.Equals(staffLinkId)).ToList();
        }

        public bool Contains(HRStaffPosition item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(HRStaffPosition[] array, int arrayIndex)
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

        public bool Remove(HRStaffPosition item)
        {
            return _list.Remove(item);
        }

        public IEnumerator<HRStaffPosition> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
    }
}
