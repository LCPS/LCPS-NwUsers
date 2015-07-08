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
using LCPS.v2015.v001.NwUsers.Importing;
using System.Web;
using System.Web.Mvc;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;

using Anvil.v2015.v001.Domain.Entities;

#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.HRImport
{
    [Serializable]
    public class HREmployeeTypeSession : ImportFileTSV
    {

        public HREmployeeTypeSession()
            : base(
            addIfNotExists: true,
            updateIfExists: false,
            itemType: typeof(HREmployeeTypeCandidate),
            viewTitle: "Import Employee Types",
            area: "HumanResources",
            controller: "HREmployeeTypes",
            action: "Preview",
            viewLayoutPath: "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml")
        {
            SessionId = Guid.NewGuid();
        }

        public HREmployeeTypeSession(ImportSession s )
            :base(s)
        {}
         
        public override void DetectConflicts()
        {
            List<HREmployeeTypeCandidate> candidates = Candidates.Select(x => x.SourceItem).Cast<HREmployeeTypeCandidate>().ToList();
            List<HREmployeeTypeCandidate> dups = candidates.Duplicates(true).Cast<HREmployeeTypeCandidate>().ToList();
            foreach(HREmployeeTypeCandidate dup in dups)
            {
                Candidates[dup.LineIndex].Comment = "Duplicates were found for employee Id";
                Candidates[dup.LineIndex].EntityStatus = ImportEntityStatus.Duplicate;
                Candidates[dup.LineIndex].Record();
            }
        }
    }
}
