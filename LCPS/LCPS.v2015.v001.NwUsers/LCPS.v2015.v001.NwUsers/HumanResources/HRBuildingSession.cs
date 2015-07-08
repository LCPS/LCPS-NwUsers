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

using Anvil.v2015.v001.Domain.Entities;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;

#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources
{
    public class HRBuildingSession : ImportFileTSV
    {

        public HRBuildingSession()
            : base(
            addIfNotExists: true,
            updateIfExists: false,
            itemType: typeof(HRBuildingCandidate),
            viewTitle: "Import Buildings",
            area: "HumanResources",
            controller: "HRBuildings",
            action: "Preview",
            viewLayoutPath: "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml")
        {
            SessionId = Guid.NewGuid();
        }

        public HRBuildingSession(ImportSession s)
            :base(s)
        { 

        }


        public override void DetectConflicts()
        {
            List<HRBuildingCandidate> candidates = Candidates.Select(x => x.SourceItem).Cast<HRBuildingCandidate>().ToList();
            List<HRBuildingCandidate> dups = candidates.Duplicates(true).Cast<HRBuildingCandidate>().ToList();
            foreach (HRBuildingCandidate dup in dups)
            {
                Candidates[dup.LineIndex].Comment = "Duplicates were found for employee Id";
                Candidates[dup.LineIndex].EntityStatus = ImportEntityStatus.Duplicate;
                Candidates[dup.LineIndex].Record();
            }
        }
    }
}
