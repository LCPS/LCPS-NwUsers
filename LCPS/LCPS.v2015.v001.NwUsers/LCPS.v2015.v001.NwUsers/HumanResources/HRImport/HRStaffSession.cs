using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Anvil.v2015.v001.Domain.Entities;

using LCPS.v2015.v001.NwUsers.Importing;
using LCPS.v2015.v001.NwUsers.Infrastructure;
using LCPS.v2015.v001.NwUsers.HumanResources.Staff;
using LCPS.v2015.v001.NwUsers.HumanResources.HRImport;

namespace LCPS.v2015.v001.NwUsers.HumanResources.HRImport
{
    public class HRStaffSession : ImportFileTSV
    {
        public HRStaffSession()
            : base(
            addIfNotExists: true,
            updateIfExists: false,
            itemType: typeof(HRStaffCandidate),
            viewTitle: "Import Employee Types",
            area: "HumanResources",
            controller: "HRStaff",
            action: "Preview",
            viewLayoutPath: "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml")
        {}

        public HRStaffSession(ImportSession s)
            : base(s)
        { }

        public override void DetectConflicts()
        {
            List<HRStaffCandidate> candidates = Candidates.Select(x => x.SourceItem).Cast<HRStaffCandidate>().ToList();
            List<HRStaffCandidate> dups = candidates.Duplicates(true).Cast<HRStaffCandidate>().ToList();
            foreach (HRStaffCandidate dup in dups)
            {
                Candidates[dup.LineIndex].Comment = "Duplicates were found for employee Id";
                Candidates[dup.LineIndex].EntityStatus = ImportEntityStatus.Duplicate;
                Candidates[dup.LineIndex].Record();
            }
        }
    }
}
