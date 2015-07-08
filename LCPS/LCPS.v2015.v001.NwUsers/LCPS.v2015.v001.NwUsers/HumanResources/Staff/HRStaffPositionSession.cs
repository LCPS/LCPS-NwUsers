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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Anvil.v2015.v001.Domain.Entities;
using Anvil.v2015.v001.Domain.Exceptions;



#endregion

namespace LCPS.v2015.v001.NwUsers.HumanResources.Staff
{
    public class HRStaffPositionSession : ImportFileTSV
    {

        public HRStaffPositionSession()
            : base(
                addIfNotExists: true,
                updateIfExists: false,
                itemType: typeof(HRStaffPositionCandidate),
                viewTitle: "Import Staff Positions",
                area: "HumanResources",
                controller: "HRStaffPositions",
                action: "Preview",
                viewLayoutPath: "~/Areas/HumanResources/Views/Shared/_HumanResourcesLayout.cshtml")
        {
            SessionId = Guid.NewGuid();
        }


        public HRStaffPositionSession(ImportSession s)
            :base(s)
        { }

        public override void DetectConflicts()
        {
            try
            {
                List<HRStaffPositionCandidate> candidates = Candidates.Select(x => x.SourceItem).Cast<HRStaffPositionCandidate>().ToList();
                List<HRStaffPositionCandidate> dups = candidates.Duplicates(true).Cast<HRStaffPositionCandidate>().ToList();
                foreach (HRStaffPositionCandidate dup in dups)
                {
                    Candidates[dup.LineIndex].Comment = "Duplicates were found for job title Id";
                    Candidates[dup.LineIndex].EntityStatus = ImportEntityStatus.Duplicate;
                    Candidates[dup.LineIndex].Record();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Could not detect conflicts", ex);
            }
        }
    }

}
